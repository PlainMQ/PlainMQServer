using System.Collections.Concurrent;
using System.ComponentModel;

namespace PlainMQServer.Models
{
    /// <summary>
    /// Simple event bound queue to be used in the context of the GlobalEventQueue
    /// </summary>
    /// <typeparam name="T">Type of object found in the EventQueue</typeparam>
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
