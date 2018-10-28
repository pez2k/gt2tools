namespace GT2.GT1ArchiveExtractor
{
    public class BRAKEHeader : KnownHeader
    {
        public BRAKEHeader()
        {
            Extension = "brake";
            Header = FromString("@(#)BRAKE");
        }
    }
}