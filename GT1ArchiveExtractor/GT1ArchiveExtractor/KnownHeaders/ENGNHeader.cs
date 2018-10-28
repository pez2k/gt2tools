namespace GT2.GT1ArchiveExtractor
{
    public class ENGNHeader : KnownHeader
    {
        public ENGNHeader()
        {
            Extension = "engn";
            Header = FromString("ENGN");
        }
    }
}