using PlainMQServer.Models;
using PlainMQServer.Models.Enums;
using System.ComponentModel;

namespace PlainMQServer.ThreadManagement
{
    public class ManagedThread : IManagedThread
    {
        public ManagedThreadStatus Status { get; private set; }

        public ParameterizedThreadStart Action { get; set; }

        public ThreadClass InvokeClass { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }

        private Thread _thread { get; set; }

        public void ThreadAction(object? o)
        {
            var state = ManagedThreadStatus.IDLE;

            ManagedThreadPool.GlobalEventQueue.QueueChange += (object sender, CollectionChangeEventArgs args) =>
            {
                var ubEvent = (ThreadEvent)args.Element;

                if (ubEvent.Class == InvokeClass)
                    Action.Invoke(ubEvent);
            };

        }
    }
}
