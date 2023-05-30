﻿using PlainMQServer.Models;
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
        internal Queue<ThreadEvent> LocalQueue { get; set; }        

        public ManagedQueueThread()
        {
            LocalQueue = new Queue<ThreadEvent>();

            Action = (object? o) =>
            {
                while(Status != Models.Enums.ManagedThreadStatus.ERROR)
                {
                    if(o is ThreadEvent)
                    {
                        if(LocalQueue.Any())
                        {
                            QueueAction?.Invoke(LocalQueue.Dequeue());
                        }
                    }
                }
            };

            _thread = new Thread(Action);
            _thread.Start();
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
                    LocalQueue.Enqueue(ubEvent);
                }
            };
        }

        /// <summary>
        /// The Action that occurs when an item is added to the LocalQueue
        /// </summary>
        public Action<ThreadEvent>? QueueAction { get; set; }


    }
}
