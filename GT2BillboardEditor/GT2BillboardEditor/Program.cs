namespace GT2BillboardEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (FileStream file = new(".crstims.tsd", FileMode.Open, FileAccess.Read))
            {
                CourseTIMs crsTims = new();
                crsTims.ReadFromFile(file);
            }
        }
    }
}
