namespace GT1.DataSplitter
{
    public class Suspension : DataStructure
    {
        public Suspension()
        {
            Header = "SUSPENS";
            Size = 0x48;
            // 0x00: f ??? 30 - doesn't change per level - or per car?
            // 0x01: f ??? 30 - doesn't change per level - or per car?
            // 0x02: f ??? 70 - doesn't change per level - or per car?
            // 0x03: f ??? 70 - doesn't change per level - or per car?
            // 0x04: f bumprubber
            // 0x05: r ??? 30 - doesn't change per level - or per car?
            // 0x06: r ??? 30 - doesn't change per level - or per car?
            // 0x07: r ??? 70 - doesn't change per level - or per car?
            // 0x08: r ??? 70 - doesn't change per level - or per car?
            // 0x09: r bumprubber
            // 0x0A: f camber
            // 0x0B: f ride height
            // 0x0C: f spring rate
            // 0x0D: f damper bound 1 - kgf * 5
            // 0x0E: f damper rebound 1 - kgf * 5
            // 0x0F: f damper bound 2?
            // 0x10: f damper rebound 2?
            // 0x11: r camber
            // 0x12: r ride height
            // 0x13: r spring rate
            // 0x14: r damper bound - kgf * 5
            // 0x15: r damper rebound - kgf * 5
            // 0x16: r damper bound 2?
            // 0x17: r damper rebound 2?
            // 0x18: f camber min
            // 0x19: f camber max
            // 0x1A: f ride height min
            // 0x1B: f ride height max
            // 0x1C: f spring rate min
            // 0x1D: f spring rate max
            // 0x1E: f damper bound min
            // 0x1F: f damper bound max
            // 0x20: f damper rebound min
            // 0x21: f damper rebound max
            // 0x22: f damper bound 2? min
            // 0x23: f damper bound 2? max
            // 0x24: f damper rebound 2? min
            // 0x25: f damper rebound 2? max
            // 0x26: f damper steps
            // 0x27: r camber min
            // 0x28: r camber max
            // 0x29: r ride height min
            // 0x2A: r ride height max
            // 0x2B: r spring rate min
            // 0x2C: r spring rate max
            // 0x2D: r damper bound min
            // 0x2E: r damper bound max
            // 0x2F: r damper rebound min
            // 0x30: r damper rebound max
            // 0x31: r damper bound 2? min
            // 0x32: r damper bound 2? max
            // 0x33: r damper rebound 2? min
            // 0x34: r damper rebound 2? max
            // 0x35: r damper steps
            // 0x36: car ID 2b
            // 0x37: stage
            // 0x38: stage duplicate
            // 0x39: padding 2b
            // 0x3C: price 4b
            // 0x40: name 1
            // 0x42: name table 1
            // 0x44: name 2
            // 0x46: name table 2
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x36);
    }
}