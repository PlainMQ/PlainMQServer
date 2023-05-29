using System.Collections.Concurrent;
using System.ComponentModel;

namespace PlainMQServer.Models
{
    public class EventQueue<T>
    {
        public ConcurrentQueue<T> Queue { get; set; }

        public event CollectionChangeEventHandler QueueChange;

        public EventQueue()
        {
            Queue = new ConcurrentQueue<T>();
        }

        public void Enqueue(T obj)
        {
            Queue.Enqueue(obj);
            var cArgs = new CollectionChangeEventArgs(CollectionChangeAction.Add, obj);
            QueueChange.Invoke(this, cArgs);
        }
    }
}
