using StreamExtensions;
using System.Text;

namespace GT1.SystemEnvEditor
{
    internal class Program
    {
        private readonly static string[] bgNames = new string[] { "Test", "tl_sky2", "tl_sky2g", "dawn", "dawn2", "dawn3", "c1_bg", "noon", "au", "m_sky_X" };

        static void Main()
        {
            using (FileStream file = new("SYSTEM.DAT", FileMode.Open, FileAccess.Read))
            {
                if (file.ReadCharacters() != "@(#)GTENV")
                {
                    Console.WriteLine("Not a GTENV file.");
                    return;
                }

                ushort unknownCount1 = file.ReadUShort();
                ushort courseCount = file.ReadUShort();
                ushort carCount = file.ReadUShort();
                ushort carcadeCount = file.ReadUShort();
                ushort musicCount = file.ReadUShort();
                ushort unknownCount2 = file.ReadUShort();
                file.Position = 0x48;

                string[] courseCodes = ReadStrings(file, courseCount);

                List<byte[]> courseModes = new(courseCount);
                var courseBGs = new byte[courseCount];
                for (int i = 0; i < courseCount; i++)
                {
                    var courseMode = new byte[16];
                    file.Read(courseMode);
                    byte firstByte = courseMode[0];
                    courseBGs[i] = (byte)(firstByte & 0x7F);
                    courseMode[0] = (byte)(firstByte >> 7);
                    courseModes.Add(courseMode);
                }

                string[] courseNames = ReadStrings(file, courseCount);
                
                var unknownData1 = new byte[32];
                file.Read(unknownData1);

                var carList = new string[carCount];
                for (int i = 0; i < carCount; i++)
                {
                    carList[i] = file.ReadCharacters();
                }

                var carColours = new List<byte>[carCount];
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

                string[] carNames = ReadStrings(file, carCount);
                for (int i = 0; i < carCount; i++)
                {
                    carNames[i] = carNames[i].Replace($"{(char)0x7F}", "[R]");
                }

                ushort[] arcadeCarIDs = new ushort[carcadeCount];
                for (int i = 0; i < carcadeCount; i++)
                {
                    arcadeCarIDs[i] = file.ReadUShort();
                }

                string[] musicCodes = ReadStrings(file, musicCount);
                var musicData = new uint[musicCount * 2];
                for (int i = 0; i < musicCount * 2; i++)
                {
                    musicData[i] = file.ReadUInt();
                }
                string[] musicNames = ReadStrings(file, musicCount);

                var unknownData2 = new uint[22];
                for (int i = 0; i < 22; i++)
                {
                    unknownData2[i] = file.ReadUInt();
                }

                List<string> otherSettings = new();
                while (file.Position != file.Length)
                {
                    otherSettings.Add(file.ReadCharacters());
                }
                otherSettings = otherSettings.Where(setting => !string.IsNullOrWhiteSpace(setting)).ToList();

                using (FileStream output = new("SYSTEM.ENV", FileMode.Create, FileAccess.Write))
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
                            writer.WriteLine($"course.bg.{i}=BG {bgNames[courseBGs[i]]}");
                        }
                        writer.WriteLine($"course.code={string.Join(',', courseCodes)}");
                        writer.WriteLine($"music.code={musicCount},{string.Join(',', Enumerable.Range(0, musicCount).Select(i => $"{musicCodes[i]},{musicData[i*2]:X},{musicData[(i * 2) + 1]:X}"))}");
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
                        writer.WriteLine($"unknown.data.1={string.Join(',', unknownData1.Select(value => $"{value:X2}"))}");
                        writer.WriteLine($"unknown.data.2={string.Join(',', unknownData2.Select(value => $"{value:X8}"))}");
                    }
                }
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
    }
}