using System.Runtime.InteropServices;

namespace GT2.ModelTool.Structures
{
    public class CDOHeader // 0x50
    {
        public ushort VertexCount { get; set; }
        public ushort NormalCount { get; set; }
        public ushort TriangleCount { get; set; }
        public ushort QuadCount { get; set; }
        public ushort Unknown1 { get; set; } = 0;
        public ushort Unknown2 { get; set; } = 0;
        public ushort UVTriangleCount { get; set; }
        public ushort UVQuadCount { get; set; }
        public ushort Unknown3 { get; set; } = 0;
        public ushort Unknown4 { get; set; } = 0;
        public ushort Unknown5 { get; set; } = 0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)]
        public ushort[] Padding = new ushort[27];
    }
}