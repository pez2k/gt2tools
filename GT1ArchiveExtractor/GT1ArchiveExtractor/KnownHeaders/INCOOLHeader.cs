namespace GT1.ArchiveExtractor
{
    public class INCOOLHeader : KnownHeader
    {
        public INCOOLHeader()
        {
            Extension = "incool";
            Header = FromString("@(#)INCOOL");
        }
    }
}