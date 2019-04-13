namespace GT1.ArchiveExtractor
{
    public class COMPRESHeader : KnownHeader
    {
        public COMPRESHeader()
        {
            Extension = "compres";
            Header = FromString("@(#)COMPRES");
        }
    }
}