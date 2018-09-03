using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace GT2.DataSplitter
{
    using StreamExtensions;

    public class ArcadeData
    {
        public List<Brakes> BrakeParts { get; set; } = new List<Brakes>();
        public List<BrakeBalanceController> BrakeBalanceControllerParts { get; set; } = new List<BrakeBalanceController>();
        public List<Steering> SteeringParts { get; set; } = new List<Steering>();
        public List<Dimensions> DimensionsParts { get; set; } = new List<Dimensions>();
        public List<WeightReduction> WeightReductionParts { get; set; } = new List<WeightReduction>();
        public List<Body> BodyParts { get; set; } = new List<Body>();
        public List<Engine> EngineParts { get; set; } = new List<Engine>();
        public List<PortPolishing> PortPolishingParts { get; set; } = new List<PortPolishing>();
        public List<EngineBalancing> EngineBalancingParts { get; set; } = new List<EngineBalancing>();
        public List<DisplacementIncrease> DisplacementIncreaseParts { get; set; } = new List<DisplacementIncrease>();
        public List<Chip> ChipParts { get; set; } = new List<Chip>();
        public List<NATuning> NATuningParts { get; set; } = new List<NATuning>();
        public List<TurboKit> TurboKitParts { get; set; } = new List<TurboKit>();
        public List<Drivetrain> DrivetrainParts { get; set; } = new List<Drivetrain>();
        public List<Flywheel> FlywheelParts { get; set; } = new List<Flywheel>();
        public List<Clutch> ClutchParts { get; set; } = new List<Clutch>();
        public List<Propshaft> PropshaftParts { get; set; } = new List<Propshaft>();
        public List<Transmission> TransmissionParts { get; set; } = new List<Transmission>();
        public List<Suspension> SuspensionParts { get; set; } = new List<Suspension>();
        public List<Intercooler> IntercoolerParts { get; set; } = new List<Intercooler>();
        public List<Exhaust> ExhaustParts { get; set; } = new List<Exhaust>();
        public List<Differential> DifferentialParts { get; set; } = new List<Differential>();
        public List<TyresFront> TyresFrontParts { get; set; } = new List<TyresFront>();
        public List<TyresRear> TyresRearParts { get; set; } = new List<TyresRear>();
        public List<ArcadeUnknown1> Unknown1 { get; set; } = new List<ArcadeUnknown1>();
        public List<ArcadeUnknown2> Unknown2 { get; set; } = new List<ArcadeUnknown2>();
        public List<ArcadeUnknown3> Unknown3 { get; set; } = new List<ArcadeUnknown3>();
        public List<ArcadeUnknown4> Unknown4 { get; set; } = new List<ArcadeUnknown4>();
        public List<ArcadeUnknown5> Unknown5 { get; set; } = new List<ArcadeUnknown5>();
        public List<ArcadeUnknown6> Unknown6 { get; set; } = new List<ArcadeUnknown6>();
        public List<Race> Races { get; set; } = new List<Race>();
        public List<LicenseCar> Unknown7 { get; set; } = new List<LicenseCar>(); // unknown
        public List<CarArcadeMaybe> CarsMaybe { get; set; } = new List<CarArcadeMaybe>();
        public List<OpponentArcadeMaybe> OpponentsMaybe { get; set; } = new List<OpponentArcadeMaybe>();
        
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
                BrakeBalanceControllerParts.Read(file, blocks[1].BlockStart, blocks[1].BlockSize);
                SteeringParts.Read(file, blocks[2].BlockStart, blocks[2].BlockSize);
                DimensionsParts.Read(file, blocks[3].BlockStart, blocks[3].BlockSize);
                WeightReductionParts.Read(file, blocks[4].BlockStart, blocks[4].BlockSize);
                BodyParts.Read(file, blocks[5].BlockStart, blocks[5].BlockSize);
                EngineParts.Read(file, blocks[6].BlockStart, blocks[6].BlockSize);
                PortPolishingParts.Read(file, blocks[7].BlockStart, blocks[7].BlockSize);
                EngineBalancingParts.Read(file, blocks[8].BlockStart, blocks[8].BlockSize);
                DisplacementIncreaseParts.Read(file, blocks[9].BlockStart, blocks[9].BlockSize);
                ChipParts.Read(file, blocks[10].BlockStart, blocks[10].BlockSize);
                NATuningParts.Read(file, blocks[11].BlockStart, blocks[11].BlockSize);
                TurboKitParts.Read(file, blocks[12].BlockStart, blocks[12].BlockSize);
                DrivetrainParts.Read(file, blocks[13].BlockStart, blocks[13].BlockSize);
                FlywheelParts.Read(file, blocks[14].BlockStart, blocks[14].BlockSize);
                ClutchParts.Read(file, blocks[15].BlockStart, blocks[15].BlockSize);
                PropshaftParts.Read(file, blocks[16].BlockStart, blocks[16].BlockSize);
                TransmissionParts.Read(file, blocks[17].BlockStart, blocks[17].BlockSize);
                SuspensionParts.Read(file, blocks[18].BlockStart, blocks[18].BlockSize);
                IntercoolerParts.Read(file, blocks[19].BlockStart, blocks[19].BlockSize);
                ExhaustParts.Read(file, blocks[20].BlockStart, blocks[20].BlockSize);
                DifferentialParts.Read(file, blocks[21].BlockStart, blocks[21].BlockSize);
                TyresFrontParts.Read(file, blocks[22].BlockStart, blocks[22].BlockSize);
                TyresRearParts.Read(file, blocks[23].BlockStart, blocks[23].BlockSize);
                Unknown1.Read(file, blocks[24].BlockStart, blocks[24].BlockSize);
                Unknown2.Read(file, blocks[25].BlockStart, blocks[25].BlockSize);
                Unknown3.Read(file, blocks[26].BlockStart, blocks[26].BlockSize);
                Unknown4.Read(file, blocks[27].BlockStart, blocks[27].BlockSize);
                Unknown5.Read(file, blocks[28].BlockStart, blocks[28].BlockSize);
                Unknown6.Read(file, blocks[29].BlockStart, blocks[29].BlockSize);
                Races.Read(file, blocks[30].BlockStart, blocks[30].BlockSize);
                Unknown7.Read(file, blocks[31].BlockStart, blocks[31].BlockSize);
                CarsMaybe.Read(file, blocks[32].BlockStart, blocks[32].BlockSize);
                OpponentsMaybe.Read(file, blocks[33].BlockStart, blocks[33].BlockSize);

                uint stringTableStart = blocks[33].BlockStart + blocks[33].BlockSize;
                RaceStringTable.Read(file, stringTableStart, (uint)file.Length - stringTableStart);
            }
        }

        public void DumpData()
        {
            BrakeParts.Dump();
            BrakeBalanceControllerParts.Dump();
            SteeringParts.Dump();
            DimensionsParts.Dump();
            WeightReductionParts.Dump();
            BodyParts.Dump();
            EngineParts.Dump();
            PortPolishingParts.Dump();
            EngineBalancingParts.Dump();
            DisplacementIncreaseParts.Dump();
            ChipParts.Dump();
            NATuningParts.Dump();
            TurboKitParts.Dump();
            DrivetrainParts.Dump();
            FlywheelParts.Dump();
            ClutchParts.Dump();
            PropshaftParts.Dump();
            TransmissionParts.Dump();
            SuspensionParts.Dump();
            IntercoolerParts.Dump();
            ExhaustParts.Dump();
            DifferentialParts.Dump();
            TyresFrontParts.Dump();
            TyresRearParts.Dump();
            Unknown1.Dump();
            Unknown2.Dump();
            Unknown3.Dump();
            Unknown4.Dump();
            Unknown5.Dump();
            Unknown6.Dump();
            Races.Dump();
            Unknown7.Dump();
            CarsMaybe.Dump();
            OpponentsMaybe.Dump();
        }

        public void ImportData()
        {
            // Todo
        }

        public void WriteData(string filename)
        {
            filename = "new_" + filename;

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
