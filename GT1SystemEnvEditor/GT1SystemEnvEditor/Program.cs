namespace GT1.SystemEnvEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            string filename = args[0];
            string defaultOutputType = "-e";

            SystemEnv data = new();
            if (File.GetAttributes(filename).HasFlag(FileAttributes.Directory))
            {
                data.ReadFromEditable(filename);
                filename = Path.GetFileName(filename) ?? throw new Exception();
                defaultOutputType = "-b";
            }
            else
            {
                data.ReadFromBinary(filename);
                filename = Path.GetFileNameWithoutExtension(filename);
            }

            string outputType = args.Length > 1 ? args[1] : defaultOutputType;
            if (outputType == "-t")
            {
                data.WriteToPlaintext($"{filename}.ENV");
            }
            else if (outputType == "-e")
            {
                data.WriteToEditable(filename);
            }
            else
            {
                data.WriteToBinary($"{filename}.DAT");
            }
        }
    }
}