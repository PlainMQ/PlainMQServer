using System.Text;

namespace PlainMQServer.Models
{
    public enum MessageDirection
    {
        Read,
        Write
    }

    public struct PlainMessage
    {
        public int LENGTH;
        public byte[]? BODY;

        public byte[] ToMessageBytes()
        {
            byte[] ret = new byte[LENGTH + sizeof(int)];

            BitConverter.GetBytes(LENGTH).CopyTo(ret, 0);
            BODY.CopyTo(ret, sizeof(int));

            return ret;
        }

        public PlainMessage(byte[] inBytes, int len)
        {
            Span<byte> span = new Span<byte>(inBytes);
            LENGTH = len;
            BODY = inBytes;
        }

        public PlainMessage(int len)
        {
            LENGTH = len;
            BODY = new byte[LENGTH];
        }
    }
}
