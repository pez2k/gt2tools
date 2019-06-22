using StreamExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT3.GameConfigEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var file = new FileStream("product_eu.gcf", FileMode.Open, FileAccess.Read))
            {
                Directory.CreateDirectory("output");
                uint unknown = file.ReadUInt();
                uint unknown2 = file.ReadUInt();
                uint unknown3 = file.ReadUInt();
                uint headerSize = file.ReadUInt();

                while (file.Position < headerSize)
                {
                    uint unknown4 = file.ReadUInt();
                    uint structurePos = file.ReadUInt();
                    long currentPos = file.Position;

                    uint nextStructurePos;
                    if (file.Position < headerSize)
                    {
                        uint nextUnknown4 = file.ReadUInt();
                        nextStructurePos = file.ReadUInt();
                    }
                    else
                    {
                        nextStructurePos = (uint)file.Length;
                    }
                    
                    uint structureSize = nextStructurePos - structurePos;

                    file.Position = structurePos;
                    byte[] buffer = new byte[structureSize];
                    file.Read(buffer);

                    using (var output = new FileStream($"output\\Pos_0x{structurePos:X4}_Unk_0x{unknown4:X4}.dat", FileMode.Create, FileAccess.Write))
                    {
                        output.Write(buffer);
                    }
                    file.Position = currentPos;
                }
            }
        }
    }
}
