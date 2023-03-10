﻿using System.IO;

namespace GT1.DataSplitter
{
    public class PortPolish : DataStructure
    {
        public PortPolish()
        {
            Header = "POLISH";
            Size = 0x20;
            // 0x10: car ID
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0x10]:X2}{Path.GetExtension(filename)}");
        }
    }
}