using PlainMQServer.Models.Enums;

namespace PlainMQServer.Models
{
    internal class ThreadEvent
    {
        internal ThreadClass Class { get; set; }
        internal object EventPayload { get; set; }
    }
}
