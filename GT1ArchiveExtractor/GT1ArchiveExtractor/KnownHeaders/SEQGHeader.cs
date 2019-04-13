namespace GT1.ArchiveExtractor
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