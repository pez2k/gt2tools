namespace GT1.ArchiveExtractor
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