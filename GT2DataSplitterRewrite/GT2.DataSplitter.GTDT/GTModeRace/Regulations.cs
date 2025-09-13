using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.GTModeRace
{
    public class Regulations : MappedDataStructure<Regulations.Data, Models.GTMode.Regulations>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x80
        public struct Data
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] EligibleCarIds;
        }

        public override Models.GTMode.Regulations MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.GTMode.Regulations
            {
                EligibleCarIds = data.EligibleCarIds
            };
    }
}