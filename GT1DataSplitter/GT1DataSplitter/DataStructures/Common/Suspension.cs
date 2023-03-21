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
            // 0x0D: f damper bound - kgf * 5
            // 0x0E: f damper rebound - kgf * 5
            // 0x0F: f ??? x  55 s0 42 s1  72 s3
            // 0x10: f ??? y 100 s0 72 s1 112 s3
            // 0x11: r camber
            // 0x12: r ride height
            // 0x13: r spring rate
            // 0x14: r damper bound - kgf * 5
            // 0x15: r damper rebound - kgf * 5
            // 0x16: r ??? x  55 s0 42 s1  72 s3
            // 0x17: r ??? y 100 s0 72 s1 112 s3
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
            // 0x22: f ??? x min  55 s0  18 s3
            // 0x23: f ??? x max  55 s0 180 s3
            // 0x24: f ??? y min 100 s0  24 s3
            // 0x25: f ??? y max 100 s0 222 s3
            // 0x26: f ???         1 s0   3 s1   5 s2  10 s3
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
            // 0x31: r ??? x min  55 s0  18 s3
            // 0x32: r ??? x max  55 s0 180 s3
            // 0x33: r ??? y min 100 s0  24 s3
            // 0x34: r ??? y max 100 s0 222 s3
            // 0x35: r ???         1 s0   3 s1   5 s2  10 s3
            // 0x36: car ID 2b
            // 0x37: stage
            // 0x38: isbuyabelmaybe
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