using PlainMQServer.Models;
using System.Collections.Concurrent;

namespace PlainMQServer.ThreadManagement
{
    internal class ManagedThreadPool
    {
        internal static EventQueue<ThreadEvent> GlobalEventQueue { get; } = new EventQueue<ThreadEvent>();
        private static ConcurrentStack<IManagedThread> _pool;

        internal static void Init()
        {
            _pool = new ConcurrentStack<IManagedThread>();
        }

        internal static bool AddToPool(IManagedThread t)
        {
            if(_pool.FirstOrDefault(mt => mt.ID == t.ID) == null)
            {
                t.ThreadAction();
                _pool.Push(t);
                return true;
            }
            return false;
        }

        internal static bool Broadcast(ThreadEvent tEvent)
        {
            GlobalEventQueue.Enqueue(tEvent);
            return true;
        }
    }
}
