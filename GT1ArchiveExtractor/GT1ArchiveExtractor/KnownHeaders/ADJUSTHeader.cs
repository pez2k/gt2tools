namespace GT2.GT1ArchiveExtractor
{
    public class ADJUSTHeader : KnownHeader
    {
        public ADJUSTHeader()
        {
            Extension = "adjust";
            Header = FromString("@(#)ADJUST");
        }
    }
}