namespace GT1.ArchiveExtractor
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