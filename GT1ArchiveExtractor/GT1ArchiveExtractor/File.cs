using System.Linq;

namespace GT2.GT1ArchiveExtractor
{
    public class FileData
    {
        public string Name;
        public bool Compressed;
        public byte[] Contents;

        private static readonly byte[] ARCHeader = new byte[] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x2D, 0x41, 0x52, 0x43 };
        private static readonly byte[] TEXHeader = new byte[] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x2D, 0x43, 0x54, 0x45, 0x58 };
        private static readonly byte[] CARHeader = new byte[] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x2D, 0x43, 0x41, 0x52 };
        private static readonly byte[] INSTHeader = new byte[] { 0x49, 0x4E, 0x53, 0x54 };
        private static readonly byte[] ENGNHeader = new byte[] { 0x45, 0x4E, 0x47, 0x4E };
        private static readonly byte[] TIMHeader = new byte[] { 0x10, 0x00, 0x00, 0x00 };
        private static readonly byte[] SKYHeader = new byte[] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x2D, 0x53, 0x4B, 0x59 };
        private static readonly byte[] PSHeader = new byte[] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x2D, 0x50, 0x53 };
        private static readonly byte[] SEQGHeader = new byte[] { 0x53, 0x45, 0x51, 0x47 };
        private static readonly byte[] USEDCARHeader = new byte[] { 0x40, 0x28, 0x23, 0x29, 0x55, 0x53, 0x45, 0x44, 0x43, 0x41, 0x52 };
        private static readonly byte[] HTMLHeader = new byte[] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x48, 0x54, 0x4D, 0x4C };

        private bool HeaderMatches(byte[] header) => Contents.Take(header.Length).SequenceEqual(header);

        public bool IsArchive() => HeaderMatches(ARCHeader);

        public bool IsModel() => HeaderMatches(CARHeader);

        public bool IsTexture() => HeaderMatches(TEXHeader);

        public bool IsInstrument() => HeaderMatches(INSTHeader);

        public bool IsEngineSound() => HeaderMatches(ENGNHeader);

        public bool IsTIM() => HeaderMatches(TIMHeader);

        public bool IsSky() => HeaderMatches(SKYHeader);

        public bool IsCourse() => HeaderMatches(PSHeader);

        public bool IsSEQG() => HeaderMatches(SEQGHeader);

        public bool IsUsedCar() => HeaderMatches(USEDCARHeader);

        public bool IsHTML() => HeaderMatches(HTMLHeader);
    }
}