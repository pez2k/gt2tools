namespace GT2BillboardEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string filename = args[0];
                if (filename.EndsWith(".tsd"))
                {
                    using (FileStream file = new(filename, FileMode.Open, FileAccess.Read))
                    {
                        CourseTIMs crsTims = new();
                        crsTims.ReadFromFile(file);
                        return;
                    }
                }
                else if (filename.EndsWith(".json"))
                {
                    using (FileStream file = new(".crstims.tsd", FileMode.Create, FileAccess.Write))
                    {
                        CourseTIMs crsTims = new();
                        crsTims.WriteToFile(file);
                        return;
                    }
                }
            }
            Console.WriteLine("Usage:\r\nExtract: GT2BillboardEditor .crstims.tsd\r\nBuild: GT2BillboardEditor Brands.json");
        }
    }
}
