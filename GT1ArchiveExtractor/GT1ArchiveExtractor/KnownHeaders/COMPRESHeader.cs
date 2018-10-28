namespace GT2.GT1ArchiveExtractor
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