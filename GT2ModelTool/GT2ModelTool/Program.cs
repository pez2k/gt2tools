using System.IO;

namespace GT2.ModelTool
{
    using Structures;

    class Program
    {
        static void Main(string[] args)
        {
            if (true)
            {
                string gt1Model = "hn1lr";

                if (args.Length == 1 && args[0].EndsWith(".car")) {
                    gt1Model = args[0].Replace(".car", "");
                }
                
                using (FileStream stream = new FileStream($"{gt1Model}.car", FileMode.Open, FileAccess.Read))
                {
                    var model = new Model();
                    model.ReadFromCAR(stream);
                    Polygon.values.Sort();

                    using (FileStream outStream = new FileStream($"{gt1Model}.cdo", FileMode.Create, FileAccess.ReadWrite))
                    {
                        model.WriteToCDO(outStream);
                    }
                }
            }
            else
            {
                using (FileStream stream = new FileStream("bistn.cdo", FileMode.Open, FileAccess.Read))
                {
                    var model = new Model();
                    model.ReadFromCDO(stream);
                    Polygon.values.Sort();

                    using (FileStream outStream = new FileStream("out.cdo", FileMode.Create, FileAccess.ReadWrite))
                    {
                        model.WriteToCDO(outStream);
                    }
                }
            }
        }
    }
}
