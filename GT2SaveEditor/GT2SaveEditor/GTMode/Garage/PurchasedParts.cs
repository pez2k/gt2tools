using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode.Garage
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

            ASC = IsBitSet(equippedParts1, 0);
            Brakes = IsBitSet(equippedParts1, 1);
            BrakeController = IsBitSet(equippedParts1, 2);
            ClutchSingle = IsBitSet(equippedParts1, 3);
            ClutchTwin = IsBitSet(equippedParts1, 4);
            ClutchTriple = IsBitSet(equippedParts1, 5);
            Computer = IsBitSet(equippedParts1, 6);
            Displacement = IsBitSet(equippedParts1, 7);
            Unused = IsBitSet(equippedParts2, 0);
            EngineBalancing = IsBitSet(equippedParts2, 1);
            FlywheelSports = IsBitSet(equippedParts2, 2);
            FlywheelSemiRacing = IsBitSet(equippedParts2, 3);
            FlywheelRacing = IsBitSet(equippedParts2, 4);
            GearClose = IsBitSet(equippedParts2, 5);
            GearSuperClose = IsBitSet(equippedParts2, 6);
            GearRacing = IsBitSet(equippedParts2, 7);
            IntercoolerSports = IsBitSet(equippedParts3, 0);
            IntercoolerRacing = IsBitSet(equippedParts3, 1);
            LightweightStage1 = IsBitSet(equippedParts3, 2);
            LightweightStage2 = IsBitSet(equippedParts3, 3);
            LightweightStage3 = IsBitSet(equippedParts3, 4);
            LSD1Way = IsBitSet(equippedParts3, 5);
            LSD2Way = IsBitSet(equippedParts3, 6);
            LSD15Way = IsBitSet(equippedParts3, 7);
            LSDRacing = IsBitSet(equippedParts4, 0);
            LSDActiveYawControl = IsBitSet(equippedParts4, 1);
            MufflerSports = IsBitSet(equippedParts4, 2);
            MufflerSemiRacing = IsBitSet(equippedParts4, 3);
            MufflerRacing = IsBitSet(equippedParts4, 4);
            NATuneStage1 = IsBitSet(equippedParts4, 5);
            NATuneStage2 = IsBitSet(equippedParts4, 6);
            NATuneStage3 = IsBitSet(equippedParts4, 7);
            PortPolish = IsBitSet(equippedParts5, 0);
            Propshaft = IsBitSet(equippedParts5, 1);
            RacingModify = IsBitSet(equippedParts5, 2);
            SuspensionSports = IsBitSet(equippedParts5, 3);
            SuspensionSemiRacing = IsBitSet(equippedParts5, 4);
            SuspensionRacing = IsBitSet(equippedParts5, 5);
            TCS = IsBitSet(equippedParts5, 6);
            TiresSports = IsBitSet(equippedParts5, 7);
            TiresRacingHard = IsBitSet(equippedParts6, 0);
            TiresRacingMedium = IsBitSet(equippedParts6, 1);
            TiresRacingSoft = IsBitSet(equippedParts6, 2);
            TiresRacingSuperSoft = IsBitSet(equippedParts6, 3);
            TiresSimulation = IsBitSet(equippedParts6, 4);
            TiresDirt = IsBitSet(equippedParts6, 5);
            TurbineKitStage1 = IsBitSet(equippedParts6, 6);
            TurbineKitStage2 = IsBitSet(equippedParts6, 7);
            TurbineKitStage3 = IsBitSet(equippedParts7, 0);
            TurbineKitStage4 = IsBitSet(equippedParts7, 1);
            file.Position += 0x1;
        }

        private static bool IsBitSet(byte value, byte bitPosition) => (value & 1 << bitPosition) > 0;

        public void WriteToSave(Stream file)
        {
            byte equippedParts1 = 0;
            byte equippedParts2 = 0;
            byte equippedParts3 = 0;
            byte equippedParts4 = 0;
            byte equippedParts5 = 0;
            byte equippedParts6 = 0;
            byte equippedParts7 = 0;

            equippedParts1 = SetBit(equippedParts1, 0, ASC);
            equippedParts1 = SetBit(equippedParts1, 1, Brakes);
            equippedParts1 = SetBit(equippedParts1, 2, BrakeController);
            equippedParts1 = SetBit(equippedParts1, 3, ClutchSingle);
            equippedParts1 = SetBit(equippedParts1, 4, ClutchTwin);
            equippedParts1 = SetBit(equippedParts1, 5, ClutchTriple);
            equippedParts1 = SetBit(equippedParts1, 6, Computer);
            equippedParts1 = SetBit(equippedParts1, 7, Displacement);
            equippedParts2 = SetBit(equippedParts2, 0, Unused);
            equippedParts2 = SetBit(equippedParts2, 1, EngineBalancing);
            equippedParts2 = SetBit(equippedParts2, 2, FlywheelSports);
            equippedParts2 = SetBit(equippedParts2, 3, FlywheelSemiRacing);
            equippedParts2 = SetBit(equippedParts2, 4, FlywheelRacing);
            equippedParts2 = SetBit(equippedParts2, 5, GearClose);
            equippedParts2 = SetBit(equippedParts2, 6, GearSuperClose);
            equippedParts2 = SetBit(equippedParts2, 7, GearRacing);
            equippedParts3 = SetBit(equippedParts3, 0, IntercoolerSports);
            equippedParts3 = SetBit(equippedParts3, 1, IntercoolerRacing);
            equippedParts3 = SetBit(equippedParts3, 2, LightweightStage1);
            equippedParts3 = SetBit(equippedParts3, 3, LightweightStage2);
            equippedParts3 = SetBit(equippedParts3, 4, LightweightStage3);
            equippedParts3 = SetBit(equippedParts3, 5, LSD1Way);
            equippedParts3 = SetBit(equippedParts3, 6, LSD2Way);
            equippedParts3 = SetBit(equippedParts3, 7, LSD15Way);
            equippedParts4 = SetBit(equippedParts4, 0, LSDRacing);
            equippedParts4 = SetBit(equippedParts4, 1, LSDActiveYawControl);
            equippedParts4 = SetBit(equippedParts4, 2, MufflerSports);
            equippedParts4 = SetBit(equippedParts4, 3, MufflerSemiRacing);
            equippedParts4 = SetBit(equippedParts4, 4, MufflerRacing);
            equippedParts4 = SetBit(equippedParts4, 5, NATuneStage1);
            equippedParts4 = SetBit(equippedParts4, 6, NATuneStage2);
            equippedParts4 = SetBit(equippedParts4, 7, NATuneStage3);
            equippedParts5 = SetBit(equippedParts5, 0, PortPolish);
            equippedParts5 = SetBit(equippedParts5, 1, Propshaft);
            equippedParts5 = SetBit(equippedParts5, 2, RacingModify);
            equippedParts5 = SetBit(equippedParts5, 3, SuspensionSports);
            equippedParts5 = SetBit(equippedParts5, 4, SuspensionSemiRacing);
            equippedParts5 = SetBit(equippedParts5, 5, SuspensionRacing);
            equippedParts5 = SetBit(equippedParts5, 6, TCS);
            equippedParts5 = SetBit(equippedParts5, 7, TiresSports);
            equippedParts6 = SetBit(equippedParts6, 0, TiresRacingHard);
            equippedParts6 = SetBit(equippedParts6, 1, TiresRacingMedium);
            equippedParts6 = SetBit(equippedParts6, 2, TiresRacingSoft);
            equippedParts6 = SetBit(equippedParts6, 3, TiresRacingSuperSoft);
            equippedParts6 = SetBit(equippedParts6, 4, TiresSimulation);
            equippedParts6 = SetBit(equippedParts6, 5, TiresDirt);
            equippedParts6 = SetBit(equippedParts6, 6, TurbineKitStage1);
            equippedParts6 = SetBit(equippedParts6, 7, TurbineKitStage2);
            equippedParts7 = SetBit(equippedParts7, 0, TurbineKitStage3);
            equippedParts7 = SetBit(equippedParts7, 1, TurbineKitStage4);

            file.WriteByte(equippedParts1);
            file.WriteByte(equippedParts2);
            file.WriteByte(equippedParts3);
            file.WriteByte(equippedParts4);
            file.WriteByte(equippedParts5);
            file.WriteByte(equippedParts6);
            file.WriteByte(equippedParts7);
            file.Position += 0x1;
        }

        private static byte SetBit(byte value, byte bitPosition, bool set) => set ? (byte)(value | 1 << bitPosition) : value;
    }
}