using PlainMQServer.Models;
using System.Net;
using System.Net.Sockets;

TcpListener server = null;
List<NetworkStream> Connections = new List<NetworkStream>();

try
{
    Int32 port = 13000;
    IPAddress localhost = IPAddress.Parse("127.0.0.1");
    server = new TcpListener(localhost, port);
    server.Start();

    Console.WriteLine("Server started...");

    bool terminate = false;

    while(!terminate)
    {
        TcpClient client = server.AcceptTcpClient();

        NetworkStream networkStream = client.GetStream();
        Connections.Add(networkStream);

        Console.WriteLine($"Connection received - {networkStream.Socket.RemoteEndPoint}");

        Thread readThread = new Thread(() => ReadFromStream(networkStream));
        readThread.Start();
    }

}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}


void ReadFromStream(NetworkStream nStream)
{
    try
    {
        int i;
        byte[] lenByte = new byte[sizeof(int)];        

        while((i = nStream.Read(lenByte)) != 0)
        {
            if (i != sizeof(int))
                throw new Exception("unhandled message type");            

            PlainMessage pMsg = new PlainMessage(BitConverter.ToInt32(lenByte));           

            nStream.Read(pMsg.BODY, 0, pMsg.LENGTH);

            Broadcast(pMsg);
            nStream.Flush();
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Connection lost from {nStream.Socket.RemoteEndPoint}");
        Connections.Remove(nStream);
    }
}

void Broadcast(PlainMessage pMsg)
{
    Connections.AsParallel()
        .ForAll((nStream) =>
        {
            if(nStream.CanWrite)
                nStream.Write(pMsg.ToMessageBytes());
        });
}