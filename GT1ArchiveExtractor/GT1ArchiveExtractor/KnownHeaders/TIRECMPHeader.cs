namespace GT2.GT1ArchiveExtractor
{
    public class TIRECMPHeader : KnownHeader
    {
        public TIRECMPHeader()
        {
            Extension = "tirecmp";
            Header = FromString("@(#)TIRECMP");
        }
    }
}