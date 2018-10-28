namespace GT2.GT1ArchiveExtractor
{
    public class COLORHeader : KnownHeader
    {
        public COLORHeader()
        {
            Extension = "color";
            Header = FromString("@(#)COLOR");
        }
    }
}