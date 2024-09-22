using System.IO;

namespace GT2.SaveEditor
{
    public class SaveFrame
    {
        public byte[] Data { get; set; } = new byte[0x80];

        public void ReadFromSave(Stream file) => file.Read(Data);

        public void WriteToSave(Stream file) => file.Write(Data);
    }
}