namespace GT2.GT1ArchiveExtractor
{
    public class ARCHeader : KnownHeader
    {
        public ARCHeader()
        {
            Extension = "arc";
            Header = FromString("@(#)GT-ARC");
        }
    }
}