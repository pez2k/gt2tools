namespace GT2.GT1ArchiveExtractor
{
    public class HTMLHeader : KnownHeader
    {
        public HTMLHeader()
        {
            Extension = "gthtml";
            Header = FromString("@(#)GTHTML");
        }
    }
}