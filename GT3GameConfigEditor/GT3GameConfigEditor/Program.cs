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

                        case ListType.CarClasses:
                            using (var memoryStream = new MemoryStream(buffer))
                            {
                                CarClasses.Dump(memoryStream, directory, i);
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
            using (var output = new FileStream($"{directory}_new.gcf", FileMode.Create, FileAccess.Write))
            {
                List<string> files = Directory.EnumerateFiles(directory).ToList();
                List<string> fileNumbers = files.Select(file => Path.GetFileNameWithoutExtension(file).Substring(0, 2)).Distinct().ToList();

                output.WriteUInt((uint)fileNumbers.Count);
                output.WriteUInt(8);

                long dataStart = output.Position + (fileNumbers.Count * 8);
                long headerPos = 0;

                foreach (string fileNumber in fileNumbers)
                {
                    List<string> filenames = files.Where(file => Path.GetFileNameWithoutExtension(file).StartsWith(fileNumber)).ToList();

                    string listName = Path.GetFileNameWithoutExtension(filenames[0]);
                    int secondUnderscore = listName.IndexOf('_', 2);
                    secondUnderscore = secondUnderscore < 1 ? listName.Length : secondUnderscore;
                    if (!Enum.TryParse(listName.Substring(2, secondUnderscore - 2), out ListType listType)) {
                        continue;
                    }

                    output.WriteUInt((uint)listType);
                    output.WriteUInt((uint)dataStart);
                    headerPos = output.Position;
                    output.Position = dataStart;

                    switch (listType)
                    {
                        case ListType.Demos:
                            Demos.Import(output, filenames[0]);
                            break;
                        case ListType.Events:
                            Events.Import(output, filenames[0]);
                            break;
                        case ListType.Courses:
                            Courses.Import(output, filenames[0]);
                            break;
                        case ListType.GTAutoPrices:
                            GTAutoPrices.Import(output, filenames[0]);
                            break;
                        case ListType.Replays:
                            Replays.Import(output, filenames[0]);
                            break;
                        case ListType.Prizes:
                            Prizes.Import(output, filenames[0]);
                            break;
                        case ListType.CarClasses:
                            CarClasses.Import(output, filenames);
                            break;
                        case ListType.CourseSelection:
                            CourseSelection.Import(output, filenames);
                            break;
                        default:
                            using (var file = new FileStream(filenames[0], FileMode.Open, FileAccess.Read))
                            {
                                file.CopyTo(output);
                            }
                            break;
                    }

                    dataStart = output.Position;
                    output.Position = headerPos;
                }
            }
        }
    }
}
