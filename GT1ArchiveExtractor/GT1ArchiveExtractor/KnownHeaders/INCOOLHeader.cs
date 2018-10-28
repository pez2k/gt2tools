namespace GT2.GT1ArchiveExtractor
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