namespace GT1.ArchiveExtractor
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