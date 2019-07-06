using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StreamExtensions;

namespace GT3.GameConfigEditor
{
    class Program
    {
        private enum ListType : byte {
            Unknown00,
            Unknown01,
            Unknown02,
            DemoDemos,
            Events,
            Unknown05,
            Courses,
            DemoCarClasses,
            CourseSelection,
            DemoCarUnknown,
            GTAutoPrices,
            Unknown0B,
            Demos,
            Replays,
            CarClasses,
            Prizes
        }

        static void Main(string[] args)
        {
            string filename = args[0];
            if (new DirectoryInfo(filename).Attributes.HasFlag(FileAttributes.Directory))
            {
                Build(filename);
            }
            else
            {
                Dump(filename);
            }
        }

        static void Dump(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                string directory = Path.GetFileNameWithoutExtension(filename);
                Directory.CreateDirectory(directory);
                uint listCount = file.ReadUInt();
                uint startOfIndexes = file.ReadUInt();

                for (int i = 0; i < listCount; i++)
                {
                    file.Position = startOfIndexes + (i * 8);

                    ListType listType = (ListType)file.ReadUInt();
                    uint structurePos = file.ReadUInt();

                    uint nextStructurePos;
                    if (i + 1 < listCount)
                    {
                        uint nextListType = file.ReadUInt();
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

                    switch (listType)
                    {
                        case ListType.Demos:
                            using (var memoryStream = new MemoryStream(buffer))
                            {
                                Demos.Dump(memoryStream, directory, i);
                            }
                            break;

                        case ListType.Events:
                            using (var memoryStream = new MemoryStream(buffer))
                            {
                                Events.Dump(memoryStream, directory, i);
                            }
                            break;

                        case ListType.Courses:
                            using (var memoryStream = new MemoryStream(buffer))
                            {
                                Courses.Dump(memoryStream, directory, i);
                            }
                            break;

                        case ListType.Prizes:
                            using (var memoryStream = new MemoryStream(buffer))
                            {
                                Prizes.Dump(memoryStream, directory, i);
                            }
                            break;

                        case ListType.GTAutoPrices:
                            using (var memoryStream = new MemoryStream(buffer))
                            {
                                GTAutoPrices.Dump(memoryStream, directory, i);
                            }
                            break;

                        case ListType.Replays:
                            using (var memoryStream = new MemoryStream(buffer))
                            {
                                Replays.Dump(memoryStream, directory, i);
                            }
                            break;

                        case ListType.CourseSelection:
                            using (var memoryStream = new MemoryStream(buffer))
                            {
                                CourseSelection.Dump(memoryStream, directory, i);
                            }
                            break;

                        default:
                            using (var output = new FileStream(Path.Combine(directory, $"{i}_{listType}.dat"), FileMode.Create, FileAccess.Write))
                            {
                                output.Write(buffer);
                            }
                            break;
                    }
                }
            }
        }

        static void Build(string directory)
        {
            using (var output = new FileStream($"{directory}.gcf", FileMode.Create, FileAccess.Write))
            {
                List<string> files = Directory.EnumerateFiles(directory).ToList();

                output.WriteUInt((uint)files.Count);
                output.WriteUInt(8);

                long dataStart = output.Position + (files.Count * 8);
                long headerPos = 0;

                foreach (string filename in files)
                {
                    string listName = Path.GetFileNameWithoutExtension(filename).Substring(2);
                    ListType listType = (ListType)Enum.Parse(typeof(ListType), listName);
                    output.WriteUInt((uint)listType);
                    output.WriteUInt((uint)dataStart);
                    headerPos = output.Position;
                    output.Position = dataStart;

                    using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        file.CopyTo(output);
                    }

                    dataStart = output.Position;
                    output.Position = headerPos;
                }
            }
        }
    }
}
