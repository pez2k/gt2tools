namespace GT2BillboardEditor
{
    internal class PoolMapping
    {
        public string Name { get; set; } = "";

        public Dictionary<string, ushort> WideBanners { get; set; } = [];

        public Dictionary<string, ushort> SquareLogos { get; set; } = [];

        public Dictionary<string, ushort> FlagBanners { get; set; } = [];

        public Dictionary<string, ushort> SmallBanners { get; set; } = [];

        public Dictionary<string, ushort> AltBanners { get; set; } = [];
    }
}
