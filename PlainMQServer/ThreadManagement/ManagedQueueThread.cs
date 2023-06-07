using PlainMQServer.Models;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace PlainMQServer.ThreadManagement
{
    /// <summary>
    /// Specialization of a ManagedThreadBase which maintains an internal queue
    /// This queue will then trigger a Local Action with the idea of events being processed
    /// in the local queue.
    /// 
    /// On creation, each thread will manage it's own LocalQueue
    /// </summary>
    internal class ManagedQueueThread : ManagedThreadBase
    {
        internal ConcurrentQueue<ThreadEvent> LocalQueue { get; set; }        

        public ManagedQueueThread()
        {
            LocalQueue = new ConcurrentQueue<ThreadEvent>();

            //Action = (object? o) =>
            //{
            //    while(Status != Models.Enums.ManagedThreadStatus.ERROR)
            //    {
            //        if(o is ThreadEvent)
            //        {
            //            if(LocalQueue.Any())
            //            {
            //                if(LocalQueue.TryDequeue(out var threadEvent))
            //                {
            //                    QueueAction?.Invoke(threadEvent);
            //                }
            //            }
            //        }
            //    }
            //};

            //_thread = new Thread(Action);
            //_thread.Start();
        }

        public override void ThreadAction()
        {
            ManagedThreadPool.GlobalEventQueue.QueueChange += (object? sender, CollectionChangeEventArgs args) =>
            {
                if (args.Element == null)
                    throw new InvalidDataException("Failed to subscribe to GlobalEventQueue");

                ThreadEvent ubEvent = (ThreadEvent)args.Element;

                if (ubEvent.Class == InvokeClass && ubEvent.InitiatorID != ID)
                {
                    _thread = new Thread(() => QueueAction?.Invoke(ubEvent));
                    _thread.Start();
                    Status = Models.Enums.ManagedThreadStatus.RUNNING;
                }
            };
        }

        /// <summary>
        /// The Action that occurs when an item is added to the LocalQueue
        /// </summary>
        public Action<ThreadEvent>? QueueAction { get; set; }


    }
}
