namespace GT1.ArchiveExtractor
{
    public class SUSPENSHeader : KnownHeader
    {
        public SUSPENSHeader()
        {
            Extension = "suspens";
            Header = FromString("@(#)SUSPENS");
        }
    }
}