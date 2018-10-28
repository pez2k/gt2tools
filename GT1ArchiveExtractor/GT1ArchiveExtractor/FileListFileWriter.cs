using System.IO;

namespace GT2.GT1ArchiveExtractor
{
    public class FileListFileWriter : IFileWriter
    {
        public void CreateDirectory(string path)
        {
            using (StreamWriter output = File.AppendText("filelist.txt"))
            {
                output.Write($"{path} {path}\r\n");
            }
        }

        public void Write(string path, byte[] contents) => CreateDirectory(path);
    }
}