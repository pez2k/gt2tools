using System;

namespace GT2.TrackIDConverter
{
    using TrackNameConversion;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid number of arguments.");
                return;
            }

            string input = args[0];
            string plainID;

            if (uint.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out uint hexID))
            {
                plainID = hexID.ToTrackName();
            }
            else
            {
                plainID = input;
                hexID = plainID.ToTrackID();
            }

            Console.WriteLine($"Hex      / ID");
            Console.WriteLine($"{hexID:X8} / {plainID}");
        }
    }
}
