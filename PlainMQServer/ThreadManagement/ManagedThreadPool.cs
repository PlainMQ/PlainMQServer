using PlainMQServer.Models;
using System.Collections.Concurrent;

namespace PlainMQServer.ThreadManagement
{
    /// <summary>
    /// Wrapper class of a ConcurrentDictionary & EventQueue 
    /// 
    /// Used to create/destroy threads as needed
    /// </summary>
    internal class ManagedThreadPool
    {
        internal static EventQueue<ThreadEvent> GlobalEventQueue { get; } = new EventQueue<ThreadEvent>();
        private static ConcurrentDictionary<int, IManagedThread> _pool;

        internal static void Init()
        {
            _pool = new ConcurrentDictionary<int, IManagedThread>();
        }

        internal static bool AddToPool(IManagedThread t)
        {            
            t.ThreadAction();
            return _pool.TryAdd(t.ID, t);                
        }

        internal static bool Broadcast(ThreadEvent tEvent)
        {
            GlobalEventQueue.Enqueue(tEvent);
            return true;
        }
    }
}
