namespace GT2.GT1ArchiveExtractor
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