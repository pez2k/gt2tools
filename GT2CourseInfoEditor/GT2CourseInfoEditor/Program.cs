using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using CsvHelper;

namespace GT2.CourseInfoEditor
{
    using StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            using (var file = new FileStream(".crsinfo", FileMode.Open, FileAccess.Read))
            {
                file.ReadUInt(); // CRS\0
                file.ReadUShort(); // 0x0002
                ushort courseCount = file.ReadUShort();
                using (var outFile = new FileStream($"Courses.csv", FileMode.Create, FileAccess.Write))
                {
                    using (TextWriter output = new StreamWriter(outFile, Encoding.UTF8))
                    {
                        using (CsvWriter csv = new CsvWriter(output))
                        {
                            csv.Configuration.RegisterClassMap<CourseCSVMap>();
                            csv.Configuration.ShouldQuote = (field, context) => true;
                            csv.WriteHeader<Course>();
                            csv.NextRecord();

                            for (int i = 0; i < courseCount; i++)
                            {
                                byte[] buffer = new byte[8 * 3];
                                file.Read(buffer);

                                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                                Course course = (Course)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Course));
                                handle.Free();

                                csv.WriteRecord(course);
                                csv.NextRecord();
                            }
                        }
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
