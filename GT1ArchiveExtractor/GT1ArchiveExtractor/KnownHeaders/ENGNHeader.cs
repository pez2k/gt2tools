namespace GT1.ArchiveExtractor
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