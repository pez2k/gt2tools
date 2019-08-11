namespace GT2.TextureEditor
{
    class CDPFileLayout: GameFileLayout
    {
        public override byte[] HeaderData { get; } = new byte[0];
        public override uint ColourCountIndex { get; } = 0;
        public override uint PaletteStartIndex { get; } = 0x20;
        public override ushort PaletteSize { get; } = 0x240;
        public override uint BitmapStartIndex { get; } = 0x43A0;
        public override uint BitmapEmptyFillSize { get; } = 0;
        public override uint SingleInstanceOfFlagsStartIndex { get; } = 0;
    }
}
