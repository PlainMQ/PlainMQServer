using PlainMQServer.Models;
using System.Net.Sockets;

namespace PlainMQServer.ThreadManagement
{
    internal class NetworkStreamManagedQueueThread : ManagedQueueThread, IDisposable
    {
        internal NetworkStream NStream { get; set; }

        public NetworkStreamManagedQueueThread(NetworkStream nStream)
        {
            NStream = nStream;
            InvokeClass = Models.Enums.ThreadClass.BROADCAST;
            Action = (object? o) =>
            {
                ThreadMethod();
            };

            _thread = new Thread(() => Action.Invoke(nStream));
            _thread.Start();
        }

        private void ThreadMethod()
        {
            while(Status != Models.Enums.ManagedThreadStatus.ERROR)
            {
                if (LocalQueue.Any())
                {
                    var obj = LocalQueue.Dequeue();

                    if(obj.EventPayload is PlainMessage)
                    {
                        PlainMessage msg = (PlainMessage)obj.EventPayload;
                        NStream.Write(msg.ToMessageBytes());
                    }
                }
                else
                {
                    Status = Models.Enums.ManagedThreadStatus.IDLE;
                    Thread.Sleep(10);
                }
            }
        }

        public void Dispose()
        {
            NStream?.Dispose();
        }
    }
}
