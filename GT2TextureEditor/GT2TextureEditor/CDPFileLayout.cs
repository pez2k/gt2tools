namespace GT2.TextureEditor
{
    class CDPFileLayout: GameFileLayout
    {
        public override uint ColourCountIndex { get; } = 0;
        public override uint PaletteStartIndex { get; } = 0x20;
        public override uint BitmapStartIndex { get; } = 0x43A0;
    }
}
