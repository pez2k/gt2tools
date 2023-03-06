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
            string outputType = args.Length > 1 ? args[1] : "-e";

            SystemEnv data = new();
            data.ReadFromBinary(filename);

            if (outputType == "-t")
            {
                data.WriteToPlaintext(Path.ChangeExtension(filename, ".ENV"));
            }
            else if (outputType == "-e")
            {
                data.WriteToEditable(Path.GetFileNameWithoutExtension(filename));
            }
            else
            {
                data.WriteToBinary("new_SYSTEM.DAT");
            }
        }
    }
}