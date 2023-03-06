using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using StreamExtensions;

namespace GT1.SystemEnvEditor
{
    internal class SystemEnv
    {
        private readonly static string[] bgNames = new string[] { "Test", "tl_sky2", "tl_sky2g", "dawn", "dawn2", "dawn3", "c1_bg", "noon", "au", "m_sky_X" };

        private ushort unknownCount1;
        private ushort courseCount;
        private ushort carCount;
        private ushort carcadeCount;
        private ushort musicCount;
        private ushort unknownCount2;
        private string[] courseCodes = Array.Empty<string>();
        private List<byte[]> courseModes = new();
        private byte[] courseBGs = Array.Empty<byte>();
        private string[] courseNames = Array.Empty<string>();
        private ushort[] unknownData1 = Array.Empty<ushort>();
        private string[] carList = Array.Empty<string>();
        private List<byte>[] carColours = Array.Empty<List<byte>>();
        private string[] carNames = Array.Empty<string>();
        private ushort[] arcadeCarIDs = Array.Empty<ushort>();
        private string[] musicCodes = Array.Empty<string>();
        private uint[] musicData = Array.Empty<uint>();
        private string[] musicNames = Array.Empty<string>();
        private uint[] unknownData2 = Array.Empty<uint>();
        private List<string> otherSettings = new();

        public void ReadFromBinary(string filename)
        {
            using (FileStream file = new(filename, FileMode.Open, FileAccess.Read))
            {
                if (file.ReadCharacters() != "@(#)GTENV")
                {
                    throw new Exception("Not a GTENV file.");
                }

                unknownCount1 = file.ReadUShort();
                courseCount = file.ReadUShort();
                carCount = file.ReadUShort();
                carcadeCount = file.ReadUShort();
                musicCount = file.ReadUShort();
                unknownCount2 = file.ReadUShort();
                file.Position = 0x48;

                courseCodes = ReadStrings(file, courseCount);

                courseModes = new(courseCount);
                courseBGs = new byte[courseCount];
                for (int i = 0; i < courseCount; i++)
                {
                    var courseMode = new byte[16];
                    file.Read(courseMode);
                    byte firstByte = courseMode[0];
                    courseBGs[i] = (byte)(firstByte & 0x7F);
                    courseMode[0] = (byte)(firstByte >> 7);
                    courseModes.Add(courseMode);
                }

                courseNames = ReadStrings(file, courseCount);

                unknownData1 = new ushort[16];
                for (int i = 0; i < 16; i++)
                {
                    unknownData1[i] = file.ReadUShort();
                }

                carList = new string[carCount];
                for (int i = 0; i < carCount; i++)
                {
                    carList[i] = file.ReadCharacters();
                }

                carColours = new List<byte>[carCount];
                for (int i = 0; i < carCount; i++)
                {
                    byte colourCount = file.ReadSingleByte();
                    carColours[i] = new List<byte>(colourCount);
                    for (int j = 0; j < colourCount; j++)
                    {
                        file.ReadSingleByte(); // colour number, sequential, ignorable?
                        carColours[i].Add(file.ReadSingleByte());
                    }
                }

                carNames = ReadStrings(file, carCount);
                for (int i = 0; i < carCount; i++)
                {
                    carNames[i] = carNames[i].Replace($"{(char)0x7F}", "[R]");
                }

                arcadeCarIDs = new ushort[carcadeCount];
                for (int i = 0; i < carcadeCount; i++)
                {
                    arcadeCarIDs[i] = file.ReadUShort();
                }

                musicCodes = ReadStrings(file, musicCount);
                musicData = new uint[musicCount * 2];
                for (int i = 0; i < musicCount * 2; i++)
                {
                    musicData[i] = file.ReadUInt();
                }
                musicNames = ReadStrings(file, musicCount);

                unknownData2 = new uint[22];
                for (int i = 0; i < 22; i++)
                {
                    unknownData2[i] = file.ReadUInt();
                }

                while (file.Position != file.Length)
                {
                    otherSettings.Add(file.ReadCharacters());
                }
                otherSettings = otherSettings.Where(setting => !string.IsNullOrWhiteSpace(setting)).ToList();
            }
        }

