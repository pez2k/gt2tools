namespace GT2.TextureEditor
{
    abstract class GameFileLayout
    {
        public abstract byte[] HeaderData { get; }
        public abstract uint ColourCountIndex { get; }
        public abstract uint PaletteStartIndex { get; }
        public abstract ushort PaletteSize { get; }
        public abstract uint BitmapStartIndex { get; }
        public abstract uint BitmapEmptyFillSize { get; }
        public abstract uint SingleInstanceOfFlagsStartIndex { get; }
    }
}
