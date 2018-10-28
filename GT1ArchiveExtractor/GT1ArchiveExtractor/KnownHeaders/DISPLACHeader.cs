namespace GT2.GT1ArchiveExtractor
{
    public class DISPLACHeader : KnownHeader
    {
        public DISPLACHeader()
        {
            Extension = "displac";
            Header = FromString("@(#)DISPLAC");
        }
    }
}