namespace GT2.GT1ArchiveExtractor
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