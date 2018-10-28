namespace GT2.GT1ArchiveExtractor
{
    public interface IFileWriter
    {
        void CreateDirectory(string path);

        void Write(string path, byte[] contents);
    }
}