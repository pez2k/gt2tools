using System.IO;

namespace GT2.DataSplitter
{
    public class TireCompound : DataStructure
    {
        protected string[] tireCompoundNames =
            new string[] { "RoadFront", "RoadRear", "SportsFront", "SportsRear", "HardFront", "HardRear", "MediumFront", "MediumRear",
                           "SoftFront", "SoftRear", "SuperSoftFront", "SuperSoftRear", "DirtFront", "DirtRear", "RWDDirtFront", "RWDDirtRear",
                           "SimulationFront", "SimulationRear", "PikesPeakFront", "PikesPeakRear" };

        public TireCompound() => Size = 0x40;

        protected override string CreateOutputFilename()
        {
            byte number = (byte)Directory.GetFiles(Name).Length;
            string filename = base.CreateOutputFilename();
            string compoundName = number >= tireCompoundNames.Length ? "Unknown" : tireCompoundNames[number];
            return Path.Combine(Path.GetDirectoryName(filename), $"{number:D2}_{compoundName}{Path.GetExtension(filename)}");
        }
    }
}