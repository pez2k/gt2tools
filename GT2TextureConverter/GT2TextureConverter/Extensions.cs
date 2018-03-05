using System.IO;

namespace GT2TextureConverter
{
    public static class Extensions
    {
        public static uint ReadUInt(this FileStream fileToRead)
        {
            byte[] rawValue = new byte[4];
            fileToRead.Read(rawValue, 0, 4);
            return (uint)(rawValue[3] * 256 * 256 * 256 + rawValue[2] * 256 * 256 + rawValue[1] * 256 + rawValue[0]);
        }

        public static uint ReadUInt(this byte[] arrayToRead)
        {
            return (uint)(arrayToRead[3] * 256 * 256 * 256 + arrayToRead[2] * 256 * 256 + arrayToRead[1] * 256 + arrayToRead[0]);
        }

        public static void WriteUInt(this FileStream fileToWrite, uint valueToWrite)
        {
            fileToWrite.Write(valueToWrite.ToByteArray(), 0, 4);
        }

        public static byte[] ToByteArray(this uint valueToConvert)
        {
            byte[] byteArray = new byte[4];
            byteArray[3] = (byte)((valueToConvert / 256 / 256 / 256) % 256);
            byteArray[2] = (byte)((valueToConvert / 256 / 256) % 256);
            byteArray[1] = (byte)((valueToConvert / 256) % 256);
            byteArray[0] = (byte)(valueToConvert % 256);
            return byteArray;
        }

        public static ushort ReadUShort(this FileStream fileToRead)
        {
            byte[] rawValue = new byte[2];
            fileToRead.Read(rawValue, 0, 2);
            return (ushort)(rawValue[1] * 256 + rawValue[0]);
        }
    }
}
