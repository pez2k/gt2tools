using GT3.HashGenerator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT3HashBruteforce
{
    class Program
    {
        private const int MaxDepth = 25;
        private static readonly char[] characters = new char[] { ' ', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        static void Main(string[] args)
        {
            ulong target = 0x4860B134F122DD94;
            string found = TryCharacter(0, "", target);
            Console.WriteLine(found ?? "Not found");
        }

        private static string TryCharacter(int depth, string stub, ulong targetHash)
        {
            foreach (char character in characters)
            //for (byte character = 32; character <= 122; character++)
            {
                string newStub = (stub + (char)character).TrimStart(' ');

                if (depth == MaxDepth)
                {
                    ulong newHash = HashGenerator.GenerateHash(newStub);
                    if (newHash == targetHash)
                    {
                        return newStub;
                    }
                }
                else
                {
                    string attempt = TryCharacter(depth + 1, newStub, targetHash);
                    if (attempt != null)
                    {
                        return attempt;
                    }
                }
            }
            return null;
        }
    }
}