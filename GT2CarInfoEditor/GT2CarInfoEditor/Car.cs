using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT2CarInfoEditor
{
    class Car
    {
        public string JPName { get; set; }
        public string USName { get; set; }
        public string EUName { get; set; }
        public List<CarColour> Colours { get; set; }
        public uint Unknown { get; set; }
    }
}
