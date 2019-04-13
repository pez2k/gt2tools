namespace GT1.ArchiveExtractor
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