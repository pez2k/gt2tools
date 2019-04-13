namespace GT1.ArchiveExtractor
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