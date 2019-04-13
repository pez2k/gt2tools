namespace GT1.ArchiveExtractor
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