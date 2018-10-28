namespace GT2.GT1ArchiveExtractor
{
    public class CARHeader : KnownHeader
    {
        public CARHeader()
        {
            Extension = "car";
            Header = FromString("@(#)GT-CAR");
        }
    }
}