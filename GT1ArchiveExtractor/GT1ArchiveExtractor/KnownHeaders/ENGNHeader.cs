namespace GT2.GT1ArchiveExtractor
{
    public class ENGNHeader : KnownHeader
    {
        public ENGNHeader()
        {
            Extension = "es";
            Header = FromString("ENGN");
        }
    }
}