using System;
using System.IO;
using System.Linq;
using System.Text;

namespace GT3.StringEditor
{
    using GT2.StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Wrong number of arguments.");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Input file not found.");
                return;
            }

            using (var file = new FileStream(args[0], FileMode.Open, FileAccess.Read))
            {
                byte[] magic = new byte[4];
                file.Read(magic);
                if (!magic.SequenceEqual(new byte[] { (byte)'S', (byte)'T', (byte)'D', (byte)'B', }))
                {
                    Console.WriteLine("Not a STDB file.");
                    return;
                }

                uint stringCount = file.ReadUInt();
                uint stringType = file.ReadUInt();
                bool unicode = false;
                if (stringType == 0xFFFF) {
                    unicode = true;
                }
                else if (stringType != 0x0001)
                {
                    Console.WriteLine("Unknown string type.");
                    return;
                }

                if (file.Length != file.ReadUInt())
                {
                    Console.WriteLine("File length incorrect.");
                }

                using (var textFile = new StreamWriter($"{Path.GetFileNameWithoutExtension(args[0])}.txt"))
                {
                    for (int i = 0; i < stringCount; i++)
                    {
                        uint stringPosition = file.ReadUInt();
                        long storedPosition = file.Position;
                        file.Position = stringPosition;
                        ushort stringLength = file.ReadUShort();
                        byte[] stringBytes = new byte[stringLength];
                        file.Read(stringBytes);
                        string output = (unicode ? Encoding.UTF8 : Encoding.Default).GetString(stringBytes);
                        Console.WriteLine(output);
                        textFile.WriteLine(output);
                        file.Position = storedPosition;
                    }
                }
            }
        }
    }
}
