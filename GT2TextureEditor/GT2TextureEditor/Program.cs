using System.IO;
using System.Linq;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class Program
    {
        private enum OutputType
        {
            EditableFiles,
            GT1,
            GT2
        }

        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                return;
            }

            string path = args.Last();
            bool isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            OutputType outputType = isDirectory ? OutputType.GT2 : OutputType.EditableFiles;

            if (args.Length == 2)
            {
                switch (args[0])
                {
                    case "-o1":
                        outputType = OutputType.GT1;
                        break;
                    case "-o2":
                        outputType = OutputType.GT2;
                        break;
                    case "-oe":
                        outputType = OutputType.EditableFiles;
                        break;
                    default:
                        return;
                }
            }

            CarTexture texture = isDirectory ? LoadFromEditableFiles(path) : LoadFromGameFile(path);
            if (outputType == OutputType.EditableFiles)
            {
                WriteToEditableFiles(path, texture);
            }
            else
            {
                WriteToGameFile(path, texture, outputType);
            }
        }

        static CarTexture LoadFromGameFile(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var texture = new CarTexture();
                texture.LoadFromGameFile(file, Path.GetExtension(filename) == ".tex" ? (GameFileLayout)new TEXFileLayout() : new CDPFileLayout());
                return texture;
            }
        }

        static void WriteToEditableFiles(string filename, CarTexture texture)
        {
            bool isNight = Path.GetExtension(filename) == ".cnp";
            string outputName = Path.GetFileNameWithoutExtension(filename);
            string outputPath = Path.Combine(Path.GetDirectoryName(filename), outputName + (isNight ? "_night" : ""));
            Directory.CreateDirectory(outputPath);
            using (var bitmap = new FileStream(Path.Combine(outputPath, $"{outputName}.bmp"), FileMode.Create, FileAccess.Write))
            {
                texture.WriteToEditableFiles(outputPath, bitmap);
            }
        }

        static CarTexture LoadFromEditableFiles(string directory)
        {
            string carName = Path.GetFileName(directory);
            string carNameNoSuffix = carName.Replace("_night", "");
            var texture = new CarTexture();
            using (var bitmap = new FileStream(Path.Combine(carName, $"{carNameNoSuffix}.bmp"), FileMode.Open, FileAccess.Read))
            {
                texture.LoadFromEditableFiles(directory, bitmap);
            }
            return texture;
        }

        static void WriteToGameFile(string directory, CarTexture texture, OutputType outputType)
        {
            GameFileLayout layout = outputType == OutputType.GT2 ? (GameFileLayout)new CDPFileLayout() : new TEXFileLayout();
            using (var file = new FileStream(CreateFilename(Path.GetFileNameWithoutExtension(directory), outputType), FileMode.Create, FileAccess.Write))
            {
                file.Write(layout.HeaderData);
                texture.WriteToGameFile(file, layout);
            }
        }

        static string CreateFilename(string carName, OutputType outputType)
        {
            if (outputType == OutputType.GT1)
            {
                return $"{carName}.tex";
            }
            bool isNight = carName.EndsWith("_night");
            string carNameNoSuffix = carName.Replace("_night", "");
            return $"{carNameNoSuffix}.c{(isNight ? "n" : "d")}p";
        }
    }
}
