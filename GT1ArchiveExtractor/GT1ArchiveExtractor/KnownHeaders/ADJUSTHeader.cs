namespace GT1.ArchiveExtractor
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