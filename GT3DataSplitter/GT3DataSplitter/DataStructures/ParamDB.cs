using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace GT3.DataSplitter
{
    using StreamExtensions;

    public class ParamDB
    {
        public List<Brakes> BrakeParts { get; set; } = new List<Brakes>();
        public List<BrakeBalanceController> BrakeBalanceControllerParts { get; set; } = new List<BrakeBalanceController>();
        public List<Steering> SteeringParts { get; set; } = new List<Steering>();
        public List<Chassis> ChassisParts { get; set; } = new List<Chassis>();
        public List<Lightweight> LightweightParts { get; set; } = new List<Lightweight>();
        public List<Body> BodyParts { get; set; } = new List<Body>();
        public List<Engine> EngineParts { get; set; } = new List<Engine>();
        public List<PortPolishing> PortPolishingParts { get; set; } = new List<PortPolishing>();
        public List<EngineBalancing> EngineBalancingParts { get; set; } = new List<EngineBalancing>();
        public List<DisplacementIncrease> DisplacementIncreaseParts { get; set; } = new List<DisplacementIncrease>();
        public List<Computer> ComputerParts { get; set; } = new List<Computer>();
        public List<NATune> NATuneParts { get; set; } = new List<NATune>();
        public List<TurboKit> TurboKitParts { get; set; } = new List<TurboKit>();
        public List<Drivetrain> DrivetrainParts { get; set; } = new List<Drivetrain>();
        public List<Flywheel> FlywheelParts { get; set; } = new List<Flywheel>();
        public List<Clutch> ClutchParts { get; set; } = new List<Clutch>();
        public List<Propshaft> PropshaftParts { get; set; } = new List<Propshaft>();
        public List<Gearbox> GearboxParts { get; set; } = new List<Gearbox>();
        public List<Suspension> SuspensionParts { get; set; } = new List<Suspension>();
        public List<Intercooler> IntercoolerParts { get; set; } = new List<Intercooler>();
        public List<Muffler> MufflerParts { get; set; } = new List<Muffler>();
        public List<LSD> LSDParts { get; set; } = new List<LSD>();
        public List<Unknown1> Unknown1Parts { get; set; } = new List<Unknown1>();
        public List<Unknown2> Unknown2Parts { get; set; } = new List<Unknown2>();
        public List<Wheels> WheelsParts { get; set; } = new List<Wheels>();
        public List<Unknown3> Unknown3Parts { get; set; } = new List<Unknown3>();
        public List<Unknown4> Unknown4Parts { get; set; } = new List<Unknown4>();
        public List<TyreCompound> TyreCompounds { get; set; } = new List<TyreCompound>();
        public List<TyresFront> TyresFrontParts { get; set; } = new List<TyresFront>();
        public List<TyresRear> TyresRearParts { get; set; } = new List<TyresRear>();
        public List<Car> Cars { get; set; } = new List<Car>();
        public List<Opponent> Opponents { get; set; } = new List<Opponent>();
        public List<Event> Events { get; set; } = new List<Event>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<ArcadeCar> ArcadeCars { get; set; } = new List<ArcadeCar>();
        public List<Regulations> Regulations { get; set; } = new List<Regulations>();

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
                LightweightParts.Read(dataTableStreams[4]);
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
                PropshaftParts.Read(dataTableStreams[16]);
                GearboxParts.Read(dataTableStreams[17]);
                SuspensionParts.Read(dataTableStreams[18]);
                IntercoolerParts.Read(dataTableStreams[19]);
                MufflerParts.Read(dataTableStreams[20]);
                LSDParts.Read(dataTableStreams[21]);
                Unknown1Parts.Read(dataTableStreams[22]); // VCD? ASC?
                Unknown2Parts.Read(dataTableStreams[23]);
                WheelsParts.Read(dataTableStreams[24]);
                Unknown3Parts.Read(dataTableStreams[25]); // sz?
                Unknown4Parts.Read(dataTableStreams[26]); // n?
                TyreCompounds.Read(dataTableStreams[27]);
                TyresFrontParts.Read(dataTableStreams[28]);
                TyresRearParts.Read(dataTableStreams[29]);
                Cars.Read(dataTableStreams[30]);
                Opponents.Read(dataTableStreams[31]);
                Events.Read(dataTableStreams[32]);
                Regulations.Read(dataTableStreams[33]);
                Courses.Read(dataTableStreams[34]);
                ArcadeCars.Read(dataTableStreams[35]);

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
            LightweightParts.Dump();
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
            PropshaftParts.Dump();
            GearboxParts.Dump();
            SuspensionParts.Dump();
            IntercoolerParts.Dump();
            MufflerParts.Dump();
            LSDParts.Dump();
            Unknown1Parts.Dump();
            Unknown2Parts.Dump();
            WheelsParts.Dump();
            Unknown3Parts.Dump();
            Unknown4Parts.Dump();
            TyreCompounds.Dump();
            TyresFrontParts.Dump();
            TyresRearParts.Dump();
            Cars.Dump();
            Opponents.Dump();
            Events.Dump();
            ArcadeCars.Dump();
            Courses.Dump();
            Regulations.Dump();
        }
    }
}
