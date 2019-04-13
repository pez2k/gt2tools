namespace GT1.ArchiveExtractor
{
    public class POLISHHeader : KnownHeader
    {
        public POLISHHeader()
        {
            Extension = "polish";
            Header = FromString("@(#)POLISH");
        }
    }
}