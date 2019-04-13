namespace GT1.ArchiveExtractor
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