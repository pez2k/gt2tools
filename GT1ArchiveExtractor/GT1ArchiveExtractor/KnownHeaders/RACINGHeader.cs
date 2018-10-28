namespace GT2.GT1ArchiveExtractor
{
    public class RACINGHeader : KnownHeader
    {
        public RACINGHeader()
        {
            Extension = "racing";
            Header = FromString("@(#)RACING");
        }
    }
}