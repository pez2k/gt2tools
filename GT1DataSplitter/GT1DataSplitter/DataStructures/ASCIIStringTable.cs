using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GT1.DataSplitter
{
    using StreamExtensions;

    public static class ASCIIStringTable
    {
        private static readonly List<string> strings = new();
        private static readonly Encoding encoding = Encoding.ASCII;

        public static void Read(Stream file, long startPosition)
        {
            file.Position = startPosition;
            ushort stringCount = file.ReadUShort();
            for (ushort i = 0; i < stringCount; i++)
            {
                strings.Add(ReadString(file));
            }
        }

        private static string ReadString(Stream stream)
        {
            byte stringLength = (byte)stream.ReadByte();
            byte[] stringBytes = new byte[stringLength + 1];
            stream.Read(stringBytes);
            return encoding.GetString(stringBytes).TrimEnd('\0');
        }

        public static void Write(Stream file, long indexPosition)
        {
            file.Position = file.Length;
            uint startingPosition = (uint)file.Position;

            file.WriteUShort((ushort)strings.Count);

            foreach (string newString in strings)
            {
                byte[] characters = encoding.GetBytes((newString + "\0").ToCharArray());
                byte length = (byte)(characters.Length - 1);
                file.WriteByte(length);
                file.Write(characters, 0, characters.Length);
            }

            while (file.Length % 2 != 0)
            {
                file.WriteByte(0);
            }

            uint blockSize = (uint)file.Position - startingPosition;
            file.Position = indexPosition;
            file.WriteUInt(startingPosition);
            file.WriteUInt(blockSize);
        }

        public static ushort Add(string text)
        {
            if (strings.Contains(text))
            {
                return (ushort)strings.IndexOf(text);
            }
                
            strings.Add(text);
            return (ushort)(strings.Count - 1);
        }

        public static string Get(ushort index) => strings[index];
    }
}