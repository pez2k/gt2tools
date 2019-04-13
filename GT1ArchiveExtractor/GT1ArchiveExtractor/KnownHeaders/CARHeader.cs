namespace GT1.ArchiveExtractor
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