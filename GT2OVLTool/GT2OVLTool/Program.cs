using System;
using System.IO;
using System.Linq;
using StreamExtensions;

namespace GT2.OVLTool
{
    class Program
    {
        private static readonly string[] overlayNames = new[] { "overlay0", "global", "arcade", "overlay3", "gt", "overlay5" };

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: GT2OVLTool <OVL to extract / directory to pack>");
            }
            else if (Directory.Exists(args[0]))
            {
                Rebuild(args[0]);
            }
            else
            {
                Extract(args[0]);
            }
        }

        private static void Extract(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                uint headerSize = file.ReadUInt();
                var toc = new (uint offset, uint size)[headerSize / 8];
                file.Position = 0;
                for (int i = 0; i < headerSize / 8; i++)
                {
                    toc[i] = (file.ReadUInt(), file.ReadUInt());
                }

                if (toc.Last().offset + toc.Last().size != file.Length)
                {
                    throw new Exception("Invalid file size.");
                }

                Directory.CreateDirectory("extracted");
                for (int i = 0; i < headerSize / 8; i++)
                {
                    file.Position = toc[i].offset;
                    using (var output = new FileStream(Path.Combine("extracted", $"{GetOverlayName(i)}.gz"), FileMode.Create, FileAccess.Write))
                    {
                        var buffer = new byte[toc[i].size];
                        file.Read(buffer);
                        output.Write(buffer);
                    }
                }
            }
        }

        private static void Rebuild(string directory)
        {
            using (var output = new FileStream("GT2.OVL", FileMode.Create, FileAccess.Write))
            {
                int headerSize = overlayNames.Length * 8;
                output.Position = headerSize;
                for (int i = 0; i < overlayNames.Length; i++)
                {
                    uint offset = (uint)output.Position;
                    using (var input = new FileStream(Path.Combine(directory, $"{overlayNames[i]}.gz"), FileMode.Open, FileAccess.Read))
                    {
                        uint size = (uint)input.Length;
                        var buffer = new byte[size];
                        input.Read(buffer);
                        output.Write(buffer);

                        output.Position = i * 8;
                        output.WriteUInt(offset);
                        output.WriteUInt(size);

                        output.Position = offset + size;
                        while (output.Position % 4 > 0) {
                            output.WriteByte(0);
                        }
                    }
                }
            }
        }

        private static string GetOverlayName(int overlayNumber) =>
            overlayNumber < overlayNames.Length ? overlayNames[overlayNumber] : $"overlay{overlayNumber}";
    }
}
