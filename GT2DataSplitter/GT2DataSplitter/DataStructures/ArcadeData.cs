using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace GT2.DataSplitter
{
    using StreamExtensions;

    public class ArcadeData
    {
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
        public List<ArcadeUnknown1> Unknown1 { get; set; } = new List<ArcadeUnknown1>();
        public List<ArcadeUnknown2> Unknown2 { get; set; } = new List<ArcadeUnknown2>();
        public List<ArcadeUnknown3> Unknown3 { get; set; } = new List<ArcadeUnknown3>();
        public List<ArcadeUnknown4> Unknown4 { get; set; } = new List<ArcadeUnknown4>();
        public List<ArcadeUnknown5> Unknown5 { get; set; } = new List<ArcadeUnknown5>();
        public List<ArcadeUnknown6> Unknown6 { get; set; } = new List<ArcadeUnknown6>();
        public List<Event> Events { get; set; } = new List<Event>();
        public List<EnemyCarsArcade> EnemyCars { get; set; } = new List<EnemyCarsArcade>();
        public List<CarArcade> Cars { get; set; } = new List<CarArcade>();
        public List<CarArcadeSports> CarsSports { get; set; } = new List<CarArcadeSports>();
        
        public void ReadData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var blocks = new List<DataBlock>();

                for (int i = 1; i <= 34; i++)
                {
                    file.Position = 8 * i;
                    uint blockStart = file.ReadUInt();
                    uint blockSize = file.ReadUInt();
                    blocks.Add(new DataBlock { BlockStart = blockStart, BlockSize = blockSize });
                }

                BrakeParts.Read(file, blocks[0].BlockStart, blocks[0].BlockSize);
                BrakeControllerParts.Read(file, blocks[1].BlockStart, blocks[1].BlockSize);
                SteerParts.Read(file, blocks[2].BlockStart, blocks[2].BlockSize);
                ChassisParts.Read(file, blocks[3].BlockStart, blocks[3].BlockSize);
                LightweightParts.Read(file, blocks[4].BlockStart, blocks[4].BlockSize);
                RacingModifyParts.Read(file, blocks[5].BlockStart, blocks[5].BlockSize);
                EngineParts.Read(file, blocks[6].BlockStart, blocks[6].BlockSize);
                PortPolishParts.Read(file, blocks[7].BlockStart, blocks[7].BlockSize);
                EngineBalanceParts.Read(file, blocks[8].BlockStart, blocks[8].BlockSize);
                DisplacementParts.Read(file, blocks[9].BlockStart, blocks[9].BlockSize);
                ComputerParts.Read(file, blocks[10].BlockStart, blocks[10].BlockSize);
                NATuneParts.Read(file, blocks[11].BlockStart, blocks[11].BlockSize);
                TurbineKitParts.Read(file, blocks[12].BlockStart, blocks[12].BlockSize);
                DrivetrainParts.Read(file, blocks[13].BlockStart, blocks[13].BlockSize);
                FlywheelParts.Read(file, blocks[14].BlockStart, blocks[14].BlockSize);
                ClutchParts.Read(file, blocks[15].BlockStart, blocks[15].BlockSize);
                PropellerShaftParts.Read(file, blocks[16].BlockStart, blocks[16].BlockSize);
                GearParts.Read(file, blocks[17].BlockStart, blocks[17].BlockSize);
                SuspensionParts.Read(file, blocks[18].BlockStart, blocks[18].BlockSize);
                IntercoolerParts.Read(file, blocks[19].BlockStart, blocks[19].BlockSize);
                MufflerParts.Read(file, blocks[20].BlockStart, blocks[20].BlockSize);
                LSDParts.Read(file, blocks[21].BlockStart, blocks[21].BlockSize);
                TiresFrontParts.Read(file, blocks[22].BlockStart, blocks[22].BlockSize);
                TiresRearParts.Read(file, blocks[23].BlockStart, blocks[23].BlockSize);
                Unknown1.Read(file, blocks[24].BlockStart, blocks[24].BlockSize);
                Unknown2.Read(file, blocks[25].BlockStart, blocks[25].BlockSize);
                Unknown3.Read(file, blocks[26].BlockStart, blocks[26].BlockSize);
                Unknown4.Read(file, blocks[27].BlockStart, blocks[27].BlockSize);
                Unknown5.Read(file, blocks[28].BlockStart, blocks[28].BlockSize);
                Unknown6.Read(file, blocks[29].BlockStart, blocks[29].BlockSize);
                Events.Read(file, blocks[30].BlockStart, blocks[30].BlockSize);
                EnemyCars.Read(file, blocks[31].BlockStart, blocks[31].BlockSize);
                Cars.Read(file, blocks[32].BlockStart, blocks[32].BlockSize);
                CarsSports.Read(file, blocks[33].BlockStart, blocks[33].BlockSize);

                uint stringTableStart = blocks[33].BlockStart + blocks[33].BlockSize;
                RaceStringTable.Read(file, stringTableStart, (uint)file.Length - stringTableStart);
            }
        }

        public void DumpData()
        {
            BrakeParts.Dump();
            BrakeControllerParts.Dump();
            SteerParts.Dump();
            ChassisParts.Dump();
            LightweightParts.Dump();
            RacingModifyParts.Dump();
            EngineParts.Dump();
            PortPolishParts.Dump();
            EngineBalanceParts.Dump();
            DisplacementParts.Dump();
            ComputerParts.Dump();
            NATuneParts.Dump();
            TurbineKitParts.Dump();
            DrivetrainParts.Dump();
            FlywheelParts.Dump();
            ClutchParts.Dump();
            PropellerShaftParts.Dump();
            GearParts.Dump();
            SuspensionParts.Dump();
            IntercoolerParts.Dump();
            MufflerParts.Dump();
            LSDParts.Dump();
            TiresFrontParts.Dump();
            TiresRearParts.Dump();
            Unknown1.Dump();
            Unknown2.Dump();
            Unknown3.Dump();
            Unknown4.Dump();
            Unknown5.Dump();
            Unknown6.Dump();
            Events.Dump();
            EnemyCars.Dump();
            Cars.Dump();
            CarsSports.Dump();
        }

        public void ImportData()
        {
            // Todo
        }

        public void WriteData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                file.Write(new byte[] { 0x47, 0x54, 0x44, 0x54, 0x6C, 0x00, 0x44, 0x00 }, 0, 8); // The 0x44 is the number of indices

                file.Position = (0x44 * 8) + 7;
                file.WriteByte(0x00); // Data starts at 0x1F8 so position EOF
                
                // Todo

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

        public struct DataBlock
        {
            public uint BlockStart;
            public uint BlockSize;
        }
    }
}
