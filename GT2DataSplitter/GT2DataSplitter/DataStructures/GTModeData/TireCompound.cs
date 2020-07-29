using System.Collections.Generic;
using System.IO;

namespace GT2.DataSplitter
{
    public class TireCompound : DataStructure
    {
        private readonly List<string> tireCompoundNames = new List<string> { "RoadFront", "RoadRear", "SportsFront", "SportsRear", "HardFront", "HardRear", "MediumFront", "MediumRear", "SoftFront", "SoftRear", "SuperSoftFront", "SuperSoftRear", "DirtFront", "DirtRear", "RWDDirtFront", "RWDDirtRear", "SimulationFront", "SimulationRear", "PikesPeakFront", "PikesPeakRear" };

        public TireCompound()
        {
            Size = 0x40;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            byte number = (byte)Directory.GetFiles(Name).Length;
            string filename = base.CreateOutputFilename(data);
            return Path.Combine(Path.GetDirectoryName(filename),
                                $"{number:D2}_{tireCompoundNames[number]}" +
                                $"{Path.GetExtension(filename)}");
        }
    }
}
