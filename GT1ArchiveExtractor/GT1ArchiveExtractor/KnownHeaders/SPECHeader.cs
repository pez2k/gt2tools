namespace GT1.ArchiveExtractor
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