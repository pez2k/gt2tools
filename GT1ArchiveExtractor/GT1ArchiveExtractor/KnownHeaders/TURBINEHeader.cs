namespace GT2.GT1ArchiveExtractor
{
    public class TURBINEHeader : KnownHeader
    {
        public TURBINEHeader()
        {
            Extension = "turbine";
            Header = FromString("@(#)TURBINE");
        }
    }
}