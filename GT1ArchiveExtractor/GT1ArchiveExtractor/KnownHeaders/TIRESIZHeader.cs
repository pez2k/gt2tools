namespace GT2.GT1ArchiveExtractor
{
    public class TIRESIZHeader : KnownHeader
    {
        public TIRESIZHeader()
        {
            Extension = "tiresiz";
            Header = FromString("@(#)TIRESIZ");
        }
    }
}