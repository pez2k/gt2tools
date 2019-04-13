namespace GT1.ArchiveExtractor
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