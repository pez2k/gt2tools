﻿namespace GT1.DataSplitter
{
    public class BrakeController : DataStructure
    {
        public BrakeController()
        {
            Header = "BRKCTRL";
            Size = 0x1C;
            // 0xA: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0xA);
    }
}