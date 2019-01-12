using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT2.CarIDConverter
{
    using CarNameConversion;

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
            uint hexID;

            if (input.Length < 5)
            {
                Console.WriteLine("Unknown parameter.");
                return;
            }
            else if (input.Length > 5)
            {
                if (!uint.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out hexID) || hexID > "zzzzz".ToCarID())
                {
                    Console.WriteLine("Input is not a valid hex ID.");
                    return;
                }
                plainID = hexID.ToCarName();
            }
            else
            {
                plainID = input;
                hexID = plainID.ToCarID();
            }

            Console.WriteLine($"ID      Hex     ");
            Console.WriteLine($"{plainID} / {hexID:X8}");
        }
    }
}
