using System.IO;
using System.Linq;
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

        public static void Compress(Stream input, Stream compressed)
        {
            const byte NoCompressionFlags = 0;
            for (long i = 0; i < input.Length; i++)
            {
                if (i % 8 == 0)
                {
                    compressed.WriteByte(NoCompressionFlags);
                }
                compressed.WriteByte(input.ReadSingleByte());
            }
        }

        public static void BrokenCompress(Stream input, Stream compressed)
        {
            const byte NoCompressionFlags = 0;
            long lastFlagsPosition = 0;
            byte flagsWritten = 8;
            byte compressionFlags = NoCompressionFlags;
            for (long i = 0; i < input.Length; i++)
            {
                if (flagsWritten >= 8)
                {
                    compressed.Position = lastFlagsPosition;
                    compressed.WriteByte(ReverseBits(compressionFlags));

                    if (compressed.Length > 1)
                    {
                        compressed.Position = compressed.Length;
                        lastFlagsPosition = compressed.Position;
                        compressed.WriteByte(0);
                    }
                    flagsWritten = 0;
                }

                var nextPattern = new byte[3]; // abort search if end of pattern > end of input
                input.Position = i;
                input.Read(nextPattern);

                long skipForwardTo = 0;
                long startOfPattern = -1;
                ushort patternLength = 0;
                for (long lookBackPosition = i - 1; lookBackPosition > 0; lookBackPosition--) // abort search if run out of file
                {
                    input.Position = lookBackPosition;
                    var currentPattern = new byte[3];
                    input.Read(currentPattern);

                    if (currentPattern.SequenceEqual(nextPattern))
                    {
                        // start looking for a longer match here

                        startOfPattern = lookBackPosition;
                        patternLength = 3;

                        if (lookBackPosition + patternLength > i)
                        {
                            skipForwardTo = lookBackPosition + patternLength;
                        }
                        break;
                    }

                    if (lookBackPosition == i - 32768) // end of window, abort
                    {
                        break;
                    }
                }

                if (startOfPattern == -1)
                {
                    compressed.WriteByte(nextPattern[0]); // failed to find a match, just write the current byte
                }
                else
                {
                    compressed.WriteByte((byte)(patternLength - 3));

                    ushort lookBackDistance = (ushort)(i - startOfPattern - 1);

                    if (lookBackDistance >= MultiByteDistanceFlag)
                    {
                        byte topByte = (byte)(lookBackDistance / 256);
                        compressed.WriteByte((byte)(topByte | MultiByteDistanceFlag));
                        compressed.WriteByte((byte)lookBackDistance);
                    }
                    else
                    {
                        compressed.WriteByte((byte)lookBackDistance);
                    }
                    compressionFlags |= CompressedFlag;
                }

                if (flagsWritten < 7)
                {
                    compressionFlags <<= 1;
                }
                flagsWritten++;

                if (skipForwardTo > 0)
                {
                    i = skipForwardTo;
                }
            }
        }

        private static byte ReverseBits(byte input)
        {
            byte output = 0;

            for (int i = 0; i < 8; i++)
            {
                output |= (byte)(input & 1);
                if (i < 7)
                {
                    input >>= 1;
                    output <<= 1;
                }
            }

            return output;
        }
    }
}