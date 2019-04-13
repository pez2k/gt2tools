namespace GT1.ArchiveExtractor
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