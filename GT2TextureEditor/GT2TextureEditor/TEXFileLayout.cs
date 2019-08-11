namespace GT2.TextureEditor
{
    class TEXFileLayout : GameFileLayout
    {
        public override byte[] HeaderData { get; } = new byte[] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x2D, 0x43, 0x54, 0x45, 0x58, 0x00, 0x02 }; // @(#)GT-CTEX\0\2
        public override uint ColourCountIndex { get; } = 0x0E;
        public override uint PaletteStartIndex { get; } = 0x8060;
        public override ushort PaletteSize { get; } = 0x200;
        public override uint BitmapStartIndex { get; } = 0x60;
        public override uint BitmapEmptyFillSize { get; } = 0x1000;
        public override uint SingleInstanceOfFlagsStartIndex { get; } = 0x20;
    }
}
