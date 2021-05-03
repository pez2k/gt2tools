using System.IO;

namespace GT2.ModelTool
{
    using Structures;

    class Program
    {
        private static int mode = 2;

        static void Main(string[] args)
        {
            switch (mode)
            {
                case 0:
                    ReadGT1WriteGT2(args);
                    break;
                case 1:
                    ReadGT2WriteGT2(args);
                    break;
                case 2:
                    ReadGT2WriteOBJ(args);
                    break;
            }
        }

        private static void ReadGT1WriteGT2(string[] args)
        {
            //string gt1Model = "hn1lr";
            string gt1Model = "n-15r";
            //string gt1Model = "t-hvr";
            //string gt1Model = "ttror"; // broken normals
            //string gt1Model = "cc67n"; // incorrect verts or faces?

            if (args.Length == 1 && args[0].EndsWith(".car"))
            {
                gt1Model = args[0].Replace(".car", "");
            }

            using (FileStream stream = new FileStream($"{gt1Model}.car", FileMode.Open, FileAccess.Read))
            {
                var model = new Model();
                model.ReadFromCAR(stream);
                //Polygon.values.Sort();

                using (FileStream outStream = new FileStream($"{gt1Model}.cdo", FileMode.Create, FileAccess.ReadWrite))
                {
                    model.WriteToCDO(outStream);
                }
            }
        }

        private static void ReadGT2WriteGT2(string[] args)
        {
            //string gt2Model = "bistn.cdo";
            //string gt2Model = "ga4tn.cdo";
            string gt2Model = "n-15r.cno";
            using (FileStream stream = new FileStream(gt2Model, FileMode.Open, FileAccess.Read))
            {
                var model = new Model();
                model.ReadFromCDO(stream);
                //Polygon.values.Sort();

                using (FileStream outStream = new FileStream("out.cdo", FileMode.Create, FileAccess.ReadWrite))
                {
                    model.WriteToCDO(outStream);
                }
            }
        }

        private static void ReadGT2WriteOBJ(string[] args)
        {
            string gt2Model = "bistn.cdo";
            using (FileStream stream = new FileStream(gt2Model, FileMode.Open, FileAccess.Read))
            {
                var model = new Model();
                model.ReadFromCDO(stream);
                //Polygon.values.Sort();

                using (TextWriter writer = new StreamWriter("out.obj"))
                {
                    model.WriteToOBJ(writer);
                }
            }
        }
    }
}
