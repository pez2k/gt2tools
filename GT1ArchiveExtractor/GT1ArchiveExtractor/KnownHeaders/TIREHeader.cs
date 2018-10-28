namespace GT2.GT1ArchiveExtractor
{
    public class TIREHeader : KnownHeader
    {
        public TIREHeader()
        {
            Extension = "tire";
            Header = FromString("@(#)TIRE");
        }
    }
}