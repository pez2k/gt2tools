using System.IO;
using System.Text;

namespace GT1.DataSplitter
{
    public class Spec : DataStructure
    {
        public Spec()
        {
            Header = "SPEC";
            Size = 0x1A8;
            // 0x00: car ID string
            // 0x07: always 1?
            // 0x08: always 0x08D4?
            // 0x0A: always 0x08D4?
            // 0x0C: width
            // 0x0E: height
            // 0x10: wheelbase
            // 0x12: ???
            // 0x13: ???
            // 0x14: f track
            // 0x16: r track
            // 0x18: reverse
            // 0x1A: 1st through
            // 0x26: 7th
            // 0x28: fdr
            // 0x2A-2C: ???
            // 0x2D: turbo level?
            // 0x2E: f brake level?
            // 0x2F: r brake level?
            // 0x30: power multiplier?
            // 0x31-0x50: ???
            // 0x51: AWD mode
            // 0x52-59: ???
            // 0x5A: weight
            // 0x5C-5F: ???
            // 0x60: brake power?
            // 0x61: ABS?
            // 0x62: ???
            // 0x63: brake ???
            // 0x64-65: ???
            // 0x66: f tyre size 3b?
            // 0x69: r tyre size 3b?
            // 0x6C: f camber
            // 0x6D: r camber
            // 0x6E: f spring
            // 0x6F: f stab
            // 0x70-79: ???
            // 0x7A: r spring
            // 0x7B: r stab
            // 0x7C-87: ???
            // 0x88: f df
            // 0x89: r df
            // 0x8A-13D: ???
            // 0x13E: torque curve 1 through
            // 0x15C: torque curve 16
            // 0x15E-177: ???
            // 0x178: turbo related 6b?
            // 0x17E-183: ???
            // 0x184: price
            // 0x188-190: ???
            // 0x190: car number plus 478??
            // 0x192: car number plus 2??
            // 0x194: length
            // 0x196: cc
            // 0x198: ps
            // 0x19A: power RPM
            // 0x19C: kgm
            // 0x19E: torque RPM
            // 0x1A0: 30 new / 31 used
            // 0x1A1: 01 special dealer / 00 not
            // 0x1A2: unknown flag
            // 0x1A3: 00 NA / 01 turbo / 02 MA
            // 0x1A4: 00 SOHC / 01 DOHC / 02 OHV / 03 Rotary
            // 0x1A5: 00 L4 / 01 L6 / 02 V6 / 03 V8 / 04 V10 / 05 RE / 06 Boxer4 / 07 Boxer6
            // 0x1A6: f susp - 00 strut / 01 trailing torsion beam / 02 double wishbone / 03 multilink / 08 torque arm
            // 0x1A7: r susp
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{Encoding.ASCII.GetString(rawData[..5])}{Path.GetExtension(filename)}");
        }
    }
}