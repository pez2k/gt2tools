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
                using (FileStream stream = new FileStream("t-hvr.car", FileMode.Open, FileAccess.Read))
                {
                    var model = new Model();
                    model.ReadFromCAR(stream);
                    Polygon.values.Sort();

                    using (FileStream outStream = new FileStream("t-hvr.cdo", FileMode.Create, FileAccess.ReadWrite))
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
