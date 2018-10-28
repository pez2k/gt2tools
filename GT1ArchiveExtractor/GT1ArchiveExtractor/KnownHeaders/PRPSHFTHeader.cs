namespace GT2.GT1ArchiveExtractor
{
    public class PRPSHFTHeader : KnownHeader
    {
        public PRPSHFTHeader()
        {
            Extension = "prpshft";
            Header = FromString("@(#)PRPSHFT");
        }
    }
}