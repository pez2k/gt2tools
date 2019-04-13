namespace GT1.ArchiveExtractor
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