using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Garage
{
    public class PurchasedParts
    {
        public bool ASC { get; set; }
        public bool Brakes { get; set; }
        public bool BrakeController { get; set; }
        public bool ClutchSingle { get; set; }
        public bool ClutchTwin { get; set; }
        public bool ClutchTriple { get; set; }
        public bool Computer { get; set; }
        public bool Displacement { get; set; }
        public bool Unused { get; set; }
        public bool EngineBalancing { get; set; }
        public bool FlywheelSports { get; set; }
        public bool FlywheelSemiRacing { get; set; }
        public bool FlywheelRacing { get; set; }
        public bool GearClose { get; set; }
        public bool GearSuperClose { get; set; }
        public bool GearRacing { get; set; }
        public bool IntercoolerSports { get; set; }
        public bool IntercoolerRacing { get; set; }
        public bool LightweightStage1 { get; set; }
        public bool LightweightStage2 { get; set; }
        public bool LightweightStage3 { get; set; }
        public bool LSD1Way { get; set; }
        public bool LSD2Way { get; set; }
        public bool LSD15Way { get; set; }
        public bool LSDRacing { get; set; }
        public bool LSDActiveYawControl { get; set; }
        public bool MufflerSports { get; set; }
        public bool MufflerSemiRacing { get; set; }
        public bool MufflerRacing { get; set; }
        public bool NATuneStage1 { get; set; }
        public bool NATuneStage2 { get; set; }
        public bool NATuneStage3 { get; set; }
        public bool PortPolish { get; set; }
        public bool Propshaft { get; set; }
        public bool RacingModify { get; set; }
        public bool SuspensionSports { get; set; }
        public bool SuspensionSemiRacing { get; set; }
        public bool SuspensionRacing { get; set; }
        public bool TCS { get; set; }
        public bool TiresSports { get; set; }
        public bool TiresRacingHard { get; set; }
        public bool TiresRacingMedium { get; set; }
        public bool TiresRacingSoft { get; set; }
        public bool TiresRacingSuperSoft { get; set; }
        public bool TiresSimulation { get; set; }
        public bool TiresDirt { get; set; }
        public bool TurbineKitStage1 { get; set; }
        public bool TurbineKitStage2 { get; set; }
        public bool TurbineKitStage3 { get; set; }
        public bool TurbineKitStage4 { get; set; }

        public void ReadFromSave(Stream file)
        {
            byte equippedParts1 = file.ReadSingleByte();
            byte equippedParts2 = file.ReadSingleByte();
            byte equippedParts3 = file.ReadSingleByte();
            byte equippedParts4 = file.ReadSingleByte();
            byte equippedParts5 = file.ReadSingleByte();
            byte equippedParts6 = file.ReadSingleByte();
            byte equippedParts7 = file.ReadSingleByte();

            ASC = IsBitSet(equippedParts1, 0x01);
            Brakes = IsBitSet(equippedParts1, 0x02);
            BrakeController = IsBitSet(equippedParts1, 0x04);
            ClutchSingle = IsBitSet(equippedParts1, 0x08);
            ClutchTwin = IsBitSet(equippedParts1, 0x10);
            ClutchTriple = IsBitSet(equippedParts1, 0x20);
            Computer = IsBitSet(equippedParts1, 0x40);
            Displacement = IsBitSet(equippedParts1, 0x80);
            Unused = IsBitSet(equippedParts2, 0x01);
            EngineBalancing = IsBitSet(equippedParts2, 0x02);
            FlywheelSports = IsBitSet(equippedParts2, 0x04);
            FlywheelSemiRacing = IsBitSet(equippedParts2, 0x08);
            FlywheelRacing = IsBitSet(equippedParts2, 0x10);
            GearClose = IsBitSet(equippedParts2, 0x20);
            GearSuperClose = IsBitSet(equippedParts2, 0x40);
            GearRacing = IsBitSet(equippedParts2, 0x80);
            IntercoolerSports = IsBitSet(equippedParts3, 0x01);
            IntercoolerRacing = IsBitSet(equippedParts3, 0x02);
            LightweightStage1 = IsBitSet(equippedParts3, 0x04);
            LightweightStage2 = IsBitSet(equippedParts3, 0x08);
            LightweightStage3 = IsBitSet(equippedParts3, 0x10);
            LSD1Way = IsBitSet(equippedParts3, 0x20);
            LSD2Way = IsBitSet(equippedParts3, 0x40);
            LSD15Way = IsBitSet(equippedParts3, 0x80);
            LSDRacing = IsBitSet(equippedParts4, 0x01);
            LSDActiveYawControl = IsBitSet(equippedParts4, 0x02);
            MufflerSports = IsBitSet(equippedParts4, 0x04);
            MufflerSemiRacing = IsBitSet(equippedParts4, 0x08);
            MufflerRacing = IsBitSet(equippedParts4, 0x10);
            NATuneStage1 = IsBitSet(equippedParts4, 0x20);
            NATuneStage2 = IsBitSet(equippedParts4, 0x40);
            NATuneStage3 = IsBitSet(equippedParts4, 0x80);
            PortPolish = IsBitSet(equippedParts5, 0x01);
            Propshaft = IsBitSet(equippedParts5, 0x02);
            RacingModify = IsBitSet(equippedParts5, 0x04);
            SuspensionSports = IsBitSet(equippedParts5, 0x08);
            SuspensionSemiRacing = IsBitSet(equippedParts5, 0x10);
            SuspensionRacing = IsBitSet(equippedParts5, 0x20);
            TCS = IsBitSet(equippedParts5, 0x40);
            TiresSports = IsBitSet(equippedParts5, 0x80);
            TiresRacingHard = IsBitSet(equippedParts6, 0x01);
            TiresRacingMedium = IsBitSet(equippedParts6, 0x02);
            TiresRacingSoft = IsBitSet(equippedParts6, 0x04);
            TiresRacingSuperSoft = IsBitSet(equippedParts6, 0x08);
            TiresSimulation = IsBitSet(equippedParts6, 0x10);
            TiresDirt = IsBitSet(equippedParts6, 0x20);
            TurbineKitStage1 = IsBitSet(equippedParts6, 0x40);
            TurbineKitStage2 = IsBitSet(equippedParts6, 0x80);
            TurbineKitStage3 = IsBitSet(equippedParts7, 0x01);
            TurbineKitStage4 = IsBitSet(equippedParts7, 0x02);
            file.Position += 0x3;
        }

        private bool IsBitSet(byte value, byte bit) => (value & bit) > 0;
    }
}