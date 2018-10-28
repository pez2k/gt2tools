namespace GT2.GT1ArchiveExtractor
{
    public class BALANCEHeader : KnownHeader
    {
        public BALANCEHeader()
        {
            Extension = "balance";
            Header = FromString("@(#)BALANCE");
        }
    }
}