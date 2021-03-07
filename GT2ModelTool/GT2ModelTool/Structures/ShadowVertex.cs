using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class ShadowVertex
    {
        public short X { get; set; }
        public short Z { get; set; }

        public void ReadFromCDO(Stream stream)
        {
            X = stream.ReadShort();
            Z = stream.ReadShort();
        }

        public void ReadFromCAR(Stream stream)
        {
            X = stream.ReadShort();
            stream.ReadShort(); // unused Y, always 0
            Z = stream.ReadShort();
            Z = (short)(0 - Z); // is this needed like with the main model verts?
            stream.Position += 2; // padding
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteShort(X);
            stream.WriteShort(Z);
        }
    }
}