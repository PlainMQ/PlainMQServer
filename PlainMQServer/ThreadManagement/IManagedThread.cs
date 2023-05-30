using PlainMQServer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainMQServer.ThreadManagement
{
    public interface IManagedThread
    {
        public void ThreadAction();

        public ManagedThreadStatus Status { get; internal set; }

        public ParameterizedThreadStart? Action { get; set; }

        public ThreadClass InvokeClass { get; set; }

        public int ID { get; set; }
        public string? Name { get; set; }
    }
}
