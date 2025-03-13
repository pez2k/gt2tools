using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading;
using StreamExtensions;

namespace GT2.ModelTool
{
    using ExportMetadata;
    using Structures;

    class Program
    {
        private static readonly JsonSerializerOptions serializerOptions = new() { WriteIndented = true };

        private const ushort UnknownDataVersion = 2;

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Incorrect usage.");
                return;
            }

            // avoid issues with European languages using , for decimal point, as Max doesn't like it
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            string inputPath = args[1];
            string fileExtension = Path.GetExtension(inputPath).ToLower();
            Model model;
            switch (fileExtension)
            {
                case ".car":
                    model = ReadGT1(inputPath);
                    break;
                case ".cdo":
                case ".cno":
                    model = ReadGT2(inputPath);
                    break;
                case ".obj":
                    model = ReadOBJ(inputPath);
                    break;
                default:
                    Console.WriteLine("Unsupported input type.");
                    return;
            }

            string directory = Path.GetDirectoryName(inputPath);
            string filename = Path.GetFileNameWithoutExtension(inputPath);
            bool isNight = fileExtension == ".cno" || filename.EndsWith("_night");
            switch (args[0])
            {
                case "-o2":
                    WriteGT2(model, directory, filename, isNight);
                    break;
                case "-oe":
                    WriteOBJ(model, directory, filename, isNight);
                    break;
                default:
                    Console.WriteLine("Unsupported output type.");
                    return;
            }
        }

        private static Model ReadGT1(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var model = new Model();
                model.ReadFromCAR(stream);
                return model;
            }
        }

        private static Model ReadGT2(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var model = new Model();
                model.ReadFromCDO(stream);
                return model;
            }
        }

        private static Model ReadOBJ(string filename)
        {
            using (TextReader modelReader = new StreamReader(filename))
            {
                using (Stream unknownData = TryOpenUnknownDataFile(filename))
                {
                    var model = new Model();
                    model.ReadFromOBJ(modelReader, unknownData);
                    return model;
                }
            }
        }

        private static Stream TryOpenUnknownDataFile(string objFilename)
        {
            string binFilename = Path.GetFileNameWithoutExtension(objFilename) + ".bin";
            if (File.Exists(binFilename))
            {
                var stream = new FileStream(binFilename, FileMode.Open, FileAccess.Read);
                if (stream.ReadUShort() == UnknownDataVersion)
                {
                    return stream;
                }
                Console.WriteLine("Incorrect unknown data version, skipping...");
                stream.Dispose();
            }
            return null;
        }

        private static void WriteGT2(Model model, string path, string filename, bool isNight)
        {
            using (FileStream outStream = new FileStream(Path.Combine(path, $"{filename}.c{(isNight ? "n" : "d")}o"), FileMode.Create, FileAccess.ReadWrite))
            {
                model.WriteToCDO(outStream);
            }
        }

        private static void WriteOBJ(Model model, string path, string filename, bool isNight)
        {
            filename += isNight ? "_night" : "";
            string objFileName = $"{filename}.obj";
            using (TextWriter modelWriter = new StreamWriter(Path.Combine(path, objFileName)))
            {
                using (TextWriter materialWriter = new StreamWriter(Path.Combine(path, $"{filename}.mtl")))
                {
                    using (FileStream unknownData = new(Path.Combine(path, $"{filename}.bin"), FileMode.Create, FileAccess.ReadWrite))
                    {
                        unknownData.WriteUShort(UnknownDataVersion);

                        ModelMetadata metadata = new() { ModelFilename = objFileName };
                        model.WriteToOBJ(modelWriter, materialWriter, filename, unknownData, metadata);

                        using (StreamWriter jsonWriter = new(Path.Combine(path, $"{filename}.json")))
                        {
                            jsonWriter.Write(JsonSerializer.Serialize(metadata, serializerOptions));
                        }
                    }
                }
            }
        }
    }
}