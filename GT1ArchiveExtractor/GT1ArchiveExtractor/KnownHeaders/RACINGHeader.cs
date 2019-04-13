namespace GT1.ArchiveExtractor
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