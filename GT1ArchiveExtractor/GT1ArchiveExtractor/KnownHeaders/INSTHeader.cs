namespace GT2.GT1ArchiveExtractor
{
    public class INSTHeader : KnownHeader
    {
        public INSTHeader()
        {
            Extension = "ins";
            Header = FromString("INST");
        }
    }
}