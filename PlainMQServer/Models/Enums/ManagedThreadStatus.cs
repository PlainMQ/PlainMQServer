using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainMQServer.Models.Enums
{
    public enum ManagedThreadStatus
    {
        NEW,
        IDLE,
        RUNNING,
        ERROR
    }
}
