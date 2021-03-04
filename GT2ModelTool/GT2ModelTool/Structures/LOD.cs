using System.Collections.Generic;
using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class LOD
    {
        private byte[] unknown = new byte[44];
        private ushort lowBoundX;
        private ushort lowBoundY;
        private ushort lowBoundZ;
        private ushort lowBoundW;
        private ushort highBoundX;
        private ushort highBoundY;
        private ushort highBoundZ;
        private ushort highBoundW;
        private ushort scale;
        private byte unknown2;
        private byte unknown3;

        public List<Vertex> Vertices { get; set; }
        public List<Normal> Normals { get; set; }
        public List<Polygon> Triangles { get; set; }
        public List<Polygon> Quads { get; set; }
        public List<UVPolygon> UVTriangles { get; set; }
        public List<UVPolygon> UVQuads { get; set; }

        public void ReadFromCDO(Stream stream)
        {
            ushort vertexCount = stream.ReadUShort();
            ushort normalCount = stream.ReadUShort();
            ushort triangleCount = stream.ReadUShort();
            ushort quadCount = stream.ReadUShort();
            stream.Position += sizeof(ushort) * 2;
            ushort uvTriangleCount = stream.ReadUShort();
            ushort uvQuadCount = stream.ReadUShort();
            stream.Read(unknown);
            lowBoundX = stream.ReadUShort();
            lowBoundY = stream.ReadUShort();
            lowBoundZ = stream.ReadUShort();
            lowBoundW = stream.ReadUShort();
            highBoundX = stream.ReadUShort();
            highBoundY = stream.ReadUShort();
            highBoundZ = stream.ReadUShort();
            highBoundW = stream.ReadUShort();
            scale = stream.ReadUShort();
            unknown2 = stream.ReadSingleByte();
            unknown3 = stream.ReadSingleByte();

            Vertices = new List<Vertex>(vertexCount);
            Normals = new List<Normal>(normalCount);
            Triangles = new List<Polygon>(triangleCount);
            Quads = new List<Polygon>(quadCount);
            UVTriangles = new List<UVPolygon>(uvTriangleCount);
            UVQuads = new List<UVPolygon>(uvQuadCount);
            
            for (int i = 0; i < vertexCount; i++)
            {
                var vertex = new Vertex();
                vertex.ReadFromCDO(stream);
                Vertices.Add(vertex);
            }

            for (int i = 0; i < normalCount; i++)
            {
                var normal = new Normal();
                normal.ReadFromCDO(stream);
                Normals.Add(normal);
            }

            for (int i = 0; i < triangleCount; i++)
            {
                var triangle = new Polygon();
                triangle.ReadFromCDO(stream, false, Vertices, Normals);
                Triangles.Add(triangle);
            }

            for (int i = 0; i < quadCount; i++)
            {
                var quad = new Polygon();
                quad.ReadFromCDO(stream, true, Vertices, Normals);
                Quads.Add(quad);
            }

            for (int i = 0; i < uvTriangleCount; i++)
            {
                var uvTriangle = new UVPolygon();
                uvTriangle.ReadFromCDO(stream, false, Vertices, Normals);
                UVTriangles.Add(uvTriangle);
            }

            for (int i = 0; i < uvQuadCount; i++)
            {
                var uvQuad = new UVPolygon();
                uvQuad.ReadFromCDO(stream, true, Vertices, Normals);
                UVQuads.Add(uvQuad);
            }
        }

        public void ReadFromCAR(Stream stream)
        {
            ushort vertexCount = stream.ReadUShort();
            ushort normalCount = stream.ReadUShort();
            ushort triangleCount = stream.ReadUShort();
            ushort quadCount = stream.ReadUShort();
            stream.Position += sizeof(ushort) * 2;
            ushort uvTriangleCount = stream.ReadUShort();
            ushort uvQuadCount = stream.ReadUShort();
            stream.Position += sizeof(ushort) * 12;

            Vertices = new List<Vertex>(vertexCount);
            Normals = new List<Normal>(normalCount);
            Triangles = new List<Polygon>(triangleCount);
            Quads = new List<Polygon>(quadCount);
            UVTriangles = new List<UVPolygon>(uvTriangleCount);
            UVQuads = new List<UVPolygon>(uvQuadCount);

            for (int i = 0; i < vertexCount; i++)
            {
                var vertex = new Vertex();
                vertex.ReadFromCAR(stream);
                Vertices.Add(vertex);
            }

            for (int i = 0; i < normalCount; i++)
            {
                var normal = new Normal();
                normal.ReadFromCAR(stream);
                Normals.Add(normal);
            }

            for (int i = 0; i < triangleCount; i++)
            {
                var triangle = new Polygon();
                triangle.ReadFromCAR(stream, false, Vertices, Normals);
                Triangles.Add(triangle);
            }

            for (int i = 0; i < quadCount; i++)
            {
                var quad = new Polygon();
                quad.ReadFromCAR(stream, true, Vertices, Normals);
                Quads.Add(quad);
            }

            for (int i = 0; i < uvTriangleCount; i++)
            {
                var uvTriangle = new UVPolygon();
                uvTriangle.ReadFromCAR(stream, false, Vertices, Normals);
                UVTriangles.Add(uvTriangle);
            }

            for (int i = 0; i < uvQuadCount; i++)
            {
                var uvQuad = new UVPolygon();
                uvQuad.ReadFromCAR(stream, true, Vertices, Normals);
                UVQuads.Add(uvQuad);
            }
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteUShort((ushort)Vertices.Count);
            stream.WriteUShort((ushort)Normals.Count);
            stream.WriteUShort((ushort)Triangles.Count);
            stream.WriteUShort((ushort)Quads.Count);
            stream.Position += sizeof(ushort) * 2;
            stream.WriteUShort((ushort)UVTriangles.Count);
            stream.WriteUShort((ushort)UVQuads.Count);
            stream.Write(unknown);
            stream.WriteUShort(lowBoundX);
            stream.WriteUShort(lowBoundY);
            stream.WriteUShort(lowBoundZ);
            stream.WriteUShort(lowBoundW);
            stream.WriteUShort(highBoundX);
            stream.WriteUShort(highBoundY);
            stream.WriteUShort(highBoundZ);
            stream.WriteUShort(highBoundW);
            stream.WriteUShort(scale);
            stream.WriteByte(unknown2);
            stream.WriteByte(unknown3);

            foreach (Vertex vertex in Vertices)
            {
                vertex.WriteToCDO(stream);
            }

            foreach (Normal normal in Normals)
            {
                normal.WriteToCDO(stream);
            }

            foreach (Polygon triangle in Triangles)
            {
                triangle.WriteToCDO(stream, false, Vertices);
            }

            foreach (Polygon quad in Quads)
            {
                quad.WriteToCDO(stream, true, Vertices);
            }

            foreach (UVPolygon uvTriangle in UVTriangles)
            {
                uvTriangle.WriteToCDO(stream, false, Vertices);
            }

            foreach (UVPolygon uvQuad in UVQuads)
            {
                uvQuad.WriteToCDO(stream, true, Vertices);
            }
        }
    }
}