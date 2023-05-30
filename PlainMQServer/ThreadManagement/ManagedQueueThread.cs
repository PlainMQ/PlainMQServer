using PlainMQServer.Models;
using System.ComponentModel;

namespace PlainMQServer.ThreadManagement
{
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
                            QueueAction.Invoke(LocalQueue.Dequeue());
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
                ThreadEvent ubEvent = (ThreadEvent)args.Element;

                if (ubEvent.Class == InvokeClass && ubEvent.InitiatorID != ID)
                {
                    LocalQueue.Enqueue(ubEvent);
                }
            };
        }

        public Action<ThreadEvent> QueueAction { get; set; }


    }
}
