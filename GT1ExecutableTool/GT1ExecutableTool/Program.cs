using GT1.LZSS;
using StreamExtensions;

namespace GT1ExecutableTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: GT1ExecutableTool <executable to decompress>");
                return;
            }

            const int psxBlockSize = 0x800; // Sony PS-X EXE format
            const long psxMagic = 0x45584520582D5350; // "PS-X EXE" (ASCII)
            const int pslzHeaderSize = 0x1C; // Polyphony PSLZ compressed executable
            const uint pslzMagic = 0x5A4C5350; // "PSLZ" (ASCII)

            string filePath = args[0];
            using (FileStream file = new(filePath, FileMode.Open, FileAccess.Read))
            {
                // PS-X EXE header parsing
                if (file.ReadULong() != psxMagic)
                {
                    Console.WriteLine("Error: not a PS-X EXE format file");
                    return;
                }
                file.Position = 0x10;
                uint entryPoint = file.ReadUInt();
                file.Position = 0x18;
                uint codeMemoryLocation = file.ReadUInt();
                uint memoryToFileOffset = codeMemoryLocation - psxBlockSize; // The difference between where the code will be in RAM, and where it is in the EXE

                // PSLZ header parsing
                file.Position = entryPoint - memoryToFileOffset;
                file.Position -= pslzHeaderSize; // The PSLZ header block sits immediately before the loader function, which is the EXE entry point
                if (file.ReadUInt() != pslzMagic)
                {
                    Console.WriteLine("Error: no PSLZ compression header found");
                    return;
                }
                uint decompressedDataEnd = file.ReadUInt();
                int decompressedDataSize = file.ReadInt();
                uint compressedDataEnd = file.ReadUInt();
                uint decompressorFunctionLocation = file.ReadUInt(); // Where Polyphony's loader will relocate itself to avoid being overwritten - useless here
                uint decompressorFunctionSize = file.ReadUInt(); // Unused - the code actually relocates 16 bytes, 25 times
                uint compressedDataStart = file.ReadUInt();

                uint compressedDataSize = compressedDataEnd - compressedDataStart + 1;
                file.Position = compressedDataStart - memoryToFileOffset;
                byte[] compressedData = new byte[compressedDataSize];
                file.Read(compressedData);

                // The data is compressed and decompressed backwards, so to use the normal decompressor we must reverse it
                byte[] decompressedData;
                using (MemoryStream reversedCompressedData = new(compressedData.Reverse().ToArray()))
                {
                    using (MemoryStream reversedDecompressedData = new(decompressedDataSize))
                    {
                        LZSS.Decompress(reversedCompressedData, reversedDecompressedData);
                        reversedDecompressedData.Position = 0;
                        decompressedData = reversedDecompressedData.ToArray().Reverse().ToArray();
                    }
                }

                // Create a new EXE with the decompressed code
                using (FileStream output = new($"{filePath}_decompressed", FileMode.Create, FileAccess.Write))
                {
                    file.Position = 0;
                    byte[] psxHeader = new byte[psxBlockSize];
                    file.Read(psxHeader); // Copy the old header as a start
                    output.Write(psxHeader);
                    output.Write(decompressedData);
                    output.Position = 0xC;
                    output.WriteUInt((uint)output.Length); // End of code
                    output.WriteUInt(codeMemoryLocation); // Entry point is simply the top of where the code is loaded
                    output.Position = output.Length;
                    output.MoveToNextMultipleOf(psxBlockSize); // Pad to a multiple of 0x800
                    output.SetLength(output.Position);
                    output.Position = 0x1C;
                    output.WriteUInt((uint)output.Length - psxBlockSize); // Size of code block (file size minus EXE header block)
                }
            }
        }
    }
}