using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ulong hash = GenerateHash(name);
            Console.WriteLine($"Hash             / Name");
            Console.WriteLine($"{hash:X16} / {name}");
        }

        public static ulong GenerateHash(string name)
        {
            ulong hash = 0;
            char[] nameChars = name.ToCharArray();

            foreach (char nameChar in nameChars)
            {
                hash += (byte)nameChar;
            }

            foreach (char nameChar in nameChars)
            {
                byte asciiValue = (byte)nameChar;
                ulong temp1 = hash << 7;
                ulong temp2 = hash >> 57;
                hash = temp1 | temp2;
                hash += asciiValue;
            }

            return hash;
        }
    }
}