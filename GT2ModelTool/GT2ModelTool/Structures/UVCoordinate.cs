using System.IO;

namespace GT2.ModelTool.Structures
{
    public class UVCoordinate
    {
        public byte X { get; set; }
        public byte Y { get; set; }

        public void ReadFromCDO(Stream stream)
        {
            X = (byte)stream.ReadByte();
            Y = (byte)stream.ReadByte();
        }
    }
}