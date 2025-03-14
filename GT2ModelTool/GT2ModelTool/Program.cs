using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace GT2.ModelTool
{
    using ExportMetadata;
    using Structures;

    class Program
    {
        private static readonly JsonSerializerOptions serializerOptions = new() { WriteIndented = true };
        private static readonly JsonSerializerOptions deserializerOptions = new()
        {
            AllowTrailingCommas = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        static void Main(string[] args)
        {
            try
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
                    case ".json":
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
            catch (Exception exception)
            {
                Console.WriteLine($"An error has occurred.\r\n{exception.Message}.\r\n\r\nPress any key to exit.");
                Console.ReadKey();
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
            UnvalidatedModelMetadata unvalidatedMetadata;
            using (StreamReader jsonReader = new(filename))
            {
                string json = jsonReader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new Exception("JSON file is blank");
                }
                try
                {
                    unvalidatedMetadata = JsonSerializer.Deserialize<UnvalidatedModelMetadata>(json, deserializerOptions);
                }
                catch (JsonException exception)
                {
                    string[] parts = exception.Message.Split("System.Nullable`1[System.Double]. ");
                    if (parts.Length == 2)
                    {
                        throw new Exception($"JSON error: Invalid number. {parts[1].TrimEnd('.')}");
                    }
                    parts = exception.Message.Split("System.String. ");
                    if (parts.Length == 2)
                    {
                        throw new Exception($"JSON error: Invalid text string. {parts[1].TrimEnd('.')}");
                    }
                    parts = exception.Message.Split("System.Boolean. ");
                    if (parts.Length == 2)
                    {
                        throw new Exception($"JSON error: Invalid value - only true and false are allowed, without quotes. {parts[1].TrimEnd('.')}");
                    }
                    parts = exception.Message.Split("GT2.ModelTool.ExportMetadata.Unvalidated");
                    if (parts.Length == 2)
                    {
                        throw new Exception($"JSON error: Invalid object, expected {parts[1].TrimEnd('.')}");
                    }
                    throw new Exception($"JSON error: Invalid JSON. {exception.Message.TrimEnd('.').Replace("GT2.ModelTool.ExportMetadata.Unvalidated", "")}");
                }
            }

            ModelMetadata metadata = ValidateMetadata(unvalidatedMetadata);

            string objectFilename = Path.Combine(Path.GetDirectoryName(filename), metadata.ModelFilename);
            if (!File.Exists(objectFilename))
            {
                throw new Exception($"Could not find or access a file named {metadata.ModelFilename}");
            }

            using (TextReader modelReader = new StreamReader(objectFilename))
            {
                var model = new Model();
                model.ReadFromOBJ(modelReader, metadata);
                return model;
            }
        }

        private static ModelMetadata ValidateMetadata(UnvalidatedModelMetadata unvalidatedMetadata)
        {
            ModelMetadata metadata = new();
            if (string.IsNullOrWhiteSpace(unvalidatedMetadata.ModelFilename))
            {
                throw new Exception($"JSON error: {nameof(unvalidatedMetadata.ModelFilename)} is missing or blank");
            }

            metadata.ModelFilename = unvalidatedMetadata.ModelFilename;
            metadata.MenuWheels.FrontWheelRadius = ValidateUShort(unvalidatedMetadata.MenuWheels.FrontWheelRadius, nameof(unvalidatedMetadata.MenuWheels.FrontWheelRadius));
            metadata.MenuWheels.FrontWheelWidth = ValidateUShort(unvalidatedMetadata.MenuWheels.FrontWheelWidth, nameof(unvalidatedMetadata.MenuWheels.FrontWheelWidth));
            metadata.MenuWheels.RearWheelRadius = ValidateUShort(unvalidatedMetadata.MenuWheels.RearWheelRadius, nameof(unvalidatedMetadata.MenuWheels.RearWheelRadius));
            metadata.MenuWheels.RearWheelWidth = ValidateUShort(unvalidatedMetadata.MenuWheels.RearWheelWidth, nameof(unvalidatedMetadata.MenuWheels.RearWheelWidth));
            metadata.MenuWheels.FrontLeftXOffset = ValidateShort(unvalidatedMetadata.MenuWheels.FrontLeftXOffset, nameof(unvalidatedMetadata.MenuWheels.FrontLeftXOffset));
            metadata.MenuWheels.FrontRightXOffset = ValidateShort(unvalidatedMetadata.MenuWheels.FrontRightXOffset, nameof(unvalidatedMetadata.MenuWheels.FrontRightXOffset));
            metadata.MenuWheels.RearLeftXOffset = ValidateShort(unvalidatedMetadata.MenuWheels.RearLeftXOffset, nameof(unvalidatedMetadata.MenuWheels.RearLeftXOffset));
            metadata.MenuWheels.RearRightXOffset = ValidateShort(unvalidatedMetadata.MenuWheels.RearRightXOffset, nameof(unvalidatedMetadata.MenuWheels.RearRightXOffset));
            ValidateLOD(metadata.LOD0, unvalidatedMetadata.LOD0, nameof(unvalidatedMetadata.LOD0));
            ValidateLOD(metadata.LOD1, unvalidatedMetadata.LOD1, nameof(unvalidatedMetadata.LOD1));
            ValidateLOD(metadata.LOD2, unvalidatedMetadata.LOD2, nameof(unvalidatedMetadata.LOD2));
            metadata.Shadow.Scale = ValidateScale(unvalidatedMetadata.Shadow.Scale, $"{nameof(metadata.Shadow)}.{nameof(unvalidatedMetadata.Shadow.Scale)}");
            metadata.Shadow.ScaleRelatedMaybe = ValidateUShort(unvalidatedMetadata.Shadow.ScaleRelatedMaybe, $"{nameof(metadata.Shadow)}.{nameof(unvalidatedMetadata.Shadow.ScaleRelatedMaybe)}");
            metadata.Materials = unvalidatedMetadata.Materials.Select(ValidateMaterial).ToArray();
            return metadata;
        }

        private static void ValidateLOD(LODMetadata metadata, UnvalidatedLODMetadata unvalidatedMetadata, string lodName)
        {
            metadata.MaxDistance = ValidateUShort(unvalidatedMetadata.MaxDistance, $"{lodName}.{nameof(unvalidatedMetadata.MaxDistance)}");
            metadata.Scale = ValidateScale(unvalidatedMetadata.Scale, $"{lodName}.{nameof(unvalidatedMetadata.Scale)}");
            metadata.ScaleRelatedMaybe = ValidateUShort(unvalidatedMetadata.ScaleRelatedMaybe, $"{lodName}.{nameof(unvalidatedMetadata.ScaleRelatedMaybe)}");
        }

        private static MaterialMetadata ValidateMaterial(UnvalidatedMaterialMetadata unvalidatedMetadata)
        {
            if (string.IsNullOrWhiteSpace(unvalidatedMetadata.Name))
            {
                throw new Exception($"JSON error: A material name is missing or blank");
            }
            string materialName = unvalidatedMetadata.Name;
            ushort paletteIndex = 0;
            if (!unvalidatedMetadata.IsUntextured)
            {
                paletteIndex = ValidateUShort(unvalidatedMetadata.PaletteIndex, $"Material '{materialName}': {nameof(unvalidatedMetadata.PaletteIndex)}");
            }
            else if (unvalidatedMetadata.PaletteIndex != null)
            {
                throw new Exception($"JSON error: Material '{materialName}': An untextured material cannot have a {nameof(unvalidatedMetadata.PaletteIndex)} value");
            }
            return new MaterialMetadata
            {
                Name = materialName,
                IsUntextured = unvalidatedMetadata.IsUntextured,
                PaletteIndex = unvalidatedMetadata.IsUntextured ? null : paletteIndex,
                RenderOrder = ValidateUShort(unvalidatedMetadata.RenderOrder, $"Material '{materialName}': {nameof(unvalidatedMetadata.RenderOrder)}"),
                IsBrakeLight = unvalidatedMetadata.IsBrakeLight,
                IsMatte = unvalidatedMetadata.IsMatte
            };
        }

        private static ushort ValidateUShort(double? value, string fieldName) =>
            value is null ? throw new Exception($"JSON error: {fieldName} is missing")
                          : !double.IsInteger(value.Value) ? throw new Exception($"JSON error: {fieldName} must be a whole number")
                                                           : value < ushort.MinValue ? throw new Exception($"JSON error: {fieldName} is too small")
                                                                                     : value > ushort.MaxValue ? throw new Exception($"JSON error: {fieldName} is too large")
                                                                                                               : (ushort)value.Value;

        private static short ValidateShort(double? value, string fieldName) =>
            value is null ? throw new Exception($"JSON error: {fieldName} is missing")
                          : !double.IsInteger(value.Value) ? throw new Exception($"JSON error: {fieldName} must be a whole number")
                                                           : value < short.MinValue ? throw new Exception($"JSON error: {fieldName} is too small")
                                                                                    : value > short.MaxValue ? throw new Exception($"JSON error: {fieldName} is too large")
                                                                                                             : (short)value.Value;

        private static double ValidateScale(double? value, string fieldName) =>
            value is null or 0 ? throw new Exception($"JSON error: {fieldName} is missing or zero")
                               : value < 0 ? throw new Exception($"JSON error: {fieldName} cannot be negative")
                                           : value.Value;

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
                    ModelMetadata metadata = new() { ModelFilename = objFileName };
                    model.WriteToOBJ(modelWriter, materialWriter, filename, metadata);

                    using (StreamWriter jsonWriter = new(Path.Combine(path, $"{filename}.json")))
                    {
                        jsonWriter.Write(JsonSerializer.Serialize(metadata, serializerOptions));
                    }
                }
            }
        }
    }
}