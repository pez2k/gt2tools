using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Garage
{
    using CarNameConversion;

    public class GarageCar
    {
        public string Name { get; set; } = "";
        public byte Colour { get; set; }
        public string WheelFilename { get; set; } = "";
        public ushort Brakes { get; set; }
        public ushort BrakeController { get; set; }
        public ushort Steer { get; set; }
        public ushort Gear { get; set; }
        public ushort Suspension { get; set; }
        public ushort LSD { get; set; }
        public ushort TiresFront { get; set; }
        public ushort TiresRear { get; set; }
        public ushort Lightweight { get; set; }
        public ushort RacingModify { get; set; }
        public ushort PortPolish { get; set; }
        public ushort EngineBalancing { get; set; }
        public ushort Displacement { get; set; }
        public ushort Computer { get; set; }
        public ushort NATune { get; set; }
        public ushort TurbineKit { get; set; }
        public ushort Flywheel { get; set; }
        public ushort Clutch { get; set; }
        public ushort Propshaft { get; set; }
        public ushort Muffler { get; set; }
        public ushort Intercooler { get; set; }
        public ushort ASC { get; set; }
        public ushort TCS { get; set; }
        public ushort Wheels { get; set; }
        public string BodyFilename { get; set; } = "";
        public uint CarValue { get; set; }
        public ushort DisplayedWeight { get; set; }
        public ushort DisplayedTorque { get; set; }
        public ushort DisplayedPower { get; set; }
        public bool RacingModified { get; set; }
        public PurchasedParts PurchasedParts { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            Name = file.ReadUInt().ToCarName();
            Colour = file.ReadSingleByte();
            file.Position += 0x3;
            uint wheelFilenameHash = file.ReadUInt();
            WheelFilename = wheelFilenameHash == 256 ? "" : WheelFilenameConverter.ConvertToString(wheelFilenameHash);
            Brakes = file.ReadUShort();
            BrakeController = file.ReadUShort();
            Steer = file.ReadUShort();
            file.Position += 0x6;
            Gear = file.ReadUShort();
            Suspension = file.ReadUShort();
            LSD = file.ReadUShort();
            TiresFront = file.ReadUShort();
            TiresRear = file.ReadUShort();
            Lightweight = file.ReadUShort();
            RacingModify = file.ReadUShort();
            PortPolish = file.ReadUShort();
            EngineBalancing = file.ReadUShort();
            Displacement = file.ReadUShort();
            Computer = file.ReadUShort();
            NATune = file.ReadUShort();
            TurbineKit = file.ReadUShort();
            Flywheel = file.ReadUShort();
            Clutch = file.ReadUShort();
            Propshaft = file.ReadUShort();
            Muffler = file.ReadUShort();
            Intercooler = file.ReadUShort();
            ASC = file.ReadUShort();
            TCS = file.ReadUShort();
            Wheels = file.ReadUShort();

            file.Position += 0x4A; // Skip over the car settings for now

            BodyFilename = file.ReadUInt().ToCarName();
            CarValue = file.ReadUInt();
            DisplayedWeight = file.ReadUShort();
            DisplayedTorque = file.ReadUShort();
            ushort powerAndRMFlag = file.ReadUShort();
            DisplayedPower = (ushort)(powerAndRMFlag & 0x7FFF);
            RacingModified = (powerAndRMFlag & 0x8000) > 0;
            PurchasedParts.ReadFromSave(file);
        }

        public void WriteToSave(Stream file)
        {
            file.WriteUInt(Name.ToCarID());
            file.WriteByte(Colour);
            file.Position += 0x3;
            file.WriteUInt(WheelFilenameConverter.ConvertFromString(WheelFilename));
            file.WriteUShort(Brakes);
            file.WriteUShort(BrakeController);
            file.WriteUShort(Steer);
            file.Position += 0x6;
            file.WriteUShort(Gear);
            file.WriteUShort(Suspension);
            file.WriteUShort(LSD);
            file.WriteUShort(TiresFront);
            file.WriteUShort(TiresRear);
            file.WriteUShort(Lightweight);
            file.WriteUShort(RacingModify);
            file.WriteUShort(PortPolish);
            file.WriteUShort(EngineBalancing);
            file.WriteUShort(Displacement);
            file.WriteUShort(Computer);
            file.WriteUShort(NATune);
            file.WriteUShort(TurbineKit);
            file.WriteUShort(Flywheel);
            file.WriteUShort(Clutch);
            file.WriteUShort(Propshaft);
            file.WriteUShort(Muffler);
            file.WriteUShort(Intercooler);
            file.WriteUShort(ASC);
            file.WriteUShort(TCS);
            file.WriteUShort(Wheels);

            file.Position += 0x4A; // Skip over the car settings for now

            file.WriteUInt(BodyFilename.ToCarID());
            file.WriteUInt(CarValue);
            file.WriteUShort(DisplayedWeight);
            file.WriteUShort(DisplayedTorque);
            file.WriteUShort((ushort)(DisplayedPower | (RacingModified ? 0x8000 : 0)));
            PurchasedParts.WriteToSave(file);
        }
    }
}