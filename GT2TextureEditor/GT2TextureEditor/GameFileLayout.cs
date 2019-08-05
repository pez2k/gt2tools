namespace GT2.TextureEditor
{
    abstract class GameFileLayout
    {
        public abstract uint ColourCountIndex { get; }
        public abstract uint PaletteStartIndex { get; }
        public abstract uint BitmapStartIndex { get; }
    }
}
