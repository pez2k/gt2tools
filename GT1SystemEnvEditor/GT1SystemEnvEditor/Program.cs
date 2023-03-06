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
            string outputType = args.Length > 1 ? args[1] : "-t";

            SystemEnv data = new();

            using (FileStream file = new(filename, FileMode.Open, FileAccess.Read))
            {
                data.ReadFromBinary(file);
            }

            if (outputType == "-t")
            {
                using (FileStream output = new(Path.ChangeExtension(filename, ".ENV"), FileMode.Create, FileAccess.Write))
                {
                    data.WriteToPlaintext(output);
                }
            }
        }
    }
}