namespace GT2.GT1ArchiveExtractor
{
    public class EQUIPHeader : KnownHeader
    {
        public EQUIPHeader()
        {
            Extension = "equip";
            Header = FromString("@(#)EQUIP");
        }
    }
}