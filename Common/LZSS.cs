using System.IO;
using StreamExtensions;

namespace GT1.LZSS
{
    public static class LZSS
    {
        private const byte CompressedFlag = 0x01;
        private const byte MinimumUncompressedDataLength = 3;
        private const byte MultiByteDistanceFlag = 0x80;
        private const byte MultiByteDistanceFlagMask = MultiByteDistanceFlag - 1;
        private const byte DataLengthByteSize = 1;

        public static void Decompress(Stream compressed, Stream output)
        {
            ushort compressionFlags = CompressedFlag;
            compressed.Position = 0;

            while (compressed.Position < compressed.Length)
            {
                if (AllFlagsConsumed(compressionFlags))
                {
                    compressionFlags = ReadNextCompressionFlags(compressed);
                }

                if (IsCurrentByteCompressed(compressionFlags))
                {
                    CopyCompressedBytes(compressed, output);
                }
                else
                {
                    CopyUncompressedByte(compressed, output);
                }

                compressionFlags = ConsumeFlag(compressionFlags);
            }
        }

        private static bool AllFlagsConsumed(ushort compressionFlags) => compressionFlags == CompressedFlag;

        private static ushort ReadNextCompressionFlags(Stream compressed) => (ushort)(compressed.ReadByte() | 0x100);

        private static bool IsCurrentByteCompressed(ushort compressionFlags) => (compressionFlags & CompressedFlag) == CompressedFlag;

        private static void CopyUncompressedByte(Stream compressed, Stream output) => output.WriteByte(compressed.ReadSingleByte());

        private static void CopyCompressedBytes(Stream compressed, Stream output)
        {
            ushort lengthOfDecompressedData = GetUncompressedDataLength(compressed);
            ushort distanceToStartOfUncompressedData = GetDistanceToStartOfUncompressedData(compressed);
            DecompressBytes(output, lengthOfDecompressedData, distanceToStartOfUncompressedData);
        }

        private static ushort GetUncompressedDataLength(Stream compressed) => (ushort)(compressed.ReadByte() + MinimumUncompressedDataLength);

        private static ushort GetDistanceToStartOfUncompressedData(Stream compressed)
        {
            ushort distance;
            byte distanceFirstByte = compressed.ReadSingleByte();
            if (IsMultiByteDistance(distanceFirstByte))
            {
                byte distanceSecondByte = compressed.ReadSingleByte();
                distance = (ushort)(((distanceFirstByte & MultiByteDistanceFlagMask) * 256) + distanceSecondByte); // maximum 32,768, therefore 32kb window
            }
            else
            {
                distance = distanceFirstByte;
            }
            return (ushort)(distance + DataLengthByteSize);
        }

        private static bool IsMultiByteDistance(byte firstByte) => (firstByte & MultiByteDistanceFlag) == MultiByteDistanceFlag;

        private static void DecompressBytes(Stream output, ushort lengthOfDecompressedData, ushort distanceToStartOfUncompressedData)
        {
            long endOfOutput = output.Position;
            output.Position -= distanceToStartOfUncompressedData;
            long steppedBackPosition = output.Position;
            byte[] decompressedBytes = new byte[lengthOfDecompressedData];
            for (int i = 0; i < lengthOfDecompressedData; i++)
            {
                decompressedBytes[i] = output.ReadSingleByte();
                if (output.Position >= output.Length)
                {
                    output.Position = steppedBackPosition;
                }
            }
            output.Position = endOfOutput;
            output.Write(decompressedBytes, 0, lengthOfDecompressedData);
        }

        private static ushort ConsumeFlag(ushort compressionFlags) => (ushort)(compressionFlags >> 1);
    }
}