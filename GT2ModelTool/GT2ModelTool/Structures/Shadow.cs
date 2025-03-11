﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GT2.ModelTool.Structures
{
    using ExportMetadata;
    using StreamExtensions;

    public class Shadow
    {
        private ushort unknown;
        private short lowBoundX;
        private short lowBoundY;
        private short lowBoundZ;
        private short lowBoundW;
        private short highBoundX;
        private short highBoundY;
        private short highBoundZ;
        private short highBoundW;
        private byte unknown2;
        private byte unknown3;

        public ushort Scale { get; set; }
        public List<ShadowVertex> Vertices { get; set; }
        public List<ShadowPolygon> Triangles { get; set; }
        public List<ShadowPolygon> Quads { get; set; }

        public void ReadFromCDO(Stream stream)
        {
            ushort vertexCount = stream.ReadUShort(); // 3B40
            ushort triangleCount = stream.ReadUShort();
            ushort quadCount = stream.ReadUShort();
            unknown = stream.ReadUShort();
            lowBoundX = stream.ReadShort();
            lowBoundY = stream.ReadShort();
            lowBoundZ = stream.ReadShort();
            lowBoundW = stream.ReadShort();
            highBoundX = stream.ReadShort();
            highBoundY = stream.ReadShort();
            highBoundZ = stream.ReadShort();
            highBoundW = stream.ReadShort();
            Scale = stream.ReadUShort();
            unknown2 = stream.ReadSingleByte();
            unknown3 = stream.ReadSingleByte();

            Vertices = new List<ShadowVertex>(vertexCount); // 3B5C
            Triangles = new List<ShadowPolygon>(triangleCount); // 3BBC
            Quads = new List<ShadowPolygon>(quadCount);
            
            for (int i = 0; i < vertexCount; i++)
            {
                var vertex = new ShadowVertex();
                vertex.ReadFromCDO(stream);
                Vertices.Add(vertex);
            }

            for (int i = 0; i < triangleCount; i++)
            {
                var triangle = new ShadowPolygon();
                triangle.ReadFromCDO(stream, false, Vertices);
                Triangles.Add(triangle);
            }

            for (int i = 0; i < quadCount; i++)
            {
                var quad = new ShadowPolygon();
                quad.ReadFromCDO(stream, true, Vertices);
                Quads.Add(quad);
            }
        }

        public void ReadFromCAR(Stream stream)
        {
            ushort unknown = stream.ReadUShort(); // always 0?
            ushort quadCount = stream.ReadUShort();
            Scale = stream.ReadUShort();
            ushort unknown2 = stream.ReadUShort(); // always 0?

            lowBoundX = stream.ReadShort();
            lowBoundY = stream.ReadShort();
            lowBoundZ = stream.ReadShort();
            lowBoundW = stream.ReadShort();
            highBoundX = stream.ReadShort();
            highBoundY = stream.ReadShort();
            highBoundZ = stream.ReadShort();
            highBoundW = stream.ReadShort();

            stream.Position += 8;

            int vertexCount = quadCount * 4;
            Vertices = new List<ShadowVertex>(vertexCount);
            Triangles = new List<ShadowPolygon>();
            Quads = new List<ShadowPolygon>(quadCount);

            for (int i = 0; i < vertexCount; i++)
            {
                var vertex = new ShadowVertex();
                vertex.ReadFromCAR(stream);
                Vertices.Add(vertex);
            }

            for (int i = 0; i < quadCount; i++)
            {
                var quad = new ShadowPolygon();
                quad.ReadFromCAR(stream, Vertices, i);
                Quads.Add(quad);
            }
        }

        public void GenerateBoundingBox()
        {
            lowBoundX = Vertices.Select(v => v.X).Min();
            lowBoundY = 0;
            lowBoundZ = Vertices.Select(v => v.Z).Min();
            lowBoundW = 0;
            highBoundX = Vertices.Select(v => v.X).Max();
            highBoundY = 0;
            highBoundZ = Vertices.Select(v => v.Z).Max();
            highBoundW = 0;
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteUShort((ushort)Vertices.Count);
            stream.WriteUShort((ushort)Triangles.Count);
            stream.WriteUShort((ushort)Quads.Count);
            stream.WriteUShort(unknown);
            stream.WriteShort(lowBoundX);
            stream.WriteShort(lowBoundY);
            stream.WriteShort(lowBoundZ);
            stream.WriteShort(lowBoundW);
            stream.WriteShort(highBoundX);
            stream.WriteShort(highBoundY);
            stream.WriteShort(highBoundZ);
            stream.WriteShort(highBoundW);
            stream.WriteUShort(Scale);
            stream.WriteByte(unknown2);
            stream.WriteByte(unknown3);

            foreach (ShadowVertex vertex in Vertices)
            {
                vertex.WriteToCDO(stream);
            }

            foreach (ShadowPolygon triangle in Triangles)
            {
                triangle.WriteToCDO(stream, false, Vertices);
            }

            foreach (ShadowPolygon quad in Quads)
            {
                quad.WriteToCDO(stream, true, Vertices);
            }
        }

        public void WriteToOBJ(TextWriter writer, int firstVertexNumber, Stream unknownData, ShadowMetadata metadata)
        {
            unknownData.WriteUShort(unknown);
            unknownData.WriteByte(unknown2);
            unknownData.WriteByte(unknown3);

            metadata.Unknown = unknown;
            metadata.Unknown2 = unknown2;
            metadata.Unknown3 = unknown3;

            double scaleFactor = LOD.ConvertScale(Scale);
            writer.WriteLine($"g shadow/scale={scaleFactor}");
            metadata.Scale = scaleFactor;

            writer.WriteLine("# vertices");
            Vertices.ForEach(vertex => vertex.WriteToOBJ(writer, scaleFactor));

            writer.WriteLine("# triangles");
            Triangles.ForEach(polygon => polygon.WriteToOBJ(writer, false, Vertices, firstVertexNumber));

            writer.WriteLine("# quads");
            Quads.ForEach(polygon => polygon.WriteToOBJ(writer, true, Vertices, firstVertexNumber));
        }

        public void PrepareForOBJRead(Stream unknownData)
        {
            Vertices = new List<ShadowVertex>();
            Triangles = new List<ShadowPolygon>();
            Quads = new List<ShadowPolygon>();
            if (unknownData != null)
            {
                unknown = unknownData.ReadUShort();
                unknown2 = unknownData.ReadSingleByte();
                unknown3 = unknownData.ReadSingleByte();
            }
        }
    }
}