        private static string[] ReadStrings(Stream file, ushort count)
        {
            file.MoveToNextMultipleOf(4);
            var strings = new string[count];
            for (int i = 0; i < count; i++)
            {
                byte stringLength = file.ReadSingleByte();
                var characters = new byte[stringLength - 1];
                file.Read(characters);
                strings[i] = Encoding.ASCII.GetString(characters);
                file.Position++;
            }
            file.MoveToNextMultipleOf(4);
            return strings;
        }

        public void WriteToPlaintext(string filename)
        {
            using (FileStream output = new(filename, FileMode.Create, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(output))
                {
                    writer.WriteLine($"car={carCount}");
                    for (int i = 0; i < carCount; i++)
                    {
                        writer.WriteLine($"car.name.{i}={carNames[i]}");
                    }
                    writer.WriteLine($"car.list={string.Join(',', carList)}");
                    writer.WriteLine($"car.color={string.Join(',', carColours.Select(colourIDs => $"{colourIDs.Count},{string.Join("", Enumerable.Range(0, colourIDs.Count).Select(i => $"{(char)colourIDs[i]}{i}"))}"))}");
                    writer.WriteLine($"arcade.car={carcadeCount}");
                    writer.WriteLine($"arcade.car.ids={string.Join(',', arcadeCarIDs.Select(id => $"{id}"))}");
                    writer.WriteLine($"course={courseCount}");
                    for (int i = 0; i < courseCount; i++)
                    {
                        writer.WriteLine($"course.mode.{i}={string.Join(',', courseModes[i].Select(mode => $"{mode}"))}");
                    }
                    for (int i = 0; i < courseCount; i++)
                    {
                        writer.WriteLine($"course.name.{i}={courseNames[i]}");
                    }
                    for (int i = 0; i < courseCount; i++)
                    {
                        writer.WriteLine($"course.bg.{i}=BG {SystemEnv.bgNames[courseBGs[i]]}");
                    }
                    writer.WriteLine($"course.code={string.Join(',', courseCodes)}");
                    writer.WriteLine($"music.code={musicCount},{string.Join(',', Enumerable.Range(0, musicCount).Select(i => $"{musicCodes[i]},{musicData[i * 2]:X},{musicData[(i * 2) + 1]:X}"))}");
                    for (int i = 0; i < musicCount; i++)
                    {
                        writer.WriteLine($"music.name.{i}={musicNames[i]}");
                    }
                    foreach (string setting in otherSettings)
                    {
                        writer.WriteLine(setting);
                    }
                    writer.WriteLine($"unknown.count.1={unknownCount1}");
                    writer.WriteLine($"unknown.count.2={unknownCount2}");
                    writer.WriteLine($"unknown.1={string.Join(',', unknownData1.Select(value => $"{value:X4}"))}");
                    writer.WriteLine($"unknown.2={string.Join(',', unknownData2.Select(value => $"{value:X8}"))}");
                }
            }
        }

        public void WriteToBinary(string filename)
        {
            using (FileStream file = new(filename, FileMode.Create, FileAccess.Write))
            {
                file.WriteCharacters("@(#)GTENV");
                file.WriteByte(0);

                file.WriteUShort(unknownCount1);
                file.WriteUShort(courseCount);
                file.WriteUShort(carCount);
                file.WriteUShort(carcadeCount);
                file.WriteUShort(musicCount);
                file.WriteUShort(unknownCount2);
                file.Position = 0x48;

                WriteStrings(file, courseCodes);

                for (int i = 0; i < courseCount; i++)
                {
                    byte[] courseMode = courseModes[i];
                    courseMode[0] = (byte)(courseBGs[i] | (courseMode[0] << 7));
                    file.Write(courseMode);
                }

                WriteStrings(file, courseNames);

                foreach (ushort unknown in unknownData1)
                {
                    file.WriteUShort(unknown);
                }

                foreach (string car in carList)
                {
                    file.WriteCharacters(car);
                    file.WriteByte(0);
                }

                foreach (List<byte> carColourSet in carColours)
                {
                    byte colourCount = (byte)carColourSet.Count;
                    file.WriteByte(colourCount);
                    for (int i = 0; i < colourCount; i++)
                    {
                        file.WriteByte((byte)i);
                        file.WriteByte(carColourSet[i]);
                    }
                }

                for (int i = 0; i < carCount; i++)
                {
                    carNames[i] = carNames[i].Replace("[R]", $"{(char)0x7F}");
                }
                WriteStrings(file, carNames);

                foreach (ushort arcadeCarID in arcadeCarIDs)
                {
                    file.WriteUShort(arcadeCarID);
                }

                WriteStrings(file, musicCodes);
                foreach (uint musicDataItem in musicData)
                {
                    file.WriteUInt(musicDataItem);
                }
                WriteStrings(file, musicNames);

                foreach (uint unknown in unknownData2)
                {
                    file.WriteUInt(unknown);
                }

                foreach (string setting in otherSettings)
                {
                    file.WriteCharacters(setting);
                    file.WriteByte(0);
                }

                file.MoveToNextMultipleOf(4);
                file.SetLength(file.Position);
            }
        }

        private static void WriteStrings(Stream file, string[] strings)
        {
            file.MoveToNextMultipleOf(4);
            foreach (string s in strings)
            {
                byte stringLength = (byte)(s.Length + 1);
                file.WriteByte(stringLength);
                file.Write(Encoding.ASCII.GetBytes(s));
                file.WriteByte(0);
            }
            file.MoveToNextMultipleOf(4);
        }

        public void WriteToEditable(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            CsvConfiguration csvConfig = new(CultureInfo.CurrentCulture) { ShouldQuote = (args) => true };

            using (FileStream file = new(Path.Combine(directory, "Courses.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    writer.WriteLine("\"Code\",\"Name\",\"Background\",\"IsNight\",\"Unknown1\",\"Unknown2\",\"Unknown3\",\"Unknown4\",\"Unknown5\",\"Unknown6\",\"Unknown7\",\"Unknown8\",\"Unknown9\",\"Unknown10\",\"Unknown11\",\"Unknown12\",\"Unknown13\",\"Unknown14\",\"Unknown15\"");
                    for (int i = 0; i < courseCount; i++)
                    {
                        writer.WriteLine($"\"{courseCodes[i]}\",\"{courseNames[i]}\",\"{courseBGs[i]}\",\"{string.Join("\",\"", courseModes[i].Select(value => $"{value}"))}\"");
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "Cars.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("Code");
                        csv.WriteField("Name");
                        csv.NextRecord();

                        for (int i = 0; i < carCount; i++)
                        {
                            csv.WriteField(carList[i]);
                            csv.WriteField(carNames[i]);
                            csv.NextRecord();
                        }
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "CarColours.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("Code");
                        csv.WriteField("ColourID");
                        csv.NextRecord();

                        for (int i = 0; i < carCount; i++)
                        {
                            foreach (byte colourID in carColours[i])
                            {
                                csv.WriteField(carList[i]);
                                csv.WriteField($"{colourID:X2}");
                                csv.NextRecord();
                            }
                        }
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "ArcadeCars.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("ID");
                        csv.NextRecord();

                        foreach (ushort carID in arcadeCarIDs)
                        {
                            csv.WriteField($"{carID:X4}");
                            csv.NextRecord();
                            writer.WriteLine();
                        }
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "Music.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("Code");
                        csv.WriteField("Name");
                        csv.WriteField("Unknown1");
                        csv.WriteField("LengthMaybe");
                        csv.NextRecord();

                        for (int i = 0; i < musicCount; i++)
                        {
                            csv.WriteField(musicCodes[i]);
                            csv.WriteField(musicNames[i]);
                            csv.WriteField($"{musicData[i * 2]:X4}");
                            csv.WriteField($"{musicData[(i * 2) + 1]:X4}");
                            csv.NextRecord();
                        }
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "UnknownData1.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("Unknown1");
                        csv.NextRecord();

                        foreach (ushort unknown in unknownData1)
                        {
                            csv.WriteField($"{unknown:X4}");
                            csv.NextRecord();
                        }
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "UnknownData2.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("Unknown1");
                        csv.NextRecord();

                        foreach (uint unknown in unknownData2)
                        {
                            csv.WriteField($"{unknown:X8}");
                            csv.NextRecord();
                        }
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "UnknownValues.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("UnknownCount1");
                        csv.WriteField("UnknownCount2");
                        csv.NextRecord();

                        csv.WriteField($"{unknownCount1:X4}");
                        csv.WriteField($"{unknownCount2:X4}");
                        csv.NextRecord();
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "Settings.txt"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    foreach (string setting in otherSettings)
                    {
                        writer.WriteLine(setting);
                    }
                }
            }
        }
    }
}