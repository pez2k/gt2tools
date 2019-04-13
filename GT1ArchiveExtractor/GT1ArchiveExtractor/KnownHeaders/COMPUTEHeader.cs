namespace GT1.ArchiveExtractor
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