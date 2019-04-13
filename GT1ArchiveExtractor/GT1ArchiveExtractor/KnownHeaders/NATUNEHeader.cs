namespace GT1.ArchiveExtractor
{
    public class NATUNEHeader : KnownHeader
    {
        public NATUNEHeader()
        {
            Extension = "natune";
            Header = FromString("@(#)NATUNE");
        }
    }
}