using System.Collections.Generic;
using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class LOD
    {
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
            stream.Position += sizeof(ushort) * 32;

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
    }
}