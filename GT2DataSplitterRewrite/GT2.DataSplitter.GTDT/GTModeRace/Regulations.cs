using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.GTModeRace
{
    public class Regulations : MappedDataStructure<Regulations.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x80
        public struct Data
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] EligibleCarIds;
        }

        public Models.GTMode.Regulations MapToModel() =>
            new Models.GTMode.Regulations
            {
                EligibleCarIds = data.EligibleCarIds
            };
    }
}