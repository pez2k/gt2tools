using System;
using System.Collections.Generic;
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
            byte[] rawValue = new byte[sizeof(uint)];
            stream.Read(rawValue);
            return rawValue.ReadUInt();
        }

        public static uint ReadUInt(this byte[] array) => BitConverter.ToUInt32(array, 0);

        public static void WriteUInt(this Stream stream, uint value) => stream.Write(value.ToByteArray(), 0, sizeof(uint));

        public static byte[] ToByteArray(this uint value) => BitConverter.GetBytes(value);

        public static int ReadInt(this Stream stream)
        {
            byte[] rawValue = new byte[sizeof(int)];
            stream.Read(rawValue);
            return rawValue.ReadInt();
        }

        public static int ReadInt(this byte[] array) => BitConverter.ToInt32(array, 0);

        public static void WriteInt(this Stream stream, int value) => stream.Write(value.ToByteArray(), 0, sizeof(int));

        public static byte[] ToByteArray(this int value) => BitConverter.GetBytes(value);

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
            byte[] rawValue = new byte[sizeof(ushort)];
            stream.Read(rawValue);
            return rawValue.ReadUShort();
        }

        public static ushort ReadUShort(this byte[] array) => BitConverter.ToUInt16(array, 0);

        public static void WriteUShort(this Stream stream, ushort value)
        {
            stream.Write(value.ToByteArray(), 0, sizeof(ushort));
        }

        public static byte[] ToByteArray(this ushort value) => BitConverter.GetBytes(value);

        public static string ReadCharacters(this Stream stream)
        {
            return ReadCharacters(stream, Encoding.ASCII);
        }

        public static string ReadCharacters(this Stream stream, Encoding encoding)
        {
            byte characterByte;
            List<byte> bytes = new List<byte>();
            while (true)
            {
                characterByte = stream.ReadSingleByte();
                if (characterByte == 0)
                {
                    break;
                }
                bytes.Add(characterByte);
            }
            return encoding.GetString(bytes.ToArray());
        }

        public static void WriteCharacters(this Stream stream, string characters)
        {
            WriteCharacters(stream, characters, Encoding.ASCII);
        }

        public static void WriteCharacters(this Stream stream, string characters, Encoding encoding)
        {
            stream.Write(encoding.GetBytes(characters));
        }

        public static byte[] ToByteArray(this string value)
        {
            return Encoding.Default.GetBytes(value.ToCharArray());
        }

        public static ulong ReadULong(this Stream stream)
        {
            byte[] value = new byte[sizeof(ulong)];
            stream.Read(value);
            return ReadULong(value);
        }

        public static ulong ReadULong(this byte[] array) => BitConverter.ToUInt64(array, 0);

        public static void WriteULong(this Stream stream, ulong value)
        {
            stream.Write(value.ToByteArray(), 0, sizeof(ulong));
        }

        public static byte[] ToByteArray(this ulong value) => BitConverter.GetBytes(value);

        public static void MoveToNextMultipleOf(this Stream stream, int value)
        {
            while (stream.Position % value != 0)
            {
                stream.Position++;
            }
        }

        public static bool ReadByteAsBool(this Stream stream) => stream.ReadSingleByte() == 1;

        public static void WriteBoolAsByte(this Stream stream, bool value) => stream.WriteByte((byte)(value ? 1 : 0));

        public static sbyte ReadSByte(this Stream stream) => (sbyte)stream.ReadByte();

        public static void WriteSByte(this Stream stream, sbyte value) => stream.WriteByte((byte)value);

        // Original ReadShort / WriteShort implementations above give a value of 0 for 0xFFFF... whoops
        // Retain the old behaviour until tools that rely on it can be audited and fixed
        public static short ReadShortFixed(this Stream stream)
        {
            byte[] rawValue = new byte[sizeof(short)];
            stream.Read(rawValue);
            return rawValue.ReadShortFixed();
        }

        public static short ReadShortFixed(this byte[] array) => BitConverter.ToInt16(array, 0);

        public static void WriteShortFixed(this Stream stream, short value)
        {
            stream.Write(value.ToByteArray(), 0, sizeof(short));
        }

        public static byte[] ToByteArray(this short value) => BitConverter.GetBytes(value);
    }
}