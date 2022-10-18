using System;
using System.IO;
using System.Linq;
using StreamExtensions;

namespace GT2.ArcadeCarLogoTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: GT2ArcadeCarLogoTool <arc_carlogo file to extract>\r\nOR\r\nGT2ArcadeCarLogoTool <directory of TIM files to pack>");
                return;
            }

            if (File.GetAttributes(args[0]).HasFlag(FileAttributes.Directory))
            {
                Build(args[0]);
            }
            else
            {
                Extract(args[0]);
            }
        }

        private static void Extract(string filename)
        {
            using (Stream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                uint entryCount = file.ReadUInt();
                uint[] offsets = new uint[entryCount];
                for (uint i = 0; i < entryCount; i++)
                {
                    offsets[i] = file.ReadUInt();
                }

                Directory.CreateDirectory("extracted");
                for (uint i = 0; i < entryCount; i++)
                {
                    file.Position = offsets[i];
                    uint endPosition = i + 1 == entryCount ? (uint)file.Length : offsets[i + 1];
                    using (Stream output = new FileStream($"extracted\\logo{i:D2}.tim", FileMode.Create, FileAccess.Write))
                    {
                        byte[] timFile = new byte[endPosition - file.Position];
                        file.Read(timFile);
                        output.Write(timFile);
                    }
                }
            }
        }

        private static void Build(string timDirectory)
        {
            string[] files = Directory.EnumerateFiles(timDirectory, "*.tim").ToArray();
            using (Stream output = new FileStream($"arc_carlogo", FileMode.Create, FileAccess.Write))
            {
                output.WriteUInt((uint)files.Length);
                output.SetLength((files.Length + 1) * 4);

                for (int i = 0; i < files.Length; i++)
                {
                    output.Position = (i + 1) * 4;
                    output.WriteUInt((uint)output.Length);
                    output.Position = output.Length;
                    using (Stream file = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                    {
                        file.CopyTo(output);
                    }
                }

            }
        }
    }
}