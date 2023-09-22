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

        private ushort unknownCount;
        private ushort courseCount;
        private ushort carCount;
        private ushort carcadeCount;
        private ushort musicCount;
        private ushort streamCount;
        private string[] courseCodes = Array.Empty<string>();
        private List<byte[]> courseModes = new();
        private byte[] courseBGs = Array.Empty<byte>();
        private string[] courseNames = Array.Empty<string>();
        private ushort[] unknownData = Array.Empty<ushort>();
        private string[] carList = Array.Empty<string>();
        private List<byte>[] carColours = Array.Empty<List<byte>>();
        private string[] carNames = Array.Empty<string>();
        private ushort[] arcadeCarIDs = Array.Empty<ushort>();
        private string[] musicCodes = Array.Empty<string>();
        private uint[] musicData = Array.Empty<uint>();
        private string[] musicNames = Array.Empty<string>();
        private uint[] streamData = Array.Empty<uint>();
        private List<string> otherSettings = new();

        public void ReadFromBinary(string filename)
        {
            using (FileStream file = new(filename, FileMode.Open, FileAccess.Read))
            {
                if (file.ReadCharacters() != "@(#)GTENV")
                {
                    throw new Exception("Not a GTENV file.");
                }

                unknownCount = file.ReadUShort();
                courseCount = file.ReadUShort();
                carCount = file.ReadUShort();
                carcadeCount = file.ReadUShort();
                musicCount = file.ReadUShort();
                streamCount = file.ReadUShort();
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

                unknownData = new ushort[16];
                for (int i = 0; i < 16; i++)
                {
                    unknownData[i] = file.ReadUShort();
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

                streamData = new uint[streamCount * 2];
                for (int i = 0; i < streamCount * 2; i++)
                {
                    streamData[i] = file.ReadUInt();
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

        public void ReadFromEditable(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new Exception("Directory does not exist.");
            }

            CsvConfiguration csvConfig = new(CultureInfo.CurrentCulture) { ShouldQuote = (args) => true };

            using (FileStream file = new(Path.Combine(directory, "Courses.csv"), FileMode.Open, FileAccess.Read))
            {
                using (StreamReader writer = new(file))
                {
                    using (CsvReader csv = new(writer, csvConfig))
                    {
                        List<string> readCourseCodes = new();
                        List<string> readCourseNames = new();
                        List<byte> readCourseBGs = new();
                        courseModes = new();
                        csv.Read();
                        while (csv.Read())
                        {
                            readCourseCodes.Add(csv.GetField(0) ?? "");
                            readCourseNames.Add(csv.GetField(1) ?? "");
                            readCourseBGs.Add(byte.Parse(csv.GetField(2) ?? ""));

                            var readCourseMode = new byte[16];
                            for (int i = 0; i < 16; i++)
                            {
                                readCourseMode[i] = byte.Parse(csv.GetField(i + 3) ?? "");
                            }
                            courseModes.Add(readCourseMode);
                        }
                        courseCodes = readCourseCodes.ToArray();
                        courseNames = readCourseNames.ToArray();
                        courseBGs = readCourseBGs.ToArray();
                        courseCount = (ushort)courseCodes.Length;
                    }
                }
            }

            SortedDictionary<string, string> cars = new();
            ReadCars(Path.Combine(directory, "Cars.csv"), csvConfig, cars);
            foreach (string csvPath in Directory.EnumerateFiles(directory, "Cars_*.csv"))
            {
                ReadCars(csvPath, csvConfig, cars);
            }
            carList = cars.Keys.ToArray();
            carNames = cars.Values.ToArray();
            carCount = (ushort)carList.Length;

            SortedDictionary<string, string> readCarColours = new();
            ReadCarColours(Path.Combine(directory, "CarColours.csv"), csvConfig, readCarColours);
            foreach (string csvPath in Directory.EnumerateFiles(directory, "CarColours_*.csv"))
            {
                ReadCarColours(csvPath, csvConfig, readCarColours);
            }

            if (readCarColours.Keys.Count != carCount || !readCarColours.Keys.SequenceEqual(carList))
            {
                throw new Exception("Car IDs in CarColours don't match car IDs in Cars.");
            }

            List<List<byte>> parsedCarColours = new(readCarColours.Values.Count);
            foreach (string readIDs in readCarColours.Values)
            {
                List<byte> parsedIDs = readIDs.Split(',').Select(colourID => byte.Parse(colourID, NumberStyles.HexNumber)).ToList();
                if (parsedIDs.Count > 16)
                {
                    throw new Exception("Max of 16 colours per car.");
                }
                parsedCarColours.Add(parsedIDs);
            }
            carColours = parsedCarColours.ToArray();

            using (FileStream file = new(Path.Combine(directory, "ArcadeCars.csv"), FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new(file))
                {
                    using (CsvReader csv = new(reader, csvConfig))
                    {
                        List<ushort> readIDs = new();
                        csv.Read();
                        while (csv.Read())
                        {
                            readIDs.Add(ushort.Parse(csv.GetField(0) ?? "", NumberStyles.HexNumber));
                        }
                        arcadeCarIDs = readIDs.ToArray();
                        carcadeCount = (ushort)arcadeCarIDs.Length;
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "Music.csv"), FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new(file))
                {
                    using (CsvReader csv = new(reader, csvConfig))
                    {
                        List<string> readMusicCodes = new();
                        List<string> readMusicNames = new();
                        List<uint> readMusicData = new();
                        csv.Read();
                        while (csv.Read())
                        {
                            readMusicCodes.Add(csv.GetField(0) ?? "");
                            readMusicNames.Add(csv.GetField(1) ?? "");
                            uint streamNumber = uint.Parse(csv.GetField(2) ?? "", NumberStyles.HexNumber);
                            uint startPosition = uint.Parse(csv.GetField(3) ?? "", NumberStyles.HexNumber);
                            readMusicData.Add((startPosition << 2) + (streamNumber & 3));
                            readMusicData.Add(uint.Parse(csv.GetField(4) ?? "", NumberStyles.HexNumber) << 2);
                        }
                        musicCodes = readMusicCodes.ToArray();
                        musicNames = readMusicNames.ToArray();
                        musicData = readMusicData.ToArray();
                        musicCount = (ushort)musicCodes.Length;
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "Streams.csv"), FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new(file))
                {
                    using (CsvReader csv = new(reader, csvConfig))
                    {
                        List<uint> readValues = new();
                        csv.Read();
                        while (csv.Read())
                        {
                            readValues.Add(uint.Parse(csv.GetField(0) ?? "", NumberStyles.HexNumber));
                            readValues.Add(uint.Parse(csv.GetField(1) ?? "", NumberStyles.HexNumber));
                        }
                        streamData = readValues.ToArray();
                        streamCount = (ushort)(readValues.Count / 2);
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "UnknownData.csv"), FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new(file))
                {
                    using (CsvReader csv = new(reader, csvConfig))
                    {
                        List<ushort> readValues = new();
                        csv.Read();
                        while (csv.Read())
                        {
                            readValues.Add(ushort.Parse(csv.GetField(0) ?? "", NumberStyles.HexNumber));
                        }
                        unknownData = readValues.ToArray();
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "UnknownValues.csv"), FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new(file))
                {
                    using (CsvReader csv = new(reader, csvConfig))
                    {
                        csv.Read();
                        csv.Read();
                        unknownCount = ushort.Parse(csv.GetField(0) ?? "", NumberStyles.HexNumber);
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "Settings.txt"), FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new(file))
                {
                    otherSettings = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        otherSettings.Add(reader.ReadLine() ?? "");
                    }
                    otherSettings = otherSettings.Where(setting => !string.IsNullOrWhiteSpace(setting)).ToList();
                }
            }
        }

        private void ReadCars(string path, CsvConfiguration csvConfig, SortedDictionary<string, string> cars)
        {
            using (FileStream file = new(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new(file))
                {
                    using (CsvReader csv = new(reader, csvConfig))
                    {
                        csv.Read();
                        while (csv.Read())
                        {
                            if (!string.IsNullOrWhiteSpace(csv.GetField(0)))
                            {
                                cars[csv.GetField(0) ?? ""] = csv.GetField(1) ?? "";
                            }
                        }
                    }
                }
            }
        }

        private void ReadCarColours(string path, CsvConfiguration csvConfig, SortedDictionary<string, string> carColours)
        {
            using (FileStream file = new(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new(file))
                {
                    using (CsvReader csv = new(reader, csvConfig))
                    {
                        csv.Read();
                        while (csv.Read())
                        {
                            carColours[csv.GetField(0) ?? ""] = csv.GetField(1) ?? "";
                        }
                    }
                }
            }
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
                    writer.WriteLine($"streams.code={string.Join(',', Enumerable.Range(0, streamCount).Select(i => $"{streamData[i * 2]:X},{streamData[(i * 2) + 1]:X}"))}");
                    foreach (string setting in otherSettings)
                    {
                        writer.WriteLine(setting);
                    }
                    writer.WriteLine($"unknown.count={unknownCount}");
                    writer.WriteLine($"unknown.code={string.Join(',', unknownData.Select(value => $"{value:X4}"))}");
                }
            }
        }

        public void WriteToBinary(string filename)
        {
            using (FileStream file = new(filename, FileMode.Create, FileAccess.Write))
            {
                file.WriteCharacters("@(#)GTENV");
                file.WriteByte(0);

                file.WriteUShort(unknownCount);
                file.WriteUShort(courseCount);
                file.WriteUShort(carCount);
                file.WriteUShort(carcadeCount);
                file.WriteUShort(musicCount);
                file.WriteUShort(streamCount);
                file.Position = 0x48;

                WriteStrings(file, courseCodes);

                for (int i = 0; i < courseCount; i++)
                {
                    byte[] courseMode = courseModes[i];
                    courseMode[0] = (byte)(courseBGs[i] | (courseMode[0] << 7));
                    file.Write(courseMode);
                }

                WriteStrings(file, courseNames);

                foreach (ushort unknown in unknownData)
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

                foreach (uint streamDataItem in streamData)
                {
                    file.WriteUInt(streamDataItem);
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
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("Code");
                        csv.WriteField("Name");
                        csv.WriteField("Background");
                        csv.WriteField("IsNight");
                        csv.WriteField("Unknown1");
                        csv.WriteField("Unknown2");
                        csv.WriteField("Unknown3");
                        csv.WriteField("Unknown4");
                        csv.WriteField("Unknown5");
                        csv.WriteField("Unknown6");
                        csv.WriteField("Unknown7");
                        csv.WriteField("Unknown8");
                        csv.WriteField("Unknown9");
                        csv.WriteField("Unknown10");
                        csv.WriteField("Unknown11");
                        csv.WriteField("Unknown12");
                        csv.WriteField("Unknown13");
                        csv.WriteField("Unknown14");
                        csv.WriteField("Unknown15");
                        csv.NextRecord();

                        for (int i = 0; i < courseCount; i++)
                        {
                            csv.WriteField(courseCodes[i]);
                            csv.WriteField(courseNames[i]);
                            csv.WriteField(courseBGs[i]);

                            foreach (byte value in courseModes[i])
                            {
                                csv.WriteField(value);
                            }
                            csv.NextRecord();
                        }
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
                        csv.WriteField("ColourIDs");
                        csv.NextRecord();

                        for (int i = 0; i < carCount; i++)
                        {
                            csv.WriteField(carList[i]);
                            csv.WriteField(string.Join(',', carColours[i].Select(colourID => $"{colourID:X2}")));
                            csv.NextRecord();
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
                        csv.WriteField("Stream");
                        csv.WriteField("StartPosition");
                        csv.WriteField("Length");
                        csv.NextRecord();

                        for (int i = 0; i < musicCount; i++)
                        {
                            csv.WriteField(musicCodes[i]);
                            csv.WriteField(musicNames[i]);
                            csv.WriteField($"{musicData[i * 2] & 3:X4}");
                            csv.WriteField($"{musicData[i * 2] >> 2:X4}");
                            csv.WriteField($"{musicData[(i * 2) + 1] >> 2:X4}");
                            csv.NextRecord();
                        }
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "Streams.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("StartOffset");
                        csv.WriteField("Length");
                        csv.NextRecord();

                        for (int i = 0; i < streamCount; i++)
                        {
                            csv.WriteField($"{streamData[i * 2]:X8}");
                            csv.WriteField($"{streamData[(i * 2) + 1]:X8}");
                            csv.NextRecord();
                        }
                    }
                }
            }

            using (FileStream file = new(Path.Combine(directory, "UnknownData.csv"), FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new(file))
                {
                    using (CsvWriter csv = new(writer, csvConfig))
                    {
                        csv.WriteField("Unknown1");
                        csv.NextRecord();

                        foreach (ushort unknown in unknownData)
                        {
                            csv.WriteField($"{unknown:X4}");
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
                        csv.NextRecord();

                        csv.WriteField($"{unknownCount:X4}");
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