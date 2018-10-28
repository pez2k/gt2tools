namespace GT2.GT1ArchiveExtractor
{
    public class COMPUTEHeader : KnownHeader
    {
        public COMPUTEHeader()
        {
            Extension = "compute";
            Header = FromString("@(#)COMPUTE");
        }
    }
}