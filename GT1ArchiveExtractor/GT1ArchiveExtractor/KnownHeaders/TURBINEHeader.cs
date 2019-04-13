namespace GT1.ArchiveExtractor
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