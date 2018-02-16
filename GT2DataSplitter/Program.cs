using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GT2DataSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                BuildFile("eng_gtmode_data.dat");
                return;
            }

            string filename = args[0];
            string extension = Path.GetExtension(filename);
            
            if (extension == ".gz")
            {
                string innerFilename = Path.GetFileNameWithoutExtension(filename);
                extension = Path.GetExtension(innerFilename);

                if (extension != ".dat")
                {
                    return;
                }

                using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    // Un-gzip
                    using (GZipStream unzip = new GZipStream(infile, CompressionMode.Decompress))
                    {
                        filename = innerFilename;
                        using (FileStream outfile = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                        {
                            unzip.CopyTo(outfile);
                        }
                    }
                }
            }

            if (extension == ".dat")
            {
                SplitFile(filename);
            }
        }

        static Type[] dataTypes = { typeof(CarBrakes), typeof(CarBrakeBalanceController), typeof(CarSteering), typeof(CarDimensions), typeof(CarWeightReduction),
                                    typeof(CarRacingModification), typeof(CarEngineData), typeof(CarPortGrinding), typeof(CarEngineBalancing), typeof(CarDisplacementIncrease),
                                    typeof(CarChip), typeof(CarNATuning), typeof(CarTurboTuning), typeof(CarDrivetrain), typeof(CarFlywheel), typeof(CarClutch),
                                    typeof(CarPropshaft), typeof(CarGearbox), typeof(CarSuspension), typeof(CarIntercooler), typeof(CarMuffler), typeof(CarLSD),
                                    typeof(CarTyres), typeof(CarRearTyres), typeof(CarUnknown1), typeof(CarUnknown2), typeof(CarUnknown3), typeof(CarUnknown4),
                                    typeof(CarUnknown5), typeof(CarUnknown6), typeof(CarData) };

        static void SplitFile(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                int i = 1;
                foreach(Type dataType in dataTypes)
                {
                    file.Position = 8 * i;
                    uint blockStart = readUInt32(file);
                    uint blockSize = readUInt32(file);

                    readType(dataType, file, blockStart, blockSize);

                    i++;
                }
            }
        }

        static void BuildFile(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                file.Write(new byte[]{ 0x47, 0x54, 0x44, 0x54, 0x6C, 0x00, 0x3E, 0x00 }, 0, 8);

                file.Position = 0x1F7;
                file.WriteByte(0x00);
                uint i = 1;
                foreach (Type dataType in dataTypes)
                {
                    writeType(dataType, file, 8 * i);
                    i++;
                }

                file.Position = 0;
                using (FileStream zipFile = new FileStream(filename + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new GZipStream(zipFile, CompressionMode.Compress))
                    {
                        file.CopyTo(zip);
                    }
                }
            }
        }

        static uint readUInt32(FileStream fileToRead)
        {
            byte[] rawValue = new byte[4];
            fileToRead.Read(rawValue, 0, 4);
            return (uint)(rawValue[3] * 256 * 256 * 256 + rawValue[2] * 256 * 256 + rawValue[1] * 256 + rawValue[0]);
        }

        static uint readUInt32(byte[] arrayToRead)
        {
            return (uint)(arrayToRead[3] * 256 * 256 * 256 + arrayToRead[2] * 256 * 256 + arrayToRead[1] * 256 + arrayToRead[0]);
        }

        static void writeUInt32(FileStream fileToWrite, uint valueToWrite)
        {
            fileToWrite.Write(convertUInt32ToBytes(valueToWrite), 0, 4);
        }

        static byte[] convertUInt32ToBytes(uint valueToConvert)
        {
            byte[] byteArray = new byte[4];
            byteArray[3] = (byte)((valueToConvert / 256 / 256 / 256) % 256);
            byteArray[2] = (byte)((valueToConvert / 256 / 256) % 256);
            byteArray[1] = (byte)((valueToConvert / 256) % 256);
            byteArray[0] = (byte)(valueToConvert % 256);
            return byteArray;
        }

        static unsafe void readType(Type type, FileStream infile, uint blockStart, uint blockSize)
        {
            var typeInstance = Activator.CreateInstance(type);
            int typeSize = Marshal.SizeOf(typeInstance);

            if (blockSize % typeSize > 0)
            {
                return;
            }

            if (!Directory.Exists(type.Name)) {
                Directory.CreateDirectory(type.Name);
            }

            infile.Position = blockStart;
            long blockCount = blockSize / typeSize;

            for (int i = 0; i < blockCount; i++)
            {
                byte[] data = new byte[typeSize];
                infile.Read(data, 0, typeSize);
                
                string filename = type.Name;

                if (blockCount > 1)
                {
                    uint carID = readUInt32(data);
                    filename += "\\" + GetCarName(carID);

                    if (!Directory.Exists(filename))
                    {
                        Directory.CreateDirectory(filename);
                    }
                }

                filename += "\\" + Directory.GetFiles(filename).Length.ToString() + "0.dat";

                using (FileStream outfile = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    outfile.Write(data, 0, data.Length);
                }
            }
        }

        static void writeType(Type type, FileStream outfile, uint indexPosition)
        {
            Dictionary<uint, string> cars = new Dictionary<uint, string>();

            foreach (string carName in Directory.EnumerateDirectories(type.Name))
            {
                cars.Add(GetCarID(carName), carName);
            }

            if (cars.Count == 0)
            {
                cars.Add(0, type.Name);
            }

            outfile.Position = outfile.Length;
            uint startingPosition = (uint)outfile.Position;

            foreach (string carName in cars.Values)
            {
                foreach (string filename in Directory.EnumerateFiles(carName))
                {
                    using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        infile.CopyTo(outfile);
                    }
                }
            }

            uint blockSize = (uint)outfile.Position - startingPosition;
            outfile.Position = indexPosition;
            writeUInt32(outfile, startingPosition);
            writeUInt32(outfile, blockSize);
        }

        private static char[] characterSet = { '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private static List<char> characterList = characterSet.ToList();

        static string GetCarName(uint carID)
        {
            string carName = "";

            for (int i = 0; i < 5; i++)
            {
                uint currentCharNo = (carID >> (i * 6)) & 0x3F;
                carName = characterSet[currentCharNo] + carName;
            }

            return carName;
        }

        static uint GetCarID(string carName)
        {
            uint carID = 0;
            char[] carNameChars = carName.ToCharArray();
            long currentCarID = 0;

            foreach (char carNameChar in carNameChars)
            {
                currentCarID += characterList.IndexOf(carNameChar);
                currentCarID = currentCarID << 6;
            }
            currentCarID = currentCarID >> 6;
            carID = (uint)currentCarID;
            return carID;
        }
    }
}
