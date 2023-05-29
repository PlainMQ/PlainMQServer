using PlainMQServer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainMQServer.ThreadManagement
{
    internal class ManagedThreadPool
    {
        internal static EventQueue<ThreadEvent> GlobalEventQueue { get; } = new EventQueue<ThreadEvent>();
        private static ConcurrentStack<ManagedThread> _pool;

        internal static void Init()
        {
            _pool = new ConcurrentStack<ManagedThread>();
        }

        internal static bool AddToPool(ManagedThread t)
        {
            _pool.Push(t);
            return true;
        }

        internal static bool Broadcast(ThreadEvent tEvent)
        {
            GlobalEventQueue.Enqueue(tEvent);
            return true;
        }
    }
}
