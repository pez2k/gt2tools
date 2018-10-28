namespace GT2.GT1ArchiveExtractor
{
    public class TEXHeader : KnownHeader
    {
        public TEXHeader()
        {
            Extension = "tex";
            Header = FromString("@(#)GT-CTEX");
        }
    }
}