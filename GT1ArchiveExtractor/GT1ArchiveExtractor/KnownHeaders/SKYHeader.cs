namespace GT1.ArchiveExtractor
{
    public class SKYHeader : KnownHeader
    {
        public SKYHeader()
        {
            Extension = "sky";
            Header = FromString("@(#)GT-SKY");
        }
    }
}