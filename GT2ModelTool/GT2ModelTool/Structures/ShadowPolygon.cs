using System;
using System.Collections.Generic;
using System.IO;
using StreamExtensions;

namespace GT2.ModelTool.Structures
{
    using ExportMetadata;

    public class ShadowPolygon
    {
        public ShadowVertex Vertex0 { get; set; }
        public ShadowVertex Vertex1 { get; set; }
        public ShadowVertex Vertex2 { get; set; }
        public ShadowVertex Vertex3 { get; set; }
        public bool IsGradientShaded { get; set; }
        public bool IsQuad => Vertex3 != null;

        public static List<byte> values = new List<byte>();

        public virtual void ReadFromCDO(Stream stream, bool isQuad, List<ShadowVertex> vertices)
        {
            uint data = stream.ReadUInt();
            IsGradientShaded = (data & 0x8000_0000) == 0;

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
            // Leo's GTCarViewer shadow faces
            var mockups = new int[4, 4] { { 0, 1, 2, 3 }, { 3, 2, 7, 6 }, { 6, 7, 4, 5 }, { 5, 4, 8, 9 } };
            int vertexNumber = 0;
            Vertex0 = vertices[mockups[polygonNumber, vertexNumber++]];
            Vertex1 = vertices[mockups[polygonNumber, vertexNumber++]];
            Vertex2 = vertices[mockups[polygonNumber, vertexNumber++]];
            Vertex3 = vertices[mockups[polygonNumber, vertexNumber++]];
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
            if (!IsGradientShaded)
            {
                data += 0x8000_0000;
            }
            stream.WriteUInt(data);
        }

        public void WriteToOBJ(TextWriter writer, bool isQuad, List<ShadowVertex> vertices, int firstVertexNumber)
        {
            writer.WriteLine($"usemtl shadow{(IsGradientShaded ? "gradient" : "")}");
            writer.WriteLine($"f {WriteVertexToOBJ(Vertex0, vertices, firstVertexNumber)} " +
                             $"{WriteVertexToOBJ(Vertex1, vertices, firstVertexNumber)} " +
                             $"{WriteVertexToOBJ(Vertex2, vertices, firstVertexNumber)}" +
                             (isQuad ? $" {WriteVertexToOBJ(Vertex3, vertices, firstVertexNumber)}" : ""));
        }

        private string WriteVertexToOBJ(ShadowVertex vertex, List<ShadowVertex> vertices, int firstVertexNumber) =>
            $"{vertices.IndexOf(vertex) + firstVertexNumber}";

        public void ReadFromOBJ(string line, List<ShadowVertex> vertices, int startID, string material, ShadowMetadata metadata)
        {
            string[] parts = line.Split(' ');
            if (parts.Length < 4 || parts.Length > 5)
            {
                throw new Exception("Shadow face does not contain exactly three or four vertices");
            }
            Vertex0 = ParseVertex(parts[1], vertices, startID);
            Vertex1 = ParseVertex(parts[2], vertices, startID);
            Vertex2 = ParseVertex(parts[3], vertices, startID);
            if (parts.Length == 5)
            {
                Vertex3 = ParseVertex(parts[4], vertices, startID);
            }
            IsGradientShaded = material == metadata.GradientMaterialName;
        }

        private ShadowVertex ParseVertex(string value, List<ShadowVertex> vertices, int startID)
        {
            string[] vertexData = value.Split('/');
            int vertexID = int.Parse(vertexData[0]) - 1 - startID;
            return vertices[vertexID];
        }
    }
}