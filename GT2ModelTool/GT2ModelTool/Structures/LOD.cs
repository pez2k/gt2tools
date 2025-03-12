﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GT2.ModelTool.Structures
{
    using ExportMetadata;
    using StreamExtensions;

    public class LOD
    {
        private byte[] unknown = new byte[44];
        private uint unknown40;
        private uint unknown41;
        private uint unknown42;
        private uint unknown43;
        private uint unknown44;
        private uint unknown45;
        private uint unknown46;
        private uint unknown47;
        private uint unknown48;
        private uint unknown49;
        private uint unknown50;
        private short lowBoundX;
        private short lowBoundY;
        private short lowBoundZ;
        private short lowBoundW;
        private short highBoundX;
        private short highBoundY;
        private short highBoundZ;
        private short highBoundW;
        private ushort unknown2;

        public ushort Scale { get; set; }
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
            stream.Position -= 44;
            unknown40 = stream.ReadUInt();
            unknown41 = stream.ReadUInt();
            unknown42 = stream.ReadUInt();
            unknown43 = stream.ReadUInt();
            unknown44 = stream.ReadUInt();
            unknown45 = stream.ReadUInt();
            unknown46 = stream.ReadUInt();
            unknown47 = stream.ReadUInt();
            unknown48 = stream.ReadUInt();
            unknown49 = stream.ReadUInt();
            unknown50 = stream.ReadUInt();

            lowBoundX = stream.ReadShort(); // at 8C0
            lowBoundY = stream.ReadShort();
            lowBoundZ = stream.ReadShort();
            lowBoundW = stream.ReadShort();
            highBoundX = stream.ReadShort();
            highBoundY = stream.ReadShort();
            highBoundZ = stream.ReadShort();
            highBoundW = stream.ReadShort();
            Scale = stream.ReadUShort(); // at 8D0
            unknown2 = stream.ReadUShort(); // 13E8 LOD0/1, 14C1 LOD2

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
            ushort vertexCount = stream.ReadUShort(); // at 0x80
            ushort normalCount = stream.ReadUShort();
            ushort triangleCount = stream.ReadUShort();
            ushort quadCount = stream.ReadUShort();
            stream.Position += sizeof(ushort) * 2;
            ushort uvTriangleCount = stream.ReadUShort();
            ushort uvQuadCount = stream.ReadUShort();
            stream.Position += sizeof(ushort) * 10; // at 0x90
            Scale = stream.ReadUShort();
            stream.Position += 2;

            Vertices = new List<Vertex>(vertexCount); // at 0xA8
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

            // calculate model bounds - can't spot this sort of data in CAR
            GenerateBoundingBox();
        }

        public void GenerateBoundingBox()
        {
            lowBoundX = Vertices.Select(v => v.X).Min();
            lowBoundY = Vertices.Select(v => v.Y).Min();
            lowBoundZ = Vertices.Select(v => v.Z).Min();
            lowBoundW = 0;
            highBoundX = Vertices.Select(v => v.X).Max();
            highBoundY = Vertices.Select(v => v.Y).Max();
            highBoundZ = Vertices.Select(v => v.Z).Max();
            highBoundW = 0;
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
            stream.WriteShort(lowBoundX);
            stream.WriteShort(lowBoundY);
            stream.WriteShort(lowBoundZ);
            stream.WriteShort(lowBoundW);
            stream.WriteShort(highBoundX);
            stream.WriteShort(highBoundY);
            stream.WriteShort(highBoundZ);
            stream.WriteShort(highBoundW);
            stream.WriteUShort(Scale);
            stream.WriteUShort(unknown2);

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
                triangle.WriteToCDO(stream, false, Vertices, Normals);
            }

            foreach (Polygon quad in Quads)
            {
                quad.WriteToCDO(stream, true, Vertices, Normals);
            }

            foreach (UVPolygon uvTriangle in UVTriangles)
            {
                uvTriangle.WriteToCDO(stream, false, Vertices, Normals);
            }

            foreach (UVPolygon uvQuad in UVQuads)
            {
                uvQuad.WriteToCDO(stream, true, Vertices, Normals);
            }
        }

        public void WriteToOBJ(TextWriter writer, int lodNumber, int firstVertexNumber, int firstNormalNumber, int firstCoordNumber,
                               Dictionary<string, int?> materialNames, Stream unknownData, LODMetadata metadata, List<MaterialMetadata> materialMetadata)
        {
            unknownData.Write(unknown);
            unknownData.WriteUShort(unknown2);

            metadata.ReplayZoomRelatedMaybe = unknown2;
            metadata.Unknown40 = unknown40;
            metadata.Unknown41 = unknown41;
            metadata.Unknown42 = unknown42;
            metadata.Unknown43 = unknown43;
            metadata.Unknown44 = unknown44;
            metadata.Unknown45 = unknown45;
            metadata.Unknown46 = unknown46;
            metadata.Unknown47 = unknown47;
            metadata.Unknown48 = unknown48;
            metadata.Unknown49 = unknown49;
            metadata.Unknown50 = unknown50;

            double scaleFactor = ConvertScale(Scale);
            writer.WriteLine($"g lod{lodNumber}/scale={scaleFactor}");
            metadata.Scale = scaleFactor;

            writer.WriteLine("# vertices");
            Vertices.ForEach(vertex => vertex.WriteToOBJ(writer, scaleFactor));

            writer.WriteLine("# normals");
            Normals.ForEach(normal => normal.WriteToOBJ(writer));

            List<UVCoordinate> coords = GetAllUVCoords();
            writer.WriteLine("# UV coords");
            coords.ForEach(coord => coord.WriteToOBJ(writer));

            writer.WriteLine("# triangles");
            Triangles.ForEach(polygon => polygon.WriteToOBJ(writer, false, Vertices, Normals, firstVertexNumber, firstNormalNumber, materialNames, materialMetadata));

            writer.WriteLine("# quads");
            Quads.ForEach(polygon => polygon.WriteToOBJ(writer, true, Vertices, Normals, firstVertexNumber, firstNormalNumber, materialNames, materialMetadata));

            writer.WriteLine("# UV triangles");
            UVTriangles.ForEach(polygon => polygon.WriteToOBJ(writer, false, Vertices, Normals, firstVertexNumber, firstNormalNumber, coords, firstCoordNumber, materialNames, materialMetadata));

            writer.WriteLine("# UV quads");
            UVQuads.ForEach(polygon => polygon.WriteToOBJ(writer, true, Vertices, Normals, firstVertexNumber, firstNormalNumber, coords, firstCoordNumber, materialNames, materialMetadata));
        }

        public static double ConvertScale(ushort scale) // from commongear's research
        {
            int scaleAmount = scale - 16;
            return scaleAmount < 0 ? 1D / (1 << -scaleAmount) : 1 << scaleAmount;
        }

        public static ushort ConvertScale(double scale)
        {
            return scale < 1 ? (ushort)(16 - GetShiftDistance((int)(1 / scale))) : (ushort)(16 + GetShiftDistance((int)scale));
        }

        private static int GetShiftDistance(int value)
        {
            if (value < 1 || (value > 1 && value % 2 != 0))
            {
                throw new Exception("Invalid scale value - must be 1, a power of 2, or 1 over a power of 2.");
            }

            int bits = 0;
            while (value > 1)
            {
                value >>= 1;
                bits++;
            }
            return bits;
        }

        public List<UVCoordinate> GetAllUVCoords() =>
            UVTriangles.SelectMany(polygon => new UVCoordinate[] { polygon.Vertex0UV, polygon.Vertex1UV, polygon.Vertex2UV })
                       .Concat(UVQuads.SelectMany(polygon => new UVCoordinate[] { polygon.Vertex0UV, polygon.Vertex1UV, polygon.Vertex2UV, polygon.Vertex3UV })).ToList();

        public void PrepareForOBJRead(Stream unknownData)
        {
            Vertices = new List<Vertex>();
            Normals = new List<Normal>();
            Triangles = new List<Polygon>();
            Quads = new List<Polygon>();
            UVTriangles = new List<UVPolygon>();
            UVQuads = new List<UVPolygon>();
            if (unknownData != null)
            {
                unknownData.Read(unknown);
                unknown2 = unknownData.ReadUShort();
            }
        }

        public void ReadFromOBJ(List<Vertex> vertices, List<Normal> normals, List<int> usedVertexIDs, List<int> usedNormalIDs)
        {
            Vertices = usedVertexIDs.OrderBy(id => id).Distinct().Select(id => vertices[id]).ToList();
            Normals = usedNormalIDs.OrderBy(id => id).Distinct().Select(id => normals[id]).ToList();
            GenerateBoundingBox();
        }
    }
}