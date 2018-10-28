namespace GT2.GT1ArchiveExtractor
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