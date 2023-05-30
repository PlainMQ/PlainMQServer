using PlainMQServer.Models;
using PlainMQServer.ThreadManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = null;
List<NetworkStream> Connections = new List<NetworkStream>();
bool shutdown = false;
int globalBroadcastID = 1;

ManagedThreadPool.Init();

ManagedThreadBase Main = new ManagedThreadBase
{
    Action = (object? o) =>
    {
        Int32 port = 13000;
        IPAddress localhost = IPAddress.Parse("127.0.0.1");
        server = new TcpListener(localhost, port);
        server.Start();

        Console.WriteLine("Server started...");

        ThreadEvent te = (ThreadEvent)o;
        PlainMessage pMsg = (PlainMessage)te.EventPayload;

        Console.WriteLine(Encoding.UTF8.GetString(pMsg.BODY));

        while (!shutdown)
        {
            TcpClient client = server.AcceptTcpClient();

            NetworkStream networkStream = client.GetStream();

            Console.WriteLine($"Connection received - {networkStream.Socket.RemoteEndPoint}");

            NetworkStreamManagedQueueThread nStreamThread = new NetworkStreamManagedQueueThread(networkStream);
            nStreamThread.ID = globalBroadcastID++;
            nStreamThread.Name = $"CONN|{networkStream.Socket.RemoteEndPoint}";

            ManagedThreadPool.AddToPool(nStreamThread);

            Thread readThread = new Thread(() => ReadFromStream(nStreamThread));
            readThread.Start();
        }
    },

    ID = 0,

    InvokeClass = PlainMQServer.Models.Enums.ThreadClass.MAIN,
    Name = "MainThread",
};

ManagedThreadPool.AddToPool(Main);

ManagedThreadPool.Broadcast(new ThreadEvent
{
    Class = PlainMQServer.Models.Enums.ThreadClass.MAIN,
    EventPayload = new PlainMessage(Encoding.UTF8.GetBytes("INTITED"), 7)
});

void ReadFromStream(NetworkStreamManagedQueueThread nStreamThread)
{
    try
    {
        int i;
        byte[] lenByte = new byte[sizeof(int)];

        while ((i = nStreamThread.NStream.Read(lenByte)) != 0)
        {
            if (i != sizeof(int))
                throw new Exception("unhandled message type");

            PlainMessage pMsg = new PlainMessage(BitConverter.ToInt32(lenByte));

            nStreamThread.NStream.Read(pMsg.BODY, 0, pMsg.LENGTH);

            ManagedThreadPool.Broadcast(new ThreadEvent
            {
                InitiatorID = nStreamThread.ID,
                Class = PlainMQServer.Models.Enums.ThreadClass.BROADCAST,
                EventPayload = pMsg
            });

            nStreamThread.NStream.Flush();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Connection lost from {nStreamThread.NStream.Socket.RemoteEndPoint}");
        Connections.Remove(nStreamThread.NStream);
    }
}