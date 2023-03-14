using System;
using System.Collections.Generic;

namespace GT1.DataSplitter.Caches
{
    public static class CarIDCache
    {
        public static List<string> Cache { get; set; } = new() { "" }; // Ensure that the index starts at 1 instead of 0

        public static void Add(string carIDString) => Cache.Add(carIDString);

        public static string Get(int carIDNumber) => Cache[carIDNumber];

        public static int Get(string carIDString) => Cache.Contains(carIDString) ? Cache.IndexOf(carIDString) : throw new Exception($"Car ID {carIDString} not mapped.");
    }
}
