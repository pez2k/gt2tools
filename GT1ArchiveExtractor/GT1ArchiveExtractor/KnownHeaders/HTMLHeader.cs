namespace GT1.ArchiveExtractor
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