namespace GT2.GT1ArchiveExtractor
{
    public class FLYWHELHeader : KnownHeader
    {
        public FLYWHELHeader()
        {
            Extension = "flywhel";
            Header = FromString("@(#)FLYWHEL");
        }
    }
}