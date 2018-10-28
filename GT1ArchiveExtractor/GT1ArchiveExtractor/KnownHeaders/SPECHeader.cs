namespace GT2.GT1ArchiveExtractor
{
    public class SPECHeader : KnownHeader
    {
        public SPECHeader()
        {
            Extension = "spec";
            Header = FromString("@(#)SPEC");
        }
    }
}