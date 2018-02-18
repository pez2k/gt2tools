using System.Collections.Generic;
using System.IO;

namespace GT2DataSplitter
{
    public class GTModeData
    {
        public List<Brakes> BrakeParts { get; set; } = new List<Brakes>();
        public List<BrakeBalanceController> BrakeBalanceControllerParts { get; set; } = new List<BrakeBalanceController>();
        public Steering SteeringPart { get; set; } = new Steering();
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
        public List<Gearbox> GearboxParts { get; set; } = new List<Gearbox>();
        public List<Suspension> SuspensionParts { get; set; } = new List<Suspension>();
        public List<Intercooler> IntercoolerParts { get; set; } = new List<Intercooler>();
        public List<Exhaust> ExhaustParts { get; set; } = new List<Exhaust>();
        public List<Differential> DifferentialParts { get; set; } = new List<Differential>();
        public List<TyresFront> TyresFrontParts { get; set; } = new List<TyresFront>();
        public List<TyresRear> TyresRearParts { get; set; } = new List<TyresRear>();
        public CarUnknown1 Unknown1 { get; set; } = new CarUnknown1();
        public CarUnknown2 Unknown2 { get; set; } = new CarUnknown2();
        public CarUnknown3 Unknown3 { get; set; } = new CarUnknown3();
        public CarUnknown4 Unknown4 { get; set; } = new CarUnknown4();
        public CarUnknown5 Unknown5 { get; set; } = new CarUnknown5();
        public CarUnknown6 Unknown6 { get; set; } = new CarUnknown6();
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
                BrakeBalanceControllerParts.Read(file, blocks[1].BlockStart, blocks[1].BlockSize);
                SteeringPart.Read(file);
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
                GearboxParts.Read(file, blocks[17].BlockStart, blocks[17].BlockSize);
                SuspensionParts.Read(file, blocks[18].BlockStart, blocks[18].BlockSize);
                IntercoolerParts.Read(file, blocks[19].BlockStart, blocks[19].BlockSize);
                ExhaustParts.Read(file, blocks[20].BlockStart, blocks[20].BlockSize);
                DifferentialParts.Read(file, blocks[21].BlockStart, blocks[21].BlockSize);
                TyresFrontParts.Read(file, blocks[22].BlockStart, blocks[22].BlockSize);
                TyresRearParts.Read(file, blocks[23].BlockStart, blocks[23].BlockSize);
                Unknown1.Read(file);
                Unknown2.Read(file);
                Unknown3.Read(file);
                Unknown4.Read(file);
                Unknown5.Read(file);
                Unknown6.Read(file);
                Cars.Read(file, blocks[30].BlockStart, blocks[30].BlockSize);
            }
        }

        public void DumpData()
        {
            BrakeParts.Dump();
            BrakeBalanceControllerParts.Dump();
            SteeringPart.Dump();
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
            GearboxParts.Dump();
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
            Cars.Dump();
        }

        public struct DataBlock
        {
            public uint BlockStart;
            public uint BlockSize;
        }
    }
}
