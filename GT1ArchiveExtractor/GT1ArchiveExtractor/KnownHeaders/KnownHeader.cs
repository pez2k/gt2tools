using System.Text;

namespace GT2.GT1ArchiveExtractor
{
    public abstract class KnownHeader
    {
        public string Extension;
        public byte[] Header;

        protected byte[] FromString(string text) => Encoding.ASCII.GetBytes(text);
    }
}