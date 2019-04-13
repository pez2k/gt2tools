namespace GT1.ArchiveExtractor
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