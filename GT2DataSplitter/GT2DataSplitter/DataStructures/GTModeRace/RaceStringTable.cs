using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter
{
    using StreamExtensions;

    public static class RaceStringTable
    {
        public static List<string> Strings = new List<string>();
        public static StringTableLookup Lookup { get; set; } = new StringTableLookup();

        public static void Read(Stream file, long startPosition)
        {
            file.Position = startPosition;
            ushort stringCount = file.ReadUShort();
            for (ushort i = 0; i < stringCount; i++)
            {
                Strings.Add(ReadString(file));
            }
        }

        private static string ReadString(Stream stream)
        {
            byte stringLength = (byte)stream.ReadByte();
            byte[] stringBytes = new byte[stringLength + 1];
            stream.Read(stringBytes);
            return Encoding.Default.GetString(stringBytes).TrimEnd('\0');
        }

        public static void Write(Stream file, long indexPosition)
        {
            file.Position = file.Length;
            uint startingPosition = (uint)file.Position;

            file.WriteUShort((ushort)Strings.Count);

            foreach (string newString in Strings)
            {
                byte[] characters = Encoding.Default.GetBytes((newString + "\0").ToCharArray());
                byte length = (byte)(characters.Length - 1);
                file.WriteByte(length);
                file.Write(characters, 0, characters.Length);
            }

            uint blockSize = (uint)file.Position - startingPosition;
            file.Position = indexPosition;
            file.WriteUInt(startingPosition);
            file.WriteUInt(blockSize);
        }

        public class StringTableLookup : ITypeConverter
        {
            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                if (Strings.Contains(text))
                {
                    return (ushort)Strings.IndexOf(text);
                }
                
                Strings.Add(text);
                return (ushort)(Strings.Count - 1);
            }

            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                ushort i = Convert.ToUInt16(value);
                return Strings[i];
            }
        }
    }
}