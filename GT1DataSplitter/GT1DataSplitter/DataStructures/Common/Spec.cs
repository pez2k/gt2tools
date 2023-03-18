using System.IO;
using System.Text;

namespace GT1.DataSplitter
{
    using Caches;

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
            // 0x2A: gear count
            // 0x2B: ??? - cannot be zero?
            // 0x2C: ??? - rpm related in some way??
            // 0x2D: turbo level?
            // 0x2E: f brake level?
            // 0x2F: r brake level?
            // 0x30: power multiplier?
            // 0x31-0x33: ??? idle / redline / max RPM? - if redline at 0x32 is zero, look up last tq curve point RPM using num of points at 0x56
            // 0x34-43: torque curve RPM?
            // 0x44-0x50: ??? - identical for most cars?
            // 0x51: AWD mode
            // 0x52-55: ??? - identical for most cars?
            // 0x56: num torque curve points
            // 0x57-59: ??? - identical for most cars?
            // 0x5A: weight
            // 0x5C: ??? - identical for most cars?
            // 0x5D: ??? - identical for most cars? multiplied by 10 in code, RPM related - ClutchReleaseRPM in GT2 (always 250 / 2500rpm?)
            // 0x5E: ??? - identical for most cars?
            // 0x5F: ??? - identical for most cars?
            // 0x60: brake power?
            // 0x61: ABS?
            // 0x62: ???
            // 0x63: brake ???
            // 0x64-65: ??? - related to wheel size or gearing in some way??
            // 0x66: f tyre size 3b?
            // 0x69: r tyre size 3b?
            // 0x6C: f camber
            // 0x6D: r camber
            // 0x6E: f spring
            // 0x6F: f stab
            // 0x70-72: ???
            // 0x73: f damper 1 ???
            // 0x75: f damper 3 ???
            // 0x77: f damper 2 ???
            // 0x79: f damper 4 ???
            // 0x7A: r spring
            // 0x7B: r stab
            // 0x7C-7E: ???
            // 0x7F: r damper 1 ???
            // 0x81: r damper 3 ???
            // 0x83: r damper 2 ???
            // 0x85: r damper 4 ???
            // 0x86-87: f/r grip?
            // 0x88: f df
            // 0x89: r df
            // 0x8A: drivetrain - 00 FR / 01 FF / 02 4WD / 03 MR
            // 0x8B: something drivetrain or RPM related?
            // 0x8C-115: ??? - 8F onwards the same for most cars?
            // 0x116-117: f/r ride height?
            // 0x118: ???
            // 0x119: something drivetrain related?
            // 0x11A-13D: ??? - 11A onwards identical for most cars?
            // 0x13E: torque curve 1 through
            // 0x15C: torque curve 16 - if last value is 0, is set to second-last value
            // 0x15E-175: ??? - identical for most cars?
            // 0x176: engine sound num?
            // 0x178: turbo related 6b? - 6 individual values
            // 0x17E: ??? - identical 0 for most cars?
            // 0x180: exhaust sound num?
            // 0x182: ??? - identical 0 for most cars?
            // 0x184: price
            // 0x188: name part 1
            // 0x18A: name part 1 table (always 0)
            // 0x18C: name part 2
            // 0x18E: name part 2 table (always 1)
            // 0x190: suspension part ID
            // 0x192: tire part ID
            // 0x194: length
            // 0x196: cc
            // 0x198: ps
            // 0x19A: power RPM
            // 0x19C: kgm
            // 0x19E: torque RPM
            // 0x1A0: 30 new / 31 used
            // 0x1A1: 01 racing / 00 normal - also checked by used car code maybe?
            // 0x1A2: unknown flag
            // 0x1A3: 00 normal / 01 turbo / 02 mechanical
            // 0x1A4: 00 SOHC / 01 DOHC / 02 OHV / 03 Rotary
            // 0x1A5: 00 L4 / 01 L6 / 02 V6 / 03 V8 / 04 V10 / 05 RE / 06 Boxer4 / 07 Boxer6
            // 0x1A6: f susp - 00 strut / 01 trailing torsion beam / 02 double wishbone / 03 multilink / 04 parallel-link strut? / 05 multilink beam? / 06 macpherson strut? / 07 semi-trailing arm? / 08 torque arm
            // 0x1A7: r susp
        }

        public override void Import(string filename)
        {
            base.Import(filename);
            string carID = Encoding.ASCII.GetString(rawData[..5]);
            CarIDCache.Add(carID);
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            string carID = Encoding.ASCII.GetString(rawData[..5]);
            CarIDCache.Add(carID);
            return filename.Replace(Path.GetExtension(filename), $"_{carID}{Path.GetExtension(filename)}");
        }
    }
}