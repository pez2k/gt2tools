using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace GT2.DataSplitter
{
    using CarNameConversion;
    using StreamExtensions;

    public class DataFile
    {
        private readonly TypedData[] data;

        public List<Brake> BrakeParts { get; set; } = new List<Brake>();
        public List<BrakeController> BrakeControllerParts { get; set; } = new List<BrakeController>();
        public List<Steer> SteerParts { get; set; } = new List<Steer>();
        public List<Chassis> ChassisParts { get; set; } = new List<Chassis>();
        public List<Lightweight> LightweightParts { get; set; } = new List<Lightweight>();
        public List<RacingModify> RacingModifyParts { get; set; } = new List<RacingModify>();
        public List<Engine> EngineParts { get; set; } = new List<Engine>();
        public List<PortPolish> PortPolishParts { get; set; } = new List<PortPolish>();
        public List<EngineBalance> EngineBalanceParts { get; set; } = new List<EngineBalance>();
        public List<Displacement> DisplacementParts { get; set; } = new List<Displacement>();
        public List<Computer> ComputerParts { get; set; } = new List<Computer>();
        public List<NATune> NATuneParts { get; set; } = new List<NATune>();
        public List<TurbineKit> TurbineKitParts { get; set; } = new List<TurbineKit>();
        public List<Drivetrain> DrivetrainParts { get; set; } = new List<Drivetrain>();
        public List<Flywheel> FlywheelParts { get; set; } = new List<Flywheel>();
        public List<Clutch> ClutchParts { get; set; } = new List<Clutch>();
        public List<PropellerShaft> PropellerShaftParts { get; set; } = new List<PropellerShaft>();
        public List<Gear> GearParts { get; set; } = new List<Gear>();
        public List<Suspension> SuspensionParts { get; set; } = new List<Suspension>();
        public List<Intercooler> IntercoolerParts { get; set; } = new List<Intercooler>();
        public List<Muffler> MufflerParts { get; set; } = new List<Muffler>();
        public List<LSD> LSDParts { get; set; } = new List<LSD>();
        public List<TiresFront> TiresFrontParts { get; set; } = new List<TiresFront>();
        public List<TiresRear> TiresRearParts { get; set; } = new List<TiresRear>();
        public List<TireSize> TireSizes { get; set; } = new List<TireSize>();
        public List<TireCompound> TireCompounds { get; set; } = new List<TireCompound>();
        public List<TireForceVol> TireForceVols { get; set; } = new List<TireForceVol>();
        public List<ActiveStabilityControl> ActiveStabilityControlParts { get; set; } = new List<ActiveStabilityControl>();
        public List<TractionControlSystem> TractionControlSystemParts { get; set; } = new List<TractionControlSystem>();
        public List<CarUnknown> Unknown { get; set; } = new List<CarUnknown>();
        public List<Car> Cars { get; set; } = new List<Car>();
        
        public DataFile(params (Type type, int orderOnDisk)[] dataDefinitions)
        {
            data = dataDefinitions.Select(definition => new TypedData(definition.type, definition.orderOnDisk)).ToArray();
        }

        public void ReadData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                ReadDataFromFile(file);
            }
        }

        protected virtual void ReadDataFromFile(Stream file)
        {
            for (int i = 0; i < data.Length; i++)
            {
                file.Position = 8 * (i + 1);
                uint blockStart = file.ReadUInt();
                uint blockSize = file.ReadUInt();
                Read(file, blockStart, blockSize, data[i]);
            }
        }

        private void Read(Stream file, uint blockStart, uint blockSize, TypedData data)
        {
            var template = (DataStructure)Activator.CreateInstance(data.Type);
            Console.WriteLine($"Reading {template.Name} structures from file...");

            if (blockSize % template.Size > 0)
            {
                Console.WriteLine($"Invalid block size for {template.Name}!");
                return;
            }

            long previousPosition = file.Position;
            file.Position = blockStart;
            long blockCount = blockSize / template.Size;

            for (int i = 0; i < blockCount; i++)
            {
                var structure = (DataStructure)Activator.CreateInstance(data.Type);
                structure.Read(file);
                data.Structures.Add(structure);
            }
            file.Position = previousPosition;
        }

        public void DumpData()
        {
            foreach (TypedData item in data.OrderBy(item => item.OrderOnDisk))
            {
                Dump(item);
            }
        }

        private void Dump(TypedData data)
        {
            var template = (DataStructure)Activator.CreateInstance(data.Type);
            Console.WriteLine($"Dumping {template.Name} structures to disk...");

            if (!Directory.Exists(template.Name))
            {
                Directory.CreateDirectory(template.Name);
            }

            foreach (DataStructure structure in data.Structures)
            {
                structure.Dump();
            }
        }

        public void ImportData()
        {
            foreach (TypedData item in data.OrderBy(item => item.OrderOnDisk))
            {
                Import(item);
            }
        }

        private void Import(TypedData data)
        {
            var template = (DataStructure)Activator.CreateInstance(data.Type);
            Console.WriteLine($"Importing {template.Name} structures from disk...");

            var cars = new Dictionary<uint, string>();
            foreach (string carName in Directory.EnumerateDirectories(template.Name))
            {
                cars.Add(carName.ToCarID(), carName);
            }

            if (cars.Count == 0)
            {
                cars.Add(0, template.Name);
            }

            bool hasOverrides = Program.OverridePath != null && Directory.Exists(Path.Combine(Program.OverridePath, template.Name));
            foreach (string carName in cars.Values)
            {
                foreach (string baseFilename in Directory.EnumerateFiles(carName))
                {
                    string filename = hasOverrides ? Path.Combine(Program.OverridePath, baseFilename) : baseFilename;
                    if (!File.Exists(filename))
                    {
                        filename = baseFilename;
                    }

                    if (new FileInfo(filename).Length > 0)
                    {
                        var structure = (DataStructure)Activator.CreateInstance(data.Type);
                        structure.Import(filename);
                        data.Structures.Add(structure);
                    }
                }
            }
        }

        public void WriteData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                WriteDataToFile(file);

                if (file.Length > 0xC8000)
                {
                    throw new Exception($"{filename} exceeds 800kb size limit.");
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

        protected virtual void WriteDataToFile(Stream file)
        {
            file.WriteCharacters("GTDTl\0");
            ushort indexCount = (ushort)(data.Length * 2);
            file.WriteUShort(indexCount);
            file.Position = (indexCount * 8) + 7;
            file.WriteByte(0x00); // Data starts at end of indices, so position EOF

            for (int i = 0; i < data.Length; i++)
            {
                Write(data[i], file, (i + 1) * 8);
            }
        }

        private void Write(TypedData data, Stream file, int indexPosition)
        {
            file.Position = file.Length;
            uint startingPosition = (uint)file.Position;

            foreach (DataStructure structure in data.Structures)
            {
                structure.Write(file);
            }

            uint blockSize = (uint)file.Position - startingPosition;
            file.Position = indexPosition;
            file.WriteUInt(startingPosition);
            file.WriteUInt(blockSize);
        }
    }
}