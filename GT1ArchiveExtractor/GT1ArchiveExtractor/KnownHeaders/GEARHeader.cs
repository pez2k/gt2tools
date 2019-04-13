namespace GT1.ArchiveExtractor
{
    public class GEARHeader : KnownHeader
    {
        public GEARHeader()
        {
            Extension = "gear";
            Header = FromString("@(#)GEAR");
        }
    }
}