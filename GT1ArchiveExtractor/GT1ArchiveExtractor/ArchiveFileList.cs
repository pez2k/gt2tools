using System.Collections.Generic;
using System.IO;

namespace GT2.GT1ArchiveExtractor
{
    using StreamExtensions;

    public class ArchiveFileList : FileList
    {
        private byte[] arcFile;

        public ArchiveFileList(string fileName, byte[] arcFile) : base(fileName)
        {
            this.arcFile = arcFile;
        }

        public override IEnumerable<FileData> GetFiles()
        {
            using (var stream = new MemoryStream(arcFile))
            {
                stream.Position = 0x0E;
                ushort fileCount = stream.ReadUShort();

                for (ushort i = 0; i < fileCount; i++)
                {
                    uint offset = stream.ReadUInt();
                    uint size = stream.ReadUInt();
                    uint uncompressedSize = stream.ReadUInt();

                    long indexPosition = stream.Position;

                    stream.Position = offset;

                    byte[] buffer = new byte[size];
                    stream.Read(buffer);
                    
                    yield return new FileData { Name = $"_unknown{i:D4}", Compressed = size != uncompressedSize, Contents = buffer };

                    stream.Position = indexPosition;
                }
            }
        }
    }
}