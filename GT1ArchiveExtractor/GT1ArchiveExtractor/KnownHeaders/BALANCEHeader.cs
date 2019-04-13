namespace GT1.ArchiveExtractor
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