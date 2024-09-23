using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode.Garage
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
        public ushort Chassis { get; set; }
        public ushort Engine { get; set; }
        public ushort Drivetrain { get; set; }
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
        public ushort PowerMultiplier { get; set; }
        public short GearReverse { get; set; }
        public short Gear1 { get; set; }
        public short Gear2 { get; set; }
        public short Gear3 { get; set; }
        public short Gear4 { get; set; }
        public short Gear5 { get; set; }
        public short Gear6 { get; set; }
        public short Gear7 { get; set; }
        public ushort FinalDriveRatio { get; set; }
        public sbyte GearAutoSetting { get; set; }
        public sbyte TorqueSplit { get; set; }
        public byte BrakeLevelFront { get; set; }
        public byte BrakeLevelRear { get; set; }
        public byte DownforceFront { get; set; }
        public byte DownforceRear { get; set; }
        public uint TurboBoost1 { get; set; }
        public ushort TurboBoost2 { get; set; }
        public byte CamberFront { get; set; }
        public byte CamberRear { get; set; }
        public byte RideHeightFront { get; set; }
        public byte RideHeightRear { get; set; }
        public byte ToeFront { get; set; }
        public byte ToeRear { get; set; }
        public byte SpringRateFront { get; set; }
        public byte SpringRateRear { get; set; }
        public byte SpringFrequencyFront { get; set; }
        public byte SpringFrequencyRear { get; set; }
        public byte DamperBoundFront1 { get; set; }
        public byte DamperBoundFront2 { get; set; }
        public byte DamperReboundFront1 { get; set; }
        public byte DamperReboundFront2 { get; set; }
        public byte DamperBoundRear1 { get; set; }
        public byte DamperBoundRear2 { get; set; }
        public byte DamperReboundRear1 { get; set; }
        public byte DamperReboundRear2 { get; set; }
        public byte StabilizerFront { get; set; }
        public byte StabilizerRear { get; set; }
        public byte LSDInitialFront { get; set; }
        public byte LSDInitialRear { get; set; }
        public byte LSDAccelFront { get; set; }
        public byte LSDAccelRear { get; set; }
        public byte LSDDecelFront { get; set; }
        public byte LSDDecelRear { get; set; }
        public byte ASCLevel { get; set; }
        public byte TCSLevel { get; set; }
        public ushort EngineSoundID { get; set; }
        public EngineSoundVariantEnum EngineSoundVariant { get; set; } // Reflects which specific sound file will be used, based on aspiration and muffler upgrade level - not always up to date, possibly only set properly by pre-race parts menu
        public byte Manufacturer { get; set; }
        public byte Aspiration { get; set; } // 0xC0 for non-turbo including MA, 0xC2 for turbo - might actually be flags but with the first two set for every car?
        public byte GearAutoSettingAtLastGearCalculation { get; set; } // The auto setting value at the last point the slider was used (i.e. what the ratios were calculated from)
        public ushort FinalDriveRatioAtLastGearCalculation { get; set; } // The final drive ratio value at the last point the auto setting slider was used (i.e. what the ratios were calculated from)
        public TireUpgradeLevelEnum TiresFrontUpgradeLevelAtLastGearCalculation { get; set; } // The front tire upgrade level at the last point the auto setting slider was used (i.e. what the ratios were calculated from, presumably looking up wheel diameter)
        public TireUpgradeLevelEnum TiresRearUpgradeLevelAtLastGearCalculation { get; set; } // The rear tire upgrade level at the last point the auto setting slider was used (i.e. what the ratios were calculated from, presumably looking up wheel diameter)
        public string BodyFilename { get; set; } = "";
        public uint CarValue { get; set; }
        public ushort DisplayedWeight { get; set; }
        public ushort DisplayedTorque { get; set; }
        public ushort DisplayedPower { get; set; }
        public bool HasCVTBehaviour { get; set; }
        public bool RacingModified { get; set; }
        public PurchasedParts PurchasedParts { get; set; } = new();
        public ushort Dirtiness { get; set; }

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
            Chassis = file.ReadUShort();
            Engine = file.ReadUShort();
            Drivetrain = file.ReadUShort();
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
            PowerMultiplier = file.ReadUShort();
            GearReverse = file.ReadShortFixed();
            Gear1 = file.ReadShortFixed();
            Gear2 = file.ReadShortFixed();
            Gear3 = file.ReadShortFixed();
            Gear4 = file.ReadShortFixed();
            Gear5 = file.ReadShortFixed();
            Gear6 = file.ReadShortFixed();
            Gear7 = file.ReadShortFixed();
            FinalDriveRatio = file.ReadUShort();
            GearAutoSetting = file.ReadSByte();
            TorqueSplit = file.ReadSByte();
            BrakeLevelFront = file.ReadSingleByte();
            BrakeLevelRear = file.ReadSingleByte();
            DownforceFront = file.ReadSingleByte();
            DownforceRear = file.ReadSingleByte();
            TurboBoost1 = file.ReadUInt();
            TurboBoost2 = file.ReadUShort();
            CamberFront = file.ReadSingleByte();
            CamberRear = file.ReadSingleByte();
            RideHeightFront = file.ReadSingleByte();
            RideHeightRear = file.ReadSingleByte();
            ToeFront = file.ReadSingleByte();
            ToeRear = file.ReadSingleByte();
            SpringRateFront = file.ReadSingleByte();
            SpringRateRear = file.ReadSingleByte();
            SpringFrequencyFront = file.ReadSingleByte();
            SpringFrequencyRear = file.ReadSingleByte();
            DamperBoundFront1 = file.ReadSingleByte();
            DamperBoundFront2 = file.ReadSingleByte();
            DamperReboundFront1 = file.ReadSingleByte();
            DamperReboundFront2 = file.ReadSingleByte();
            DamperBoundRear1 = file.ReadSingleByte();
            DamperBoundRear2 = file.ReadSingleByte();
            DamperReboundRear1 = file.ReadSingleByte();
            DamperReboundRear2 = file.ReadSingleByte();
            StabilizerFront = file.ReadSingleByte();
            StabilizerRear = file.ReadSingleByte();
            LSDInitialFront = file.ReadSingleByte();
            LSDInitialRear = file.ReadSingleByte();
            LSDAccelFront = file.ReadSingleByte();
            LSDAccelRear = file.ReadSingleByte();
            LSDDecelFront = file.ReadSingleByte();
            LSDDecelRear = file.ReadSingleByte();
            ASCLevel = file.ReadSingleByte();
            TCSLevel = file.ReadSingleByte();
            EngineSoundID = file.ReadUShort();
            EngineSoundVariant = (EngineSoundVariantEnum)file.ReadSingleByte();
            Manufacturer = file.ReadSingleByte();
            Aspiration = file.ReadSingleByte();
            GearAutoSettingAtLastGearCalculation = file.ReadSingleByte();
            FinalDriveRatioAtLastGearCalculation = file.ReadUShort();
            TiresFrontUpgradeLevelAtLastGearCalculation = (TireUpgradeLevelEnum)file.ReadUShort();
            TiresRearUpgradeLevelAtLastGearCalculation = (TireUpgradeLevelEnum)file.ReadUShort();
            file.Position += 0x2;
            BodyFilename = file.ReadUInt().ToCarName();
            CarValue = file.ReadUInt();
            DisplayedWeight = file.ReadUShort();
            DisplayedTorque = file.ReadUShort();
            ushort displayedPowerAndFlags = file.ReadUShort();
            DisplayedPower = (ushort)(displayedPowerAndFlags & 0x3FFF);
            HasCVTBehaviour = (displayedPowerAndFlags & 0x4000) > 0;
            RacingModified = (displayedPowerAndFlags & 0x8000) > 0;
            PurchasedParts.ReadFromSave(file);
            Dirtiness = file.ReadUShort();
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
            file.WriteUShort(Chassis);
            file.WriteUShort(Engine);
            file.WriteUShort(Drivetrain);
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
            file.WriteUShort(PowerMultiplier);
            file.WriteShortFixed(GearReverse);
            file.WriteShortFixed(Gear1);
            file.WriteShortFixed(Gear2);
            file.WriteShortFixed(Gear3);
            file.WriteShortFixed(Gear4);
            file.WriteShortFixed(Gear5);
            file.WriteShortFixed(Gear6);
            file.WriteShortFixed(Gear7);
            file.WriteUShort(FinalDriveRatio);
            file.WriteSByte(GearAutoSetting);
            file.WriteSByte(TorqueSplit);
            file.WriteByte(BrakeLevelFront);
            file.WriteByte(BrakeLevelRear);
            file.WriteByte(DownforceFront);
            file.WriteByte(DownforceRear);
            file.WriteUInt(TurboBoost1);
            file.WriteUShort(TurboBoost2);
            file.WriteByte(CamberFront);
            file.WriteByte(CamberRear);
            file.WriteByte(RideHeightFront);
            file.WriteByte(RideHeightRear);
            file.WriteByte(ToeFront);
            file.WriteByte(ToeRear);
            file.WriteByte(SpringRateFront);
            file.WriteByte(SpringRateRear);
            file.WriteByte(SpringFrequencyFront);
            file.WriteByte(SpringFrequencyRear);
            file.WriteByte(DamperBoundFront1);
            file.WriteByte(DamperBoundFront2);
            file.WriteByte(DamperReboundFront1);
            file.WriteByte(DamperReboundFront2);
            file.WriteByte(DamperBoundRear1);
            file.WriteByte(DamperBoundRear2);
            file.WriteByte(DamperReboundRear1);
            file.WriteByte(DamperReboundRear2);
            file.WriteByte(StabilizerFront);
            file.WriteByte(StabilizerRear);
            file.WriteByte(LSDInitialFront);
            file.WriteByte(LSDInitialRear);
            file.WriteByte(LSDAccelFront);
            file.WriteByte(LSDAccelRear);
            file.WriteByte(LSDDecelFront);
            file.WriteByte(LSDDecelRear);
            file.WriteByte(ASCLevel);
            file.WriteByte(TCSLevel);
            file.WriteUShort(EngineSoundID);
            file.WriteByte((byte)EngineSoundVariant);
            file.WriteByte(Manufacturer);
            file.WriteByte(Aspiration);
            file.WriteByte(GearAutoSettingAtLastGearCalculation);
            file.WriteUShort(FinalDriveRatioAtLastGearCalculation);
            file.WriteUShort((ushort)TiresFrontUpgradeLevelAtLastGearCalculation);
            file.WriteUShort((ushort)TiresRearUpgradeLevelAtLastGearCalculation);
            file.Position += 0x2;
            file.WriteUInt(BodyFilename.ToCarID());
            file.WriteUInt(CarValue);
            file.WriteUShort(DisplayedWeight);
            file.WriteUShort(DisplayedTorque);
            file.WriteUShort((ushort)(DisplayedPower | (HasCVTBehaviour ? 0x4000 : 0) | (RacingModified ? 0x8000 : 0)));
            PurchasedParts.WriteToSave(file);
            file.WriteUShort(Dirtiness);
        }
    }
}