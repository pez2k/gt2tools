using System;
using System.Collections.Generic;
using System.IO;
using StreamExtensions;

namespace GT2.ModelTool.Structures
{
    public class ShadowPolygon
    {
        public ShadowVertex Vertex0 { get; set; }
        public ShadowVertex Vertex1 { get; set; }
        public ShadowVertex Vertex2 { get; set; }
        public ShadowVertex Vertex3 { get; set; }
        public bool IsGradientShaded { get; set; }

        public static List<byte> values = new List<byte>();

        public virtual void ReadFromCDO(Stream stream, bool isQuad, List<ShadowVertex> vertices)
        {
            uint data = stream.ReadUInt();
            IsGradientShaded = (data & 0x8000_0000) != 0;

            int vertex0Ref = (int)data & 0x3F;
            Vertex0 = vertices[vertex0Ref];
            int vertex1Ref = (int)(data >> 6) & 0x3F;
            Vertex1 = vertices[vertex1Ref];
            int vertex2Ref = (int)(data >> 12) & 0x3F;
            Vertex2 = vertices[vertex2Ref];
            int vertex3Ref = (int)(data >> 18) & 0x3F;
            if (isQuad)
            {
                Vertex3 = vertices[vertex3Ref];
            }
            else if (vertex3Ref != 0x00)
            {
                throw new Exception("Vertex 3 in shadow triangle not zero");
            }
        }

        public virtual void ReadFromCAR(Stream stream, List<ShadowVertex> vertices, int polygonNumber)
        {
            // build shadow quads - doesn't work
            int vertexNumber = polygonNumber * 4;
            Vertex0 = vertices[vertexNumber++];
            Vertex1 = vertices[vertexNumber++];
            Vertex2 = vertices[vertexNumber++];
            Vertex3 = vertices[vertexNumber++];

            // Leo's GTCarViewer shadow mockups
            /*var mockups = new int[4, 4] { { 0, 1, 2, 3 }, { 3, 2, 7, 6 }, { 6, 7, 4, 5 }, { 5, 4, 8, 9 } };
            //var mockups = new int[4, 4] { { 3, 2, 1, 0 }, { 6, 7, 2, 3 }, { 5, 4, 7, 6 }, { 9, 8, 4, 5 } };
            int vertexNumber = 0;
            Vertex0 = vertices[mockups[polygonNumber, vertexNumber++]];
            Vertex1 = vertices[mockups[polygonNumber, vertexNumber++]];
            Vertex2 = vertices[mockups[polygonNumber, vertexNumber++]];
            Vertex3 = vertices[mockups[polygonNumber, vertexNumber++]];*/
        }

        public virtual void WriteToCDO(Stream stream, bool isQuad, List<ShadowVertex> vertices)
        {
            uint data = (uint)vertices.IndexOf(Vertex0);
            data += (uint)(vertices.IndexOf(Vertex1) << 6);
            data += (uint)(vertices.IndexOf(Vertex2) << 12);
            if (isQuad)
            {
                data += (uint)(vertices.IndexOf(Vertex3) << 18);
            }
            if (IsGradientShaded)
            {
                data += 0x8000_0000;
            }
            stream.WriteUInt(data);
        }

        public void WriteToOBJ(TextWriter writer, bool isQuad, List<ShadowVertex> vertices, int firstVertexNumber) =>
            writer.WriteLine($"f {WriteVertexToOBJ(Vertex0, vertices, firstVertexNumber)} " +
                             $"{WriteVertexToOBJ(Vertex1, vertices, firstVertexNumber)} " +
                             $"{WriteVertexToOBJ(Vertex2, vertices, firstVertexNumber)}" +
                             (isQuad ? $" {WriteVertexToOBJ(Vertex3, vertices, firstVertexNumber)}" : ""));

        private string WriteVertexToOBJ(ShadowVertex vertex, List<ShadowVertex> vertices, int firstVertexNumber) =>
            $"{vertices.IndexOf(vertex) + firstVertexNumber}";
    }
}