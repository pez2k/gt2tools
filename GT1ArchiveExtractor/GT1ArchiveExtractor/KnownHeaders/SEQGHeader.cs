namespace GT2.GT1ArchiveExtractor
{
    public class SEQGHeader : KnownHeader
    {
        public SEQGHeader()
        {
            Extension = "seq";
            Header = FromString("SEQG");
        }
    }
}