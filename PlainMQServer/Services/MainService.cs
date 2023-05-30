using PlainMQServer.Models;
using PlainMQServer.ThreadManagement;
using System.Net;
using System.Net.Sockets;

namespace PlainMQServer.Services
{
    /// <summary>
    /// Main execution service, all methods that are considered "main" will live in this service
    /// 
    /// Should correlate to the ThreadClass.Main enum value
    /// </summary>
    internal class MainService
    {
        internal ServerConfig Config { get; set; }

        private readonly string PORT_CHK = "port=";
        private readonly string ADDR_CHK = "addr=";
        private readonly string TIMEOUT_CHK = "timeout=";
        private int GlobalThreadId = 0;
        internal Action<object?>? MainAction { get; set; }
        internal TcpListener? Server { get; set; }
        internal bool Terminate { get; set; } = false;
        private delegate void DelMainActions();

        /// <summary>
        /// Constructor for creating the MainService via command line args
        /// </summary>
        /// <param name="args"></param>
        public MainService(string[] args)
        {
            Config = new ServerConfig();
            ReadArgsIntoConfig(args);
            SetupMainAction();
        }

        private void ReadArgsIntoConfig(string[] args)
        {
            if(args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                    SetupConfig(args[i]);
            }
        }

        private void SetupConfig(string v)
        {
            int start = v.LastIndexOf("=") + 1;

            if (start == -1)
                return;

            int end = v.Length - start;


            if (v.Contains(PORT_CHK))
            {
                Config.ServerPort = v.Substring(start, end);
            }

            if(v.Contains(ADDR_CHK))
            {
                Config.ServerAddress = v.Substring(start, end);
            }

            if(v.Contains(TIMEOUT_CHK))
            {
                Config.Timeout = v.Substring(start, end);
            }
        }

        private void SetupMainAction()
        {
            MainAction = (object? o) =>
            {
                DelMainActions init = InitServer;
                DelMainActions startThread = StartThreadLoop;

                init += startThread;

                init.Invoke();
            };            
        }
        
        private void StartThreadLoop()
        {
            while (!Terminate)
            {
                if (Server is null)
                    break;

                TcpClient client = Server.AcceptTcpClient();

                NetworkStream networkStream = client.GetStream();

                Console.WriteLine($"Connection received - {networkStream.Socket.RemoteEndPoint}");

                NetworkStreamManagedQueueThread nStreamThread = new NetworkStreamManagedQueueThread(networkStream);
                nStreamThread.ID = GlobalThreadId++;
                nStreamThread.Name = $"CONN|{networkStream.Socket.RemoteEndPoint}";

                ManagedThreadPool.AddToPool(nStreamThread);

                Thread readThread = new Thread(() => ReadFromStream(nStreamThread));
                readThread.Start();
            }
        }

        private void InitServer()
        {
            if (Config.ServerPort == null)
            {
                Config.ServerPort = "13000";
            }

            Int32 port = Int32.Parse(Config.ServerPort);
            string Address = Config.ServerAddress ?? "127.0.0.1";

            Server = new TcpListener(IPAddress.Parse(Address), port);
            Server.Start();

            Console.WriteLine("Server started...");
        }

        private void ReadFromStream(NetworkStreamManagedQueueThread nStreamThread)
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

                    if (pMsg.BODY != null)
                    {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection lost from {nStreamThread.NStream.Socket.RemoteEndPoint} ex - {ex.Message}");
                ManagedThreadPool.RemoveFromPool(nStreamThread);
                nStreamThread.Dispose();
            }
        }
    }
}
