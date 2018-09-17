using System.IO;

namespace GT2.ModelTool
{
    using Structures;

    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream stream = new FileStream("bistn.cdo", FileMode.Open, FileAccess.Read))
            {
                var model = new Model();
                model.ReadFromCDO(stream);
                Polygon.values.Sort();
            }
        }
    }
}
