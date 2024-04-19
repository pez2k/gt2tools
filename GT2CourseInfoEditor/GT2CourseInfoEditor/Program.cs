using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;

namespace GT2.CourseInfoEditor
{
    using StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage:\r\nGT2CourseInfoEditor <crsinfo>\r\nOR\r\nGT2CourseInfoEditor <CSV file>");
                return;
            }

            if (Path.GetExtension(args[0]) == ".csv")
            {
                Import(args[0]);
            }
            else
            {
                Export(args[0]);
            }

        }

        private static void Export(string filename)
        {
            List<Course> courses = new();
            using (FileStream file = new(filename, FileMode.Open, FileAccess.Read))
            {
                if (file.ReadCharacters() != "CRS") // CRS\0
                {
                    Console.WriteLine("Error: Not a .crsinfo file.");
                    return;
                }
                if (file.ReadUShort() != 2)
                {
                    Console.WriteLine("Error: Unknown .crsinfo version.");
                    return;
                }
                ushort courseCount = file.ReadUShort();

                file.Position += courseCount * 8 * 3;

                Dictionary<uint, string> displayNames = new();

                while (file.Position < file.Length)
                {
                    uint start = (uint)file.Position;
                    string courseName = file.ReadCharacters(Encoding.UTF8);
                    displayNames.Add(start, courseName);
                }

                file.Position = 8;

                for (int i = 0; i < courseCount; i++)
                {
                    long blockStart = file.Position;

                    Course course = new();

                    uint stringAddress = file.ReadUInt();
                    if (!displayNames.TryGetValue(stringAddress, out string name))
                    {
                        throw new Exception("Unable to find referenced string.");
                    }

                    course.DisplayName = name;
                    course.Filename = file.ReadUInt();

                    byte flags = file.ReadSingleByte();
                    course.IsNight = IsBitSet(flags, 0);
                    course.IsEvening = IsBitSet(flags, 1);
                    course.IsDirt = IsBitSet(flags, 2);
                    course.Is2Player = IsBitSet(flags, 3);
                    course.IsReverse = IsBitSet(flags, 4);
                    course.IsPointToPoint = IsBitSet(flags, 5);
                    course.Flag7 = IsBitSet(flags, 6);

                    course.Padding = file.ReadSingleByte();
                    course.Skybox = file.ReadUShort();
                    course.LightingArea1Colour = ToRGBHex(file.ReadUShort());
                    course.LightingArea1ColourMultiplier = file.ReadUShort();
                    course.LightingArea2Colour = ToRGBHex(file.ReadUShort());
                    course.LightingArea2ColourMultiplier = file.ReadUShort();
                    course.LightingArea3Colour = ToRGBHex(file.ReadUShort());
                    course.LightingArea3ColourMultiplier = file.ReadUShort();

                    if (file.Position != blockStart + (8 * 3))
                    {
                        throw new Exception("Block size incorrect.");
                    }

                    courses.Add(course);
                }
            }
                
            using (FileStream outFile = new("Courses.csv", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter output = new(outFile, Encoding.UTF8))
                {
                    using (CsvWriter csv = new(output))
                    {
                        csv.Configuration.RegisterClassMap<CourseCSVMap>();
                        csv.Configuration.ShouldQuote = (field, context) => true;
                        csv.WriteHeader<Course>();
                        csv.NextRecord();

                        foreach (Course course in courses)
                        {
                            csv.WriteRecord(course);
                            csv.NextRecord();
                        }
                    }
                }
            }
        }

        private static bool IsBitSet(byte value, int position) => (value & (1 << position)) != 0;

        private static string ToRGBHex(ushort colour)
        {
            int R = colour & 0x1F;
            int G = (colour >> 5) & 0x1F;
            int B = (colour >> 10) & 0x1F;
            return $"#{R * 8:X2}{G * 8:X2}{B * 8:X2}";
        }

        private static void Import(string filename)
        {
            List<Course> courses = new();
            using (FileStream csvFile = new(filename, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader csvStream = new(csvFile, Encoding.UTF8))
                {
                    using (CsvReader csv = new(csvStream))
                    {
                        csv.Configuration.RegisterClassMap<CourseCSVMap>();
                        csv.Read();
                        csv.ReadHeader();

                        while (csv.Read())
                        {
                            courses.Add(csv.GetRecord<Course>());
                        }
                    }
                }
            }

            using (FileStream file = new(".crsinfo", FileMode.Create, FileAccess.Write))
            {
                file.WriteCharacters("CRS\0");
                file.WriteUShort(2);
                file.WriteUShort((ushort)courses.Count);
                file.Position += courses.Count * 8 * 3;

                string[] displayNames = courses.Select(course => course.DisplayName).Distinct().OrderBy(name => name, StringComparer.Ordinal).ToArray();
                Dictionary<string, uint> stringAddressMap = new(displayNames.Length);
                foreach (string name in displayNames)
                {
                    stringAddressMap[name] = (uint)file.Position;
                    file.WriteCharacters(name, Encoding.UTF8);
                    file.WriteByte(0); // Terminator
                }

                file.Position = 8;

                foreach (Course course in courses)
                {
                    if (!stringAddressMap.TryGetValue(course.DisplayName, out uint address))
                    {
                        throw new Exception("String address not mapped.");
                    }

                    file.WriteUInt(address);
                    file.WriteUInt(course.Filename);

                    byte flags = 0;
                    flags = SetBit(flags, 0, course.IsNight);
                    flags = SetBit(flags, 1, course.IsEvening);
                    flags = SetBit(flags, 2, course.IsDirt);
                    flags = SetBit(flags, 3, course.Is2Player);
                    flags = SetBit(flags, 4, course.IsReverse);
                    flags = SetBit(flags, 5, course.IsPointToPoint);
                    flags = SetBit(flags, 6, course.Flag7);
                    file.WriteByte(flags);
                    file.WriteByte(0); // Padding

                    file.WriteUShort(course.Skybox);
                    file.WriteUShort(FromRGBHex(course.LightingArea1Colour));
                    file.WriteUShort(course.LightingArea1ColourMultiplier);
                    file.WriteUShort(FromRGBHex(course.LightingArea2Colour));
                    file.WriteUShort(course.LightingArea2ColourMultiplier);
                    file.WriteUShort(FromRGBHex(course.LightingArea3Colour));
                    file.WriteUShort(course.LightingArea3ColourMultiplier);
                }
            }
        }

        private static byte SetBit(byte value, int position, bool set)
        {
            if (set)
            {
                value |= (byte)(1 << position);
            }
            return value;
        }

        private static ushort FromRGBHex(string rgb) =>
            rgb.Length != 7 ||
            !int.TryParse(rgb.AsSpan(1, 2), NumberStyles.HexNumber, null, out int r) ||
            !int.TryParse(rgb.AsSpan(3, 2), NumberStyles.HexNumber, null, out int g) ||
            !int.TryParse(rgb.AsSpan(5, 2), NumberStyles.HexNumber, null, out int b)
                ? throw new Exception($"Unable to parse hex colour '{rgb}'.")
                : (ushort)((((b / 8) & 0x1F) << 10) + (((g / 8) & 0x1F) << 5) + ((r / 8) & 0x1F));
    }
}