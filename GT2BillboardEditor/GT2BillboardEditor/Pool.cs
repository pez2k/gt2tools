using StreamExtensions;

namespace GT2BillboardEditor
{
    internal class Pool
    {
        public string Name { get; set; } = "";

        public Billboard[] WideBanners { get; set; }

        public Billboard[] SquareLogos { get; set; }

        public Billboard[] FlagBanners { get; set; }

        public Billboard[] SmallBanners { get; set; }

        public Billboard[] AltBanners { get; set; }

        public Pool(ushort wideBannerCount, ushort squareLogoCount, ushort flagBannerCount, ushort smallBannerCount, ushort altBannerCount)
        {
            WideBanners = new Billboard[wideBannerCount];
            SquareLogos = new Billboard[squareLogoCount];
            FlagBanners = new Billboard[flagBannerCount];
            SmallBanners = new Billboard[smallBannerCount];
            AltBanners = new Billboard[altBannerCount];
        }

        public void ReadFromFile(Stream file)
        {
            long position = file.Position + 0x10;
            Name = file.ReadCharacters();
            file.Position = position;

            for (int i = 0; i < WideBanners.Length; i++)
            {
                Billboard billboard = new();
                billboard.ReadFromFile(file);
                WideBanners[i] = billboard; // first 2b are some sort of brand ID? one entry per logo, last 2b are probability of appearance - MMRivit
            }
            for (int i = 0; i < SquareLogos.Length; i++)
            {
                Billboard billboard = new();
                billboard.ReadFromFile(file);
                SquareLogos[i] = billboard;
            }
            for (int i = 0; i < FlagBanners.Length; i++)
            {
                Billboard billboard = new();
                billboard.ReadFromFile(file);
                FlagBanners[i] = billboard;
            }
            for (int i = 0; i < SmallBanners.Length; i++)
            {
                Billboard billboard = new();
                billboard.ReadFromFile(file);
                SmallBanners[i] = billboard;
            }
            for (int i = 0; i < AltBanners.Length; i++)
            {
                Billboard billboard = new();
                billboard.ReadFromFile(file);
                AltBanners[i] = billboard;
            }
        }
    }
}
