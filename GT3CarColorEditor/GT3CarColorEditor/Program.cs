namespace GT3.CarColorEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new CarAndColourList();
            data.ReadFromGameFiles();
            data.WriteToCSV();
        }
    }
}