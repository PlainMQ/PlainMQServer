using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainMQServer.ThreadManagement
{
    public interface IManagedThread
    {
        public void ThreadAction(object? o);
    }
}
