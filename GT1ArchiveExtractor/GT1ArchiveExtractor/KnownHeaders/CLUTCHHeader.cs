namespace GT2.GT1ArchiveExtractor
{
    public class CLUTCHHeader : KnownHeader
    {
        public CLUTCHHeader()
        {
            Extension = "clutch";
            Header = FromString("@(#)CLUTCH");
        }
    }
}