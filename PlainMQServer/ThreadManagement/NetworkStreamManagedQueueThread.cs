using PlainMQServer.Models;
using System.Net.Sockets;

namespace PlainMQServer.ThreadManagement
{
    /// <summary>
    /// Specialization of a ManagedQueueThread which has a NetworkStream as a property.
    /// 
    /// Desired functionality is to write to the resident NetworkStream
    /// </summary>
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
                        NStream.Flush();
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
