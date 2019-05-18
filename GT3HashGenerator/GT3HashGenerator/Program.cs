using System;

namespace GT3.HashGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid number of arguments.");
                return;
            }

            string name = args[0];
            ulong hash = HashGenerator.GenerateHash(name);
            Console.WriteLine($"Hash             / Name");
            Console.WriteLine($"{hash:X16} / {name}");
        }
    }
}