using System.Text.Json;
using System.Text.Json.Serialization;
using StreamExtensions;

namespace GT2BillboardEditor
{
    internal class CourseTIMs
    {
        private static readonly JsonSerializerOptions serializerOptions = new() { WriteIndented = true };

        public Pool[] Pools { get; set; } = [];

        public void ReadFromFile(Stream file)
        {
            ushort billboardCount = file.ReadUShort();
            ushort poolCount = file.ReadUShort();
            ushort wideBannerCount = file.ReadUShort();
            ushort squareLogoCount = file.ReadUShort();
            ushort flagBannerCount = file.ReadUShort();
            ushort smallBannerCount = file.ReadUShort();
            ushort altBannerCount = file.ReadUShort();
            file.ReadUShort(); // padding?

            if (poolCount == 0)
            {
                throw new Exception("No banner pools found");
            }

            if (billboardCount != (wideBannerCount + squareLogoCount + flagBannerCount + smallBannerCount + altBannerCount))
            {
                throw new Exception("Total banner count does not match sum of other counts");
            }

            Pools = new Pool[poolCount];
            for (int i = 0; i < Pools.Length; i++)
            {
                Pool pool = new(wideBannerCount, squareLogoCount, flagBannerCount, smallBannerCount, altBannerCount);
                pool.ReadFromFile(file);
                Pools[i] = pool;
            }

            Pool template = Pools[0];
            BrandMapping brandMapping = new();
            for (int i = 0; i < template.WideBanners.Length; i++)
            {
                brandMapping.WideBanners.Add($"{i:D2}.tim", template.WideBanners[i].Brand);
            }
            for (int i = 0; i < template.SquareLogos.Length; i++)
            {
                brandMapping.SquareLogos.Add($"{i:D2}.tim", template.SquareLogos[i].Brand);
            }
            for (int i = 0; i < template.FlagBanners.Length; i++)
            {
                brandMapping.FlagBanners.Add($"{i:D2}.tim", template.FlagBanners[i].Brand);
            }
            for (int i = 0; i < template.SmallBanners.Length; i++)
            {
                brandMapping.SmallBanners.Add($"{i:D2}.tim", template.SmallBanners[i].Brand);
            }
            for (int i = 0; i < template.AltBanners.Length; i++)
            {
                brandMapping.AltBanners.Add($"{i:D2}.tim", template.AltBanners[i].Brand);
            }

            using (StreamWriter jsonWriter = new("Brands.json"))
            {
                jsonWriter.Write(JsonSerializer.Serialize(brandMapping, serializerOptions));
            }

            List<PoolMapping> poolMappings = new(poolCount);

            foreach (Pool pool in Pools)
            {
                PoolMapping poolMapping = new();
                poolMapping.Name = pool.Name;
                for (int i = 0; i < pool.WideBanners.Length; i++)
                {
                    if (pool.WideBanners[i].Chance > 0)
                    {
                        poolMapping.WideBanners.Add($"{i:D2}.tim", pool.WideBanners[i].Chance);
                    }
                }
                for (int i = 0; i < pool.SquareLogos.Length; i++)
                {
                    if (pool.SquareLogos[i].Chance > 0)
                    {
                        poolMapping.SquareLogos.Add($"{i:D2}.tim", pool.SquareLogos[i].Chance);
                    }
                }
                for (int i = 0; i < pool.FlagBanners.Length; i++)
                {
                    if (pool.FlagBanners[i].Chance > 0)
                    {
                        poolMapping.FlagBanners.Add($"{i:D2}.tim", pool.FlagBanners[i].Chance);
                    }
                }
                for (int i = 0; i < pool.SmallBanners.Length; i++)
                {
                    if (pool.SmallBanners[i].Chance > 0)
                    {
                        poolMapping.SmallBanners.Add($"{i:D2}.tim", pool.SmallBanners[i].Chance);
                    }
                }
                for (int i = 0; i < pool.AltBanners.Length; i++)
                {
                    if (pool.AltBanners[i].Chance > 0)
                    {
                        poolMapping.AltBanners.Add($"{i:D2}.tim", pool.AltBanners[i].Chance);
                    }
                }
                poolMappings.Add(poolMapping);
            }

            using (StreamWriter jsonWriter = new("Pools.json"))
            {
                jsonWriter.Write(JsonSerializer.Serialize(poolMappings, serializerOptions));
            }

            // read TIMs - one each of each size as defaults, then wideBannerCount of 0x640 banners etc
            int[] sizes = [ 0x640, 0x840, 0x5E0, 0x4C0, 0x640 ];
            ushort[] counts = [ wideBannerCount, squareLogoCount, flagBannerCount, smallBannerCount, altBannerCount ];
            string[] directories = [ "WideBanners", "SquareLogos", "FlagBanners", "SmallBanners", "AltBanners" ];

            for (int i = 0; i < sizes.Length; i++)
            {
                Directory.CreateDirectory(directories[i]);
                byte[] buffer = new byte[sizes[i]];
                file.Read(buffer);

                using (FileStream tim = new(Path.Combine(directories[i], "_default.tim"), FileMode.Create, FileAccess.Write))
                {
                    tim.Write(buffer);
                }
            }

            for (int i = 0; i < sizes.Length; i++)
            {
                for (int j = 0; j < counts[i]; j++)
                {
                    byte[] buffer = new byte[sizes[i]];
                    file.Read(buffer);

                    using (FileStream tim = new(Path.Combine(directories[i], $"{j:D2}.tim"), FileMode.Create, FileAccess.Write))
                    {
                        tim.Write(buffer);
                    }
                }
            }
        }
    }
}
