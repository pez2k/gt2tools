using System;

namespace GT3.CarColorEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                var data = new CarAndColourList();
                if (args[0].EndsWith("db"))
                {
                    data.ReadFromGameFiles();
                    data.WriteToCSV();
                    return;
                }
                else if (args[0].EndsWith(".csv"))
                {
                    data.ReadFromCSV();
                    data.WriteToGameFiles();
                    return;
                }
                else if (args[0] == "-wiki")
                {
                    data.ReadFromGameFiles();
                    data.WriteToWikiText();
                    return;
                }
            }
            Console.WriteLine("Usage: GT3CarColorEditor carcolor.db\r\nOR\r\nGT3CarColorEditor Cars.csv");
        }
    }
}