namespace GT2.GT1ArchiveExtractor
{
    public class LWEIGHTHeader : KnownHeader
    {
        public LWEIGHTHeader()
        {
            Extension = "lweight";
            Header = FromString("@(#)LWEIGHT");
        }
    }
}