namespace GT1.ArchiveExtractor
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