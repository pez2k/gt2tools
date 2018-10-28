namespace GT2.GT1ArchiveExtractor
{
    public class MUFFLERHeader : KnownHeader
    {
        public MUFFLERHeader()
        {
            Extension = "muffler";
            Header = FromString("@(#)MUFFLER");
        }
    }
}