namespace GT2.GT1ArchiveExtractor
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