namespace GT1.ArchiveExtractor
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