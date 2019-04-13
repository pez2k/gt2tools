namespace GT1.ArchiveExtractor
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