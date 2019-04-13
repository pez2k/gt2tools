namespace GT1.ArchiveExtractor
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