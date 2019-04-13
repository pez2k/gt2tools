namespace GT1.ArchiveExtractor
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