using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT2.CourseInfoEditor
{
    using StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            using (var file = new FileStream(".crsinfo", FileMode.Open, FileAccess.Read))
            {
                Directory.CreateDirectory("Courses");
                Directory.CreateDirectory("Names");

                file.ReadUInt(); // CRS\0
                file.ReadUShort(); // 0x0002
                ushort courseCount = file.ReadUShort();

                for (int i = 0; i < courseCount; i++)
                {
                    byte[] buffer = new byte[8 * 3];
                    file.Read(buffer);

                    using (var outFile = new FileStream($"Courses\\{i:D3}.dat", FileMode.Create, FileAccess.Write))
                    {
                        outFile.Write(buffer);
                    }
                }

                while (file.Position < file.Length)
                {
                    var bytes = new List<byte>();
                    byte newByte;
                    do
                    {
                        newByte = (byte)file.ReadByte();
                        bytes.Add(newByte);
                    }
                    while (newByte != 0);

                    string courseName = Encoding.UTF8.GetString(bytes.ToArray());
                }
            }
        }
    }
}
