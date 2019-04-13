using System.Text;

namespace GT1.ArchiveExtractor
{
    public abstract class KnownHeader
    {
        public string Extension;
        public byte[] Header;

        protected byte[] FromString(string text) => Encoding.ASCII.GetBytes(text);
    }
}