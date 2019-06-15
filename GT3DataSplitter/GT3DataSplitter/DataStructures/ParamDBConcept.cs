using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace GT3.DataSplitter
{
    using StreamExtensions;

    public class ParamDBConcept
    {
        public List<Brake> BrakeParts { get; set; } = new List<Brake>();
        public List<BrakeController> BrakeBalanceControllerParts { get; set; } = new List<BrakeController>();
        public List<Steer> SteeringParts { get; set; } = new List<Steer>();
        public List<ChassisConcept> ChassisParts { get; set; } = new List<ChassisConcept>();
        public List<Lightweight> WeightReductionParts { get; set; } = new List<Lightweight>();
        public List<RacingModify> BodyParts { get; set; } = new List<RacingModify>();
        public List<EngineConcept> EngineParts { get; set; } = new List<EngineConcept>();
        public List<PortPolish> PortPolishingParts { get; set; } = new List<PortPolish>();
        public List<EngineBalance> EngineBalancingParts { get; set; } = new List<EngineBalance>();
        public List<Displacement> DisplacementIncreaseParts { get; set; } = new List<Displacement>();
        public List<Computer> ComputerParts { get; set; } = new List<Computer>();
        public List<NATune> NATuneParts { get; set; } = new List<NATune>();
        public List<TurbineKit> TurboKitParts { get; set; } = new List<TurbineKit>();
        public List<Drivetrain> DrivetrainParts { get; set; } = new List<Drivetrain>();
        public List<Flywheel> FlywheelParts { get; set; } = new List<Flywheel>();
        public List<Clutch> ClutchParts { get; set; } = new List<Clutch>();
        public List<PropellerShaft> PropellerShaftParts { get; set; } = new List<PropellerShaft>();
        public List<GearConcept> GearboxParts { get; set; } = new List<GearConcept>();
        public List<Suspension> SuspensionParts { get; set; } = new List<Suspension>();
        public List<Intercooler> IntercoolerParts { get; set; } = new List<Intercooler>();
        public List<Muffler> MufflerParts { get; set; } = new List<Muffler>();
        public List<LSD> LSDParts { get; set; } = new List<LSD>();
        public List<TCSCMaybe> TCSCParts { get; set; } = new List<TCSCMaybe>();
        public List<ASCCMaybe> ASCCParts { get; set; } = new List<ASCCMaybe>();
        public List<Wheels> WheelsParts { get; set; } = new List<Wheels>();
        public List<TireSize> TyreSizeParts { get; set; } = new List<TireSize>();
        public List<TireForceVol> TyreForceVolParts { get; set; } = new List<TireForceVol>();
        public List<TireCompound> TyreCompounds { get; set; } = new List<TireCompound>();
        public List<FrontTire> TyresFrontParts { get; set; } = new List<FrontTire>();
        public List<RearTire> TyresRearParts { get; set; } = new List<RearTire>();
        public List<EnemyCars> Opponents { get; set; } = new List<EnemyCars>();
        public List<Event> Events { get; set; } = new List<Event>();
        public List<Regulations> Regulations { get; set; } = new List<Regulations>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<ArcadeCar> ArcadeCars { get; set; } = new List<ArcadeCar>();
        public List<Car> Cars { get; set; } = new List<Car>();

        public void ReadData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] magic = new byte[4];
                file.Read(magic);
                if (Encoding.ASCII.GetString(magic) != "GTAR")
                {
                    Console.WriteLine("Not a GTAR archive.");
                    return;
                }

                uint tableCount = file.ReadUInt();
                uint dataStart = file.ReadUInt();
                uint unknown = file.ReadUInt();
                var dataTableStreams = new MemoryStream[tableCount];

                for (int i = 0; i < tableCount; i++)
                {
                    file.Position = (i * 4) + 0x10;
                    uint start = file.ReadUInt();
                    uint end = file.ReadUInt();
                    uint length = end - start;

                    file.Position = start + dataStart;
                    byte[] buffer = new byte[length];
                    file.Read(buffer);
                    dataTableStreams[i] = new MemoryStream(buffer);
                }

                BrakeParts.Read(dataTableStreams[0]);
                BrakeBalanceControllerParts.Read(dataTableStreams[1]);
                SteeringParts.Read(dataTableStreams[2]);
                ChassisParts.Read(dataTableStreams[3]);
                WeightReductionParts.Read(dataTableStreams[4]);
                BodyParts.Read(dataTableStreams[5]);
                EngineParts.Read(dataTableStreams[6]);
                PortPolishingParts.Read(dataTableStreams[7]);
                EngineBalancingParts.Read(dataTableStreams[8]);
                DisplacementIncreaseParts.Read(dataTableStreams[9]);
                ComputerParts.Read(dataTableStreams[10]);
                NATuneParts.Read(dataTableStreams[11]);
                TurboKitParts.Read(dataTableStreams[12]);
                DrivetrainParts.Read(dataTableStreams[13]);
                FlywheelParts.Read(dataTableStreams[14]);
                ClutchParts.Read(dataTableStreams[15]);
                PropellerShaftParts.Read(dataTableStreams[16]);
                GearboxParts.Read(dataTableStreams[17]);
                SuspensionParts.Read(dataTableStreams[18]);
                IntercoolerParts.Read(dataTableStreams[19]);
                MufflerParts.Read(dataTableStreams[20]);
                LSDParts.Read(dataTableStreams[21]);
                TCSCParts.Read(dataTableStreams[22]);
                ASCCParts.Read(dataTableStreams[23]);
                WheelsParts.Read(dataTableStreams[24]);
                TyreSizeParts.Read(dataTableStreams[25]);
                TyreForceVolParts.Read(dataTableStreams[26]);
                TyreCompounds.Read(dataTableStreams[27]);
                TyresFrontParts.Read(dataTableStreams[28]);
                TyresRearParts.Read(dataTableStreams[29]);
                Opponents.Read(dataTableStreams[30]);
                Events.Read(dataTableStreams[31]);
                Regulations.Read(dataTableStreams[32]);
                Courses.Read(dataTableStreams[33]);
                ArcadeCars.Read(dataTableStreams[34]);
                Cars.Read(dataTableStreams[35]);

                foreach (var stream in dataTableStreams)
                {
                    stream.Dispose();
                }
            }
        }

        public void DumpData()
        {
            BrakeParts.Dump();
            BrakeBalanceControllerParts.Dump();
            SteeringParts.Dump();
            ChassisParts.Dump();
            WeightReductionParts.Dump();
            BodyParts.Dump();
            EngineParts.Dump();
            PortPolishingParts.Dump();
            EngineBalancingParts.Dump();
            DisplacementIncreaseParts.Dump();
            ComputerParts.Dump();
            NATuneParts.Dump();
            TurboKitParts.Dump();
            DrivetrainParts.Dump();
            FlywheelParts.Dump();
            ClutchParts.Dump();
            PropellerShaftParts.Dump();
            GearboxParts.Dump();
            SuspensionParts.Dump();
            IntercoolerParts.Dump();
            MufflerParts.Dump();
            LSDParts.Dump();
            TCSCParts.Dump();
            ASCCParts.Dump();
            WheelsParts.Dump();
            TyreSizeParts.Dump();
            TyreForceVolParts.Dump();
            TyreCompounds.Dump();
            TyresFrontParts.Dump();
            TyresRearParts.Dump();
            Opponents.Dump();
            Events.Dump();
            Regulations.Dump();
            Courses.Dump();
            ArcadeCars.Dump();
            Cars.Dump();
        }

        public void ImportData()
        {
            BrakeParts.Import();
            BrakeBalanceControllerParts.Import();
            SteeringParts.Import();
            ChassisParts.Import();
            WeightReductionParts.Import();
            BodyParts.Import();
            EngineParts.Import();
            PortPolishingParts.Import();
            EngineBalancingParts.Import();
            DisplacementIncreaseParts.Import();
            ComputerParts.Import();
            NATuneParts.Import();
            TurboKitParts.Import();
            DrivetrainParts.Import();
            FlywheelParts.Import();
            ClutchParts.Import();
            PropellerShaftParts.Import();
            GearboxParts.Import();
            SuspensionParts.Import();
            IntercoolerParts.Import();
            MufflerParts.Import();
            LSDParts.Import();
            TCSCParts.Import();
            ASCCParts.Import();
            WheelsParts.Import();
            TyreSizeParts.Import();
            TyreForceVolParts.Import();
            TyreCompounds.Import();
            TyresFrontParts.Import();
            TyresRearParts.Import();
            Opponents.Import();
            Events.Import();
            Regulations.Import();
            Courses.Import();
            ArcadeCars.Import();
            Cars.Import();
        }

        public void WriteData()
        {
            using (FileStream file = new FileStream("paramdb_gtc_eu.db", FileMode.Create, FileAccess.ReadWrite))
            {
                const uint DataTableCount = 0x24;
                file.WriteCharacters("GTAR");
                file.WriteUInt(DataTableCount);
                uint dataStart = ((DataTableCount + 2) * 4) + 16;
                file.WriteUInt(dataStart);
                file.WriteUInt(0x07);
                file.Position = dataStart;
                ushort tableNumber = 0;
                BrakeParts.Write(file, dataStart, tableNumber++);
                BrakeBalanceControllerParts.Write(file, dataStart, tableNumber++);
                SteeringParts.Write(file, dataStart, tableNumber++);
                ChassisParts.Write(file, dataStart, tableNumber++);
                WeightReductionParts.Write(file, dataStart, tableNumber++);
                BodyParts.Write(file, dataStart, tableNumber++);
                EngineParts.Write(file, dataStart, tableNumber++);
                PortPolishingParts.Write(file, dataStart, tableNumber++);
                EngineBalancingParts.Write(file, dataStart, tableNumber++);
                DisplacementIncreaseParts.Write(file, dataStart, tableNumber++);
                ComputerParts.Write(file, dataStart, tableNumber++);
                NATuneParts.Write(file, dataStart, tableNumber++);
                TurboKitParts.Write(file, dataStart, tableNumber++);
                DrivetrainParts.Write(file, dataStart, tableNumber++);
                FlywheelParts.Write(file, dataStart, tableNumber++);
                ClutchParts.Write(file, dataStart, tableNumber++);
                PropellerShaftParts.Write(file, dataStart, tableNumber++);
                GearboxParts.Write(file, dataStart, tableNumber++);
                SuspensionParts.Write(file, dataStart, tableNumber++);
                IntercoolerParts.Write(file, dataStart, tableNumber++);
                MufflerParts.Write(file, dataStart, tableNumber++);
                LSDParts.Write(file, dataStart, tableNumber++);
                TCSCParts.Write(file, dataStart, tableNumber++);
                ASCCParts.Write(file, dataStart, tableNumber++);
                WheelsParts.Write(file, dataStart, tableNumber++);
                TyreSizeParts.Write(file, dataStart, tableNumber++);
                TyreForceVolParts.Write(file, dataStart, tableNumber++);
                TyreCompounds.Write(file, dataStart, tableNumber++);
                TyresFrontParts.Write(file, dataStart, tableNumber++);
                TyresRearParts.Write(file, dataStart, tableNumber++);
                Opponents.Write(file, dataStart, tableNumber++);
                Events.Write(file, dataStart, tableNumber++);
                Regulations.Write(file, dataStart, tableNumber++);
                Courses.Write(file, dataStart, tableNumber++);
                ArcadeCars.Write(file, dataStart, tableNumber++);
                Cars.Write(file, dataStart, tableNumber++);

                file.Position = dataStart - 8;
                file.WriteUInt((uint)file.Length - dataStart);
            }
        }
    }
}
