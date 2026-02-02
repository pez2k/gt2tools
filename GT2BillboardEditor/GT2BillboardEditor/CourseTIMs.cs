using System.Text.Json;
using System.Text.Json.Serialization;
using StreamExtensions;

namespace GT2BillboardEditor
{
    internal class CourseTIMs
    {
        private static readonly JsonSerializerOptions serializerOptions = new() { WriteIndented = true };
        private static readonly JsonSerializerOptions deserializerOptions = new()
        {
            AllowTrailingCommas = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
        private static readonly int[] sizes = [ 0x640, 0x840, 0x5E0, 0x4C0, 0x640 ];
        private static readonly string[] directories = [ "WideBanners", "SquareLogos", "FlagBanners", "SmallBanners", "AltBanners" ];

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
                PoolMapping poolMapping = new() { Name = pool.Name };
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
            ushort[] counts = [ wideBannerCount, squareLogoCount, flagBannerCount, smallBannerCount, altBannerCount ];

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

        public void WriteToFile(Stream file)
        {
            BrandMapping brands;
            using (StreamReader jsonReader = new("Brands.json"))
            {
                brands = JsonSerializer.Deserialize<BrandMapping>(jsonReader.ReadToEnd(), deserializerOptions) ?? throw new Exception("Could not read Brands.json");
            }

            ushort wideBannerCount = (ushort)brands.WideBanners.Count;
            ushort squareLogoCount = (ushort)brands.SquareLogos.Count;
            ushort flagBannerCount = (ushort)brands.FlagBanners.Count;
            ushort smallBannerCount = (ushort)brands.SmallBanners.Count;
            ushort altBannerCount = (ushort)brands.AltBanners.Count;

            List<PoolMapping> poolMappings;
            using (StreamReader jsonReader = new("Pools.json"))
            {
                poolMappings = JsonSerializer.Deserialize<List<PoolMapping>>(jsonReader.ReadToEnd(), deserializerOptions) ?? throw new Exception("Could not read Pools.json");
            }

            List<Pool> pools = new(poolMappings.Count);
            foreach (PoolMapping poolMapping in poolMappings)
            {
                pools.Add(new Pool(wideBannerCount, squareLogoCount, flagBannerCount, smallBannerCount, altBannerCount)
                {
                    Name = poolMapping.Name,
                    WideBanners = MapBillboards(brands.WideBanners, poolMapping.WideBanners),
                    SquareLogos = MapBillboards(brands.SquareLogos, poolMapping.SquareLogos),
                    FlagBanners = MapBillboards(brands.FlagBanners, poolMapping.FlagBanners),
                    SmallBanners = MapBillboards(brands.SmallBanners, poolMapping.SmallBanners),
                    AltBanners = MapBillboards(brands.AltBanners, poolMapping.AltBanners)
                });
            }

            file.WriteUShort((ushort)(wideBannerCount + squareLogoCount + flagBannerCount + smallBannerCount + altBannerCount));
            file.WriteUShort((ushort)pools.Count);
            file.WriteUShort(wideBannerCount);
            file.WriteUShort(squareLogoCount);
            file.WriteUShort(flagBannerCount);
            file.WriteUShort(smallBannerCount);
            file.WriteUShort(altBannerCount);
            file.WriteUShort(0);

            foreach (Pool pool in pools)
            {
                pool.WriteToFile(file);
            }

            Dictionary<string, ushort>[] mappings = [ brands.WideBanners, brands.SquareLogos, brands.FlagBanners, brands.SmallBanners, brands.AltBanners ];
            for (int i = 0; i < mappings.Length; i++)
            {
                LoadTIM(file, directories[i], "_default.tim", sizes[i]);
            }
            for (int i = 0; i < mappings.Length; i++)
            {
                foreach (string filename in mappings[i].Keys)
                {
                    LoadTIM(file, directories[i], filename, sizes[i]);
                }
            }
        }

        private static Billboard[] MapBillboards(Dictionary<string, ushort> brandMapping, Dictionary<string, ushort> chanceMapping)
        {
            List<Billboard> billboards = new(brandMapping.Count);
            foreach ((string name, ushort brand) in brandMapping)
            {
                if (!chanceMapping.TryGetValue(name, out ushort chance))
                {
                    chance = 0;
                }
                billboards.Add(new Billboard() { Brand = brand, Chance = chance });
            }
            return billboards.ToArray();
        }

        private static void LoadTIM(Stream file, string type, string filename, int expectedSize)
        {
            string path = Path.Combine(type, filename);
            if (!File.Exists(path))
            {
                throw new Exception($"Could not find file {path}");
            }
            using (FileStream tim = new(path, FileMode.Open, FileAccess.Read))
            {
                if (tim.Length != expectedSize)
                {
                    throw new Exception($"File {path} must be exactly {expectedSize} bytes");
                }
                tim.CopyTo(file);
            }
        }
    }
}
