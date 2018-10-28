using System;
using System.Linq;
using System.Reflection;

namespace GT2.GT1ArchiveExtractor
{
    public class FileData
    {
        public string Name;
        public bool Compressed;
        public byte[] Contents;
        
        public string GetExtension()
        {
            foreach (Type headerType in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.BaseType == typeof(KnownHeader)))
            {
                KnownHeader header = (KnownHeader)Activator.CreateInstance(headerType);
                
                if (HeaderMatches(header))
                {
                    return header.Extension;
                }
            }
            return "dat";
        }

        public bool IsArchive() => HeaderMatches(new ARCHeader());

        private bool HeaderMatches(KnownHeader header) => Contents.Take(header.Header.Length).SequenceEqual(header.Header);
    }
}