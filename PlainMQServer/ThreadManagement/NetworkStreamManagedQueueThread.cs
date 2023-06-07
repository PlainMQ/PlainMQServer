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
        int currentWait = 2;
        internal NetworkStream NStream { get; set; }

        public NetworkStreamManagedQueueThread(NetworkStream nStream)
        {
            NStream = nStream;
            InvokeClass = Models.Enums.ThreadClass.BROADCAST;
            QueueAction = (ThreadEvent te) =>
            {
                ThreadMethod(te);
            };

            //_thread = new Thread(() => Action.Invoke(nStream));
            //_thread.Start();
        }

        private void ThreadMethod(ThreadEvent te)
        {
            try
            {

                if (te.EventPayload is PlainMessage)
                {
                    PlainMessage msg = (PlainMessage)te.EventPayload;
                    if (NStream.CanWrite)
                    {
                        NStream.Write(msg.ToMessageBytes());
                        currentWait = 2;
                    } 
                    else if(NStream.Socket.Connected == false){
                        NStream.Close();
                        Status = Models.Enums.ManagedThreadStatus.ERROR;
                    } 
                    else {
                        Thread.Sleep(currentWait++);
                        ThreadMethod(te);
                    }
                    NStream.Flush();
                }
                else throw new Exception("Unknown type being processed on a known type thread");            
            }
            catch(Exception ex)
            {
                if (NStream.CanWrite)
                {
                    Thread.Sleep(currentWait++);
                    ThreadMethod(te);
                }
                else
                {
                    NStream.Close();
                    Status = Models.Enums.ManagedThreadStatus.ERROR;
                    InvokeClass = CancelClass;
                    Console.WriteLine($"Forcibly closed Thread: {ID}");
                }
            }
        }

        public void Dispose()
        {            
            NStream?.Dispose();
            InvokeClass = CancelClass;
        }
    }
}
