using System.IO;
using System.Text;

namespace StreamExtensions
{
    public static class StreamExtensions
    {
        public static int Read(this Stream stream, byte[] buffer)
        {
            return stream.Read(buffer, 0, buffer.Length);
        }

        public static void Write(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }

        public static byte ReadSingleByte(this Stream stream) => (byte)stream.ReadByte();

        public static uint ReadUInt(this Stream stream)
        {
            byte[] rawValue = new byte[4];
            stream.Read(rawValue);
            return (uint)(rawValue[3] * 256 * 256 * 256 + rawValue[2] * 256 * 256 + rawValue[1] * 256 + rawValue[0]);
        }

        public static uint ReadUInt(this byte[] array)
        {
            return (uint)(array[3] * 256 * 256 * 256 + array[2] * 256 * 256 + array[1] * 256 + array[0]);
        }

        public static void WriteUInt(this Stream stream, uint value)
        {
            stream.Write(value.ToByteArray(), 0, 4);
        }

        public static byte[] ToByteArray(this uint value)
        {
            byte[] byteArray = new byte[4];
            byteArray[3] = (byte)((value / 256 / 256 / 256) % 256);
            byteArray[2] = (byte)((value / 256 / 256) % 256);
            byteArray[1] = (byte)((value / 256) % 256);
            byteArray[0] = (byte)(value % 256);
            return byteArray;
        }

        public static short ReadShort(this Stream stream)
        {
            int longValue = stream.ReadUShort();
            if (longValue > short.MaxValue)
            {
                longValue -= ushort.MaxValue;
            }
            return (short)longValue;
        }

        public static void WriteShort(this Stream stream, short value)
        {
            ushort fullValue = (ushort)(value < 0 ? (value + ushort.MaxValue) : value);
            stream.WriteUShort(fullValue);
        }

        public static ushort ReadUShort(this Stream stream)
        {
            byte[] rawValue = new byte[2];
            stream.Read(rawValue);
            return (ushort)(rawValue[1] * 256 + rawValue[0]);
        }

        public static void WriteUShort(this Stream stream, ushort value)
        {
            stream.Write(value.ToByteArray(), 0, 2);
        }

        public static void WriteCharacters(this Stream stream, string characters)
        {
            stream.Write(Encoding.ASCII.GetBytes(characters));
        }

        public static byte[] ToByteArray(this ushort value)
        {
            byte[] byteArray = new byte[2];
            byteArray[1] = (byte)((value / 256) % 256);
            byteArray[0] = (byte)(value % 256);
            return byteArray;
        }

        public static byte[] ToByteArray(this string value)
        {
            return Encoding.Default.GetBytes(value.ToCharArray());
        }
    }
}