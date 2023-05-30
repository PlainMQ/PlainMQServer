using PlainMQServer.Models;
using System.Collections.Concurrent;

namespace PlainMQServer.ThreadManagement
{
    /// <summary>
    /// Static wrapper class of a ConcurrentDictionary & EventQueue 
    /// 
    /// Used to create/destroy threads as needed
    /// </summary>
    internal static class ManagedThreadPool
    {
        internal static EventQueue<ThreadEvent> GlobalEventQueue { get; } = new EventQueue<ThreadEvent>();
        private static ConcurrentDictionary<int, IManagedThread>? _pool;

        internal static void Init()
        {
            _pool = new ConcurrentDictionary<int, IManagedThread>();
        }

        internal static bool AddToPool(IManagedThread t)
        {
            if (_pool != null)
            {
                t.ThreadAction();
                return _pool.TryAdd(t.ID, t);
            }
            else throw new Exception("ManagedThreadPool is null somehow");

        }

        internal static bool Broadcast(ThreadEvent tEvent)
        {
            GlobalEventQueue.Enqueue(tEvent);
            return true;
        }
    }
}
