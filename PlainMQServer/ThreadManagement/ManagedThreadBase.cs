using PlainMQServer.Models;
using PlainMQServer.Models.Enums;
using System.ComponentModel;

namespace PlainMQServer.ThreadManagement
{
    public class ManagedThreadBase : IManagedThread
    {
        public ManagedThreadStatus Status { get; set; }

        public ParameterizedThreadStart Action { get; set; }

        public ThreadClass InvokeClass { get; set; }

        public ThreadClass CancelClass { get; } = ThreadClass.TERMINATE;

        public int ID { get; set; }
        public string Name { get; set; }

        internal Thread _thread { get; set; }

        public virtual void ThreadAction()
        {
            ManagedThreadPool.GlobalEventQueue.QueueChange += (object sender, CollectionChangeEventArgs args) =>
            {
                ThreadEvent ubEvent = (ThreadEvent)args.Element;

                if (ubEvent.Class == InvokeClass)
                {
                    _thread = new Thread(() => Action.Invoke(ubEvent));
                    _thread.Start();
                }                
            };

        }
    }
}
