namespace GT2.GT1ArchiveExtractor
{
    public class STABILZHeader : KnownHeader
    {
        public STABILZHeader()
        {
            Extension = "stabilz";
            Header = FromString("@(#)STABILZ");
        }
    }
}