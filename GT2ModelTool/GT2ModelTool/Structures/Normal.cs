using System.IO;
using System.Runtime.InteropServices;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class Normal
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Unknown = new byte[4];

        public void ReadFromCDO(Stream stream)
        {
            stream.Read(Unknown);
        }

        public void ReadFromCAR(Stream stream)
        {
            stream.Position += 0x08;
        }

        public void WriteToCDO(Stream stream)
        {
            stream.Write(Unknown);
        }
    }
}