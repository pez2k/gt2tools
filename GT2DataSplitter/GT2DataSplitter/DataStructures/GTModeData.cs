using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace GT2.DataSplitter
{
    using StreamExtensions;

    public class GTModeData
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
        public List<TireSize> TireSizes { get; set; } = new List<TireSize>();
        public List<TireCompound> TireCompounds { get; set; } = new List<TireCompound>();
        public List<TireForceVol> TireForceVols { get; set; } = new List<TireForceVol>();
        public List<ActiveStabilityControl> ActiveStabilityControlParts { get; set; } = new List<ActiveStabilityControl>();
        public List<TractionControlSystem> TractionControlSystemParts { get; set; } = new List<TractionControlSystem>();
        public List<CarUnknown> Unknown { get; set; } = new List<CarUnknown>();
        public List<Car> Cars { get; set; } = new List<Car>();
        
        public void ReadData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var blocks = new List<DataBlock>();

                for (int i = 1; i <= 31; i++)
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
                TireSizes.Read(file, blocks[24].BlockStart, blocks[24].BlockSize);
                TireCompounds.Read(file, blocks[25].BlockStart, blocks[25].BlockSize);
                TireForceVols.Read(file, blocks[26].BlockStart, blocks[26].BlockSize);
                ActiveStabilityControlParts.Read(file, blocks[27].BlockStart, blocks[27].BlockSize);
                TractionControlSystemParts.Read(file, blocks[28].BlockStart, blocks[28].BlockSize);
                Unknown.Read(file, blocks[29].BlockStart, blocks[29].BlockSize);
                Cars.Read(file, blocks[30].BlockStart, blocks[30].BlockSize);
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
            TireSizes.Dump();
            TireCompounds.Dump();
            TireForceVols.Dump();
            TiresFrontParts.Dump();
            TiresRearParts.Dump();
            ActiveStabilityControlParts.Dump();
            TractionControlSystemParts.Dump();
            Unknown.Dump();
            Cars.Dump();
        }

        public void ImportData()
        {
            BrakeParts.Import();
            BrakeControllerParts.Import();
            SteerParts.Import();
            ChassisParts.Import();
            LightweightParts.Import();
            RacingModifyParts.Import();
            EngineParts.Import();
            PortPolishParts.Import();
            EngineBalanceParts.Import();
            DisplacementParts.Import();
            ComputerParts.Import();
            NATuneParts.Import();
            TurbineKitParts.Import();
            DrivetrainParts.Import();
            FlywheelParts.Import();
            ClutchParts.Import();
            PropellerShaftParts.Import();
            GearParts.Import();
            SuspensionParts.Import();
            IntercoolerParts.Import();
            MufflerParts.Import();
            LSDParts.Import();
            TireSizes.Import();
            TireCompounds.Import();
            TireForceVols.Import();
            TiresFrontParts.Import();
            TiresRearParts.Import();
            ActiveStabilityControlParts.Import();
            TractionControlSystemParts.Import();
            Unknown.Import();
            Cars.Import();
        }

        public void WriteData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                file.Write(new byte[] { 0x47, 0x54, 0x44, 0x54, 0x6C, 0x00, 0x3E, 0x00 }, 0, 8); // The 0x3E is the number of indices

                file.Position = (0x3E * 8) + 7;
                file.WriteByte(0x00); // Data starts at 0x1F8 so position EOF

                uint i = 1;
                BrakeParts.Write(file, 8 * i++);
                BrakeControllerParts.Write(file, 8 * i++);
                SteerParts.Write(file, 8 * i++);
                ChassisParts.Write(file, 8 * i++);
                LightweightParts.Write(file, 8 * i++);
                RacingModifyParts.Write(file, 8 * i++);
                EngineParts.Write(file, 8 * i++);
                PortPolishParts.Write(file, 8 * i++);
                EngineBalanceParts.Write(file, 8 * i++);
                DisplacementParts.Write(file, 8 * i++);
                ComputerParts.Write(file, 8 * i++);
                NATuneParts.Write(file, 8 * i++);
                TurbineKitParts.Write(file, 8 * i++);
                DrivetrainParts.Write(file, 8 * i++);
                FlywheelParts.Write(file, 8 * i++);
                ClutchParts.Write(file, 8 * i++);
                PropellerShaftParts.Write(file, 8 * i++);
                GearParts.Write(file, 8 * i++);
                SuspensionParts.Write(file, 8 * i++);
                IntercoolerParts.Write(file, 8 * i++);
                MufflerParts.Write(file, 8 * i++);
                LSDParts.Write(file, 8 * i++);
                TiresFrontParts.Write(file, 8 * i++);
                TiresRearParts.Write(file, 8 * i++);
                TireSizes.Write(file, 8 * i++);
                TireCompounds.Write(file, 8 * i++);
                TireForceVols.Write(file, 8 * i++);
                ActiveStabilityControlParts.Write(file, 8 * i++);
                TractionControlSystemParts.Write(file, 8 * i++);
                Unknown.Write(file, 8 * i++);
                Cars.Write(file, 8 * i++);

                if (file.Length > 800 * 1024)
                {
                    throw new Exception("gtmode_data.dat exceeds 800kb size limit.");
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

        public struct DataBlock
        {
            public uint BlockStart;
            public uint BlockSize;
        }
    }
}
