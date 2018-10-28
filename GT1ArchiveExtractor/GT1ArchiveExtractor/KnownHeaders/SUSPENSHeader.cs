namespace GT2.GT1ArchiveExtractor
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