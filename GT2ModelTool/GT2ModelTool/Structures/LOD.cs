using System.Collections.Generic;
using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class LOD
    {
        public List<Vertex> Vertices { get; set; }
        public List<Normal> Normals { get; set; }
        public List<Triangle> Triangles { get; set; }
        public List<Quad> Quads { get; set; }
        public List<UVTriangle> UVTriangles { get; set; }
        public List<UVQuad> UVQuads { get; set; }

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
            Triangles = new List<Triangle>(triangleCount);
            Quads = new List<Quad>(quadCount);
            UVTriangles = new List<UVTriangle>(uvTriangleCount);
            UVQuads = new List<UVQuad>(uvQuadCount);
            
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
                var triangle = new Triangle();
                triangle.ReadFromCDO(stream);
                Triangles.Add(triangle);
            }

            for (int i = 0; i < quadCount; i++)
            {
                var quad = new Quad();
                quad.ReadFromCDO(stream);
                Quads.Add(quad);
            }

            for (int i = 0; i < uvTriangleCount; i++)
            {
                var uvTriangle = new UVTriangle();
                uvTriangle.ReadFromCDO(stream);
                UVTriangles.Add(uvTriangle);
            }

            for (int i = 0; i < uvQuadCount; i++)
            {
                var uvQuad = new UVQuad();
                uvQuad.ReadFromCDO(stream);
                UVQuads.Add(uvQuad);
            }
        }
    }
}