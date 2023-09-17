using StreamExtensions;
using System.Numerics;
using System.Text;

namespace GT1.UsedCarEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] manufacturers = new string[] { "Toyota", "Nissan", "Mitsubishi", "Honda", "Mazda", "Subaru" };

            using (FileStream file = new("_unknown0004.usedcar", FileMode.Open, FileAccess.Read))
            {
                using (TextWriter output = new StreamWriter("dump.txt"))
                {
                    file.Position = 0x10; // skip header
                    const int WeekCount = 60;

                    uint[] blockPointers = new uint[WeekCount];
                    for (int i = 0; i < WeekCount; i++)
                    {
                        blockPointers[i] = file.ReadUInt();
                    }

                    //output.WriteLine("Week,Manufacturer,Car,Colour,Price");
                    for (int i = 0; i < WeekCount; i++)
                    {
                        output.WriteLine($"==<span class=\"mw-customtoggle-{i}\" style=\"font-size:small; display:inline-block; float:right;\"><span class=\"mw-customtoggletext\">[Show/Hide]</span></span> Days {(i * 10) + 1} to {(i * 10) + 10}==\r\n<div id=\"mw-customcollapsible-{i}\" class=\"mw-collapsible mw-collapsed\">");
                        //output.WriteLine($"Days {(i * 10) + 1} to {(i * 10) + 10}");
                        file.Position = blockPointers[i];
                        //output.WriteLine($"Week {i}");
                        //output.WriteLine();

                        foreach (string manufacturer in manufacturers)
                        {
                            //output.WriteLine(manufacturer);
                            output.WriteLine($"==={manufacturer}===");
                            output.WriteLine("{| class=\"wikitable\" + style=\"text-align: center;\" width=\"100%;\"\r\n|-\r\n!Car Name\r\n!Color\r\n!Price");
                            uint carCount = file.ReadUInt();
                            //output.WriteLine($"{carCount} cars");
                            //output.WriteLine();

                            for (int j = 0; j < carCount; j++)
                            {
                                ushort price = file.ReadUShort();
                                byte carID = file.ReadSingleByte();
                                byte colourID = file.ReadSingleByte();
                                //output.WriteLine($"{price * 10} Cr / {CarIDs[carID]} / {Colours.Where(col => col.car == carID + 1 && col.colour == colourID / 2).SingleOrDefault().name ?? Colours.FirstOrDefault(col => col.car == carID + 1).name ?? "<unknown>"}");
                                //output.WriteLine($"{i},{manufacturer},{carNames[CarIDs[carID]]},{Colours.SingleOrDefault(col => col.car == carID + 1 && col.colour == colourID / 2).name ?? Colours.FirstOrDefault(col => col.car == carID + 1).name ?? $"<unknown>"},{price * 10}");
                                output.WriteLine("|-");
                                output.WriteLine($"|{carNames[CarIDs[carID]]}||{Colours.SingleOrDefault(col => col.car == carID + 1 && col.colour == colourID / 2).name ?? Colours.FirstOrDefault(col => col.car == carID + 1).name}||{price * 10:###,##0} Cr");
                            }
                            output.WriteLine("|}");
                            output.WriteLine();
                        }
                        //output.WriteLine();
                        //output.WriteLine();
                        output.WriteLine("</div>");
                    }
                }
            }
        }

        private static readonly string[] CarIDs = new string[]
        {
            "tstln",
            "tlevn",
            "ttrnn",
            "texvn",
            "tceln",
            "tcegn",
            "tm2vn",
            "tm2sn",
            "tchvn",
            "tchsn",
            "tsoon",
            "tsoan",
            "tmrln",
            "tmr2n",
            "tsorn",
            "tsonn",
            "tsprn",
            "tspnn",
            "tsgtn",
            "tspon",
            "tlvon",
            "ttron",
            "tsplr",
            "t-ron",
            "t-hvr",
            "t-eln",
            "t-oan",
            "t-pnn",
            "t-rdn",
            "t-plr",
            "nzxvn",
            "nzx2n",
            "nzx3n",
            "nzxsn",
            "nr02n",
            "nr32n",
            "nv12n",
            "nv22n",
            "nn32n",
            "nt32n",
            "nm32n",
            "n432n",
            "nt33n",
            "nr33n",
            "nv33n",
            "nr34n",
            "nv34n",
            "nq15n",
            "ns15n",
            "nq14n",
            "ns14n",
            "nq23n",
            "ns23n",
            "nq13n",
            "ns13n",
            "npron",
            "nprin",
            "n180n",
            "n18nn",
            "n18sn",
            "nplon",
            "nl33r",
            "n-32n",
            "n-00n",
            "n-33n",
            "n-13n",
            "n-15r",
            "n-18n",
            "mgoon",
            "mgotn",
            "mgton",
            "mgttn",
            "mgtmn",
            "mgnon",
            "mgntn",
            "mgagn",
            "mgaln",
            "mecln",
            "mftrn",
            "mfton",
            "mftgn",
            "mftxn",
            "mftnn",
            "mlnon",
            "mlnnn",
            "mmgrn",
            "mmgon",
            "mgtlr",
            "m-tor",
            "m-nnn",
            "hnsxn",
            "hnsrn",
            "hnsbn",
            "hnssn",
            "hnsnn",
            "hpren",
            "hprvn",
            "hpnen",
            "hpnvn",
            "hinsn",
            "hinrn",
            "hcfnn",
            "hcvnn",
            "hcvrn",
            "hdeln",
            "hdern",
            "hdegn",
            "hdesn",
            "hcvon",
            "hcfon",
            "hcrxn",
            "hacsn",
            "hacwn",
            "hnslr",
            "h-err",
            "h-rxn",
            "h-vrn",
            "acosn",
            "acoen",
            "altsn",
            "an16n",
            "av16n",
            "as16n",
            "aminn",
            "amivn",
            "amisn",
            "afo7n",
            "afd7n",
            "afb7n",
            "afx7n",
            "afa7n",
            "afr7n",
            "afc7n",
            "ademn",
            "adgln",
            "adlxn",
            "afn7r",
            "a-ian",
            "a-emn",
            "a-a7r",
            "ssvxn",
            "ssv4n",
            "slgbn",
            "slgan",
            "slgnn",
            "slgwn",
            "siprn",
            "sipan",
            "sipbn",
            "sipcn",
            "sipdn",
            "sipzn",
            "sipsn",
            "sipwn",
            "siptn",
            "sipxr",
            "s-v4n",
            "s-pbn",
            "ld7cn",
            "ld7vn",
            "l-7cn",
            "dvprn",
            "dvpgn",
            "dcphn",
            "d-pgr",
            "d-phr",
            "ccrvn",
            "ccrcn",
            "ccamn",
            "c-30n",
            "vcrbn",
            "vgr5n",
            "vgrfn",
            "v-rbr",
            "amian"
        };

        private static readonly (byte car, byte colour, string name)[] Colours = new (byte car, byte colour, string name)[]
        {
(1, 0x31, "SUPER WHITE II"
), (1, 0x34, "BLUISH SILVER METALLIC"
), (1, 0x36, "BLACK METALLIC"
), (1, 0x62, "SUPER RED II"
), (1, 0x71, "PURPLISH BLUE MICA METALLIC"
), (2, 0x31, "SUPER WHITE II"
), (2, 0x34, "SILVER PEARL METALLIC"
), (2, 0x36, "BLACK METALLIC"
), (2, 0x65, "RED MICA METALLIC"
), (2, 0x6D, "MEDIUM GREEN MICA METALLIC"
), (2, 0x73, "DARK BLUE MICA"
), (3, 0x31, "SUPER WHITE II"
), (3, 0x34, "SILVER PEARL METALLIC"
), (3, 0x36, "BLACK METALLIC"
), (3, 0x65, "RED MICA METALLIC"
), (3, 0x6D, "MEDIUM GREEN MICA METALLIC"
), (3, 0x73, "DARK BLUE MICA"
), (4, 0x31, "WHITE PEARL MICA"
), (4, 0x34, "SILVER METALLIC"
), (4, 0x36, "BLACK"
), (4, 0x62, "RED MICA METALLIC"
), (4, 0x6C, "DARK GREEN MICA "
), (4, 0x71, "DARK BLUE MICA METALLIC"
), (5, 0x31, "SUPER WHITEII"
), (5, 0x34, "SILVER METALLIC"
), (5, 0x35, "DARK BLUISH GRAY METALLIC"
), (5, 0x36, "BLACK"
), (5, 0x62, "SUPER RED IV"
), (5, 0x6D, "DARK GREEN MICA "
), (6, 0x31, "SUPER WHITEII"
), (6, 0x34, "SILVER METALLIC"
), (6, 0x35, "DARK BLUISH GRAY METALLIC"
), (6, 0x36, "BLACK"
), (6, 0x62, "SUPER RED IV"
), (6, 0x6D, "DARK GREEN MICA "
), (7, 0x31, "SUPER WHITEII"
), (7, 0x34, "SILVER METALLIC"
), (7, 0x61, "WARM GRAY PEARL MICA"
), (7, 0x6D, "DARK GREEN M.I.O"
), (8, 0x31, "SUPER WHITEII"
), (8, 0x34, "SILVER METALLIC"
), (8, 0x61, "WARM GRAY PEARL MICA"
), (8, 0x6D, "DARK GREEN M.I.O"
), (9, 0x31, "SUPER WHITEII"
), (9, 0x61, "WHITE PEARL MICA"
), (9, 0x34, "SILVER METALLIC"
), (9, 0x65, "WINE RED MICA"
), (9, 0x35, "GRAYISH GREEN MICA METALLIC"
), (9, 0x71, "DARK BLUE MICA"
), (9, 0x6D, "DARK GREEN MICA P.I.O"
), (10, 0x31, "SUPER WHITEII"
), (10, 0x61, "WHITE PEARL MICA"
), (10, 0x34, "SILVER METALLIC"
), (10, 0x65, "WINE RED MICA"
), (10, 0x35, "GRAYISH GREEN MICA METALLIC"
), (10, 0x71, "DARK BLUE MICA"
), (10, 0x6D, "DARK GREEN MICA P.I.O"
), (11, 0x31, "SUPER WHITE PEARL MICA"
), (11, 0x34, "BLUISH SILVER METALLIC"
), (11, 0x36, "BLACK"
), (11, 0x62, "SUPER RED "
), (11, 0x65, "WINE RED MICA"
), (11, 0x6D, "BLUISH GREEN M.I.O"
), (11, 0x71, "DARK BLUE MICA PHTALOCYANINE"
), (12, 0x31, "SUPER WHITE PEARL MICA"
), (12, 0x34, "BLUISH SILVER METALLIC"
), (12, 0x36, "BLACK"
), (12, 0x62, "SUPER RED IV"
), (12, 0x65, "WINE RED MICA"
), (12, 0x6D, "DARK GREEN MICA METALLIC"
), (12, 0x71, "BLUE MICA METALLIC"
), (13, 0x31, "SUPER WHITEII"
), (13, 0x34, "SONIC SHADOW TONING"
), (13, 0x36, "BLACK"
), (13, 0x62, "SUPER RED II"
), (13, 0x68, "SUPER BRIGHT YELLOW"
), (13, 0x6D, "DARK GREEN MICA "
), (13, 0x72, "PURPLISH BLUE MICA METALLIC"
), (14, 0x31, "SUPER WHITEII"
), (14, 0x34, "BLUISH SILVER TONING"
), (14, 0x36, "BLACK"
), (14, 0x62, "SUPER RED II"
), (14, 0x68, "SUPER BRIGHT YELLOW"
), (14, 0x6D, "DARK GREEN MICA "
), (14, 0x72, "PURPLISH BLUE MICA METALLIC"
), (15, 0x31, "SUPER WHITEII"
), (15, 0x34, "SILVER METALLIC"
), (15, 0x36, "BLACK"
), (15, 0x62, "SUPER RED IV"
), (15, 0x6D, "DARK GREEN MICA METALLIC"
), (15, 0x71, "DEEP TEAL MICA METALLIC"
), (16, 0x31, "SUPER WHITEII"
), (16, 0x34, "SILVER METALLIC"
), (16, 0x36, "BLACK"
), (16, 0x62, "SUPER RED IV"
), (16, 0x6D, "DARK GREEN MICA METALLIC"
), (16, 0x71, "DEEP TEAL MICA METALLIC"
), (17, 0x31, "SUPER WHITEII"
), (17, 0x34, "SILVER METALLIC GRAPHITE"
), (17, 0x35, "GRAYISH GREEN MICA METALLIC"
), (17, 0x36, "BLACK"
), (17, 0x62, "SUPER RED IV"
), (17, 0x6D, "DARK GREEN MICA METALLIC"
), (17, 0x71, "BLUE MICA METALLIC"
), (18, 0x31, "SUPER WHITEII"
), (18, 0x34, "SILVER METALLIC GRAPHITE"
), (18, 0x35, "GRAYISH GREEN MICA METALLIC"
), (18, 0x36, "BLACK"
), (18, 0x62, "SUPER RED IV"
), (18, 0x6D, "DARK GREEN MICA METALLIC"
), (18, 0x71, "BLUE MICA METALLIC"
), (19, 0x31, "SUPER WHITE III"
), (19, 0x32, "WHITE PEARL MICA"
), (19, 0x35, "GRAY METALLIC"
), (19, 0x36, "BLACK MICA"
), (19, 0x62, "RED MICA"
), (19, 0x71, "BLUE MICA"
), (20, 0x31, "SUPER WHITE IV"
), (20, 0x32, "SUPER WHITE PEARL MICA "
), (20, 0x36, "BLACK"
), (20, 0x62, "RED MICA"
), (20, 0x6C, "DARK GREEN MICA "
), (20, 0x71, "BLUE METALLIC"
), (21, 0x31, "HIGH-TECH TWO-TONE"
), (21, 0x36, "HIGH SONIC TONING"
), (21, 0x62, "HIGH FLASH TWO-TONE"
), (22, 0x31, "HIGH-TECH TWO-TONE"
), (22, 0x36, "HIGH SONIC TONING"
), (22, 0x62, "HIGH FLASH TWO-TONE"
), (31, 0x31, "WHITE PEARL"
), (31, 0x34, "SILVER METALLIC"
), (31, 0x36, "BLACK"
), (31, 0x62, "RED"
), (31, 0x6D, "GREEN PEARL"
), (31, 0x73, "DARK BLUE GRAPHITE PEARL"
), (31, 0x77, "MIDNIGHT PURPLE PEARL"
), (32, 0x31, "WHITE PEARL"
), (32, 0x34, "SILVER METALLIC"
), (32, 0x36, "BLACK"
), (32, 0x62, "RED"
), (32, 0x6D, "GREEN PEARL"
), (32, 0x73, "DARK BLUE GRAPHITE PEARL"
), (32, 0x77, "MIDNIGHT PURPLE PEARL"
), (33, 0x31, "WHITE PEARL"
), (33, 0x34, "SILVER METALLIC"
), (33, 0x36, "BLACK"
), (33, 0x62, "RED"
), (33, 0x6D, "GREEN PEARL"
), (33, 0x73, "DARK BLUE GRAPHITE PEARL"
), (33, 0x77, "MIDNIGHT PURPLE PEARL"
), (34, 0x31, "WHITE PEARL"
), (34, 0x34, "SILVER METALLIC"
), (34, 0x36, "BLACK"
), (34, 0x62, "RED"
), (34, 0x6D, "GREEN PEARL"
), (34, 0x73, "DARK BLUE GRAPHITE PEARL"
), (34, 0x77, "MIDNIGHT PURPLE PEARL"
), (35, 0x34, "JET SILVER METALLIC"
), (35, 0x35, "GUN GRAY METALLIC"
), (35, 0x36, "BLACK PEARL METALLIC"
), (35, 0x62, "RED PEARL METALLIC"
), (35, 0x73, "DARK BLUE PEARL"
), (36, 0x31, "CRYSTAL WHITE"
), (36, 0x34, "SPARK SILVER METALLIC"
), (36, 0x35, "GUN GRAY METALLIC"
), (36, 0x36, "BLACK PEARL METALLIC"
), (36, 0x62, "RED PEARL"
), (36, 0x6F, "GRAYISH BLUE PEARL"
), (36, 0x73, "DARK BLUE PEARL"
), (37, 0x31, "CRYSTAL WHITE"
), (37, 0x34, "SPARK SILVER METALLIC"
), (37, 0x35, "GUN GRAY METALLIC"
), (37, 0x36, "BLACK PEARL METALLIC"
), (37, 0x62, "RED PEARL"
), (37, 0x6F, "GRAYISH BLUE PEARL"
), (37, 0x73, "DARK BLUE PEARL"
), (38, 0x31, "CRYSTAL WHITE"
), (38, 0x34, "SPARK SILVER METALLIC"
), (38, 0x35, "GUN GRAY METALLIC"
), (38, 0x36, "BLACK PEARL METALLIC"
), (38, 0x62, "RED PEARL"
), (38, 0x6F, "GRAYISH BLUE PEARL"
), (38, 0x73, "DARK BLUE PEARL"
), (39, 0x35, "GUN GRAY METALLIC"
), (40, 0x31, "CRYSTAL WHITE"
), (40, 0x32, "WHITE PEARL"
), (40, 0x34, "SPARK SILVER METALLIC"
), (40, 0x35, "GUN GRAY METALLIC"
), (40, 0x36, "BLACK PEARL METALLIC"
), (40, 0x62, "RED PEARL"
), (40, 0x6F, "GRAYISH BLUE PEARL"
), (40, 0x73, "DARK BLUE PEARL"
), (41, 0x31, "CRYSTAL WHITE"
), (41, 0x34, "SPARK SILVER METALLIC"
), (41, 0x35, "GUN GRAY METALLIC"
), (41, 0x36, "BLACK PEARL METALLIC"
), (41, 0x62, "RED PEARL"
), (41, 0x6F, "GRAYISH BLUE PEARL"
), (41, 0x73, "DARK BLUE PEARL"
), (42, 0x31, "CRYSTAL WHITE"
), (42, 0x34, "SPARK SILVER METALLIC"
), (42, 0x35, "GUN GRAY METALLIC"
), (42, 0x36, "BLACK PEARL METALLIC"
), (42, 0x62, "RED PEARL"
), (42, 0x6F, "GRAYISH BLUE PEARL"
), (42, 0x73, "DARK BLUE PEARL"
), (43, 0x31, "WHITE"
), (43, 0x34, "SONIC SILVER METALLIC"
), (43, 0x35, "DARK GRAY PEARL"
), (43, 0x36, "BLACK"
), (43, 0x62, "SUPER CLEAR REDII"
), (43, 0x73, "DEEP MARINE BLUE PEARL"
), (44, 0x31, "WHITE"
), (44, 0x34, "SONIC SILVER METALLIC"
), (44, 0x35, "DARK GRAY PEARL"
), (44, 0x36, "BLACK"
), (44, 0x62, "SUPER CLEAR REDII"
), (44, 0x73, "DEEP MARINE BLUE PEARL"
), (44, 0x77, "MIDNIGHT PURPLE PEARL"
), (45, 0x31, "WHITE"
), (45, 0x34, "SONIC SILVER METALLIC"
), (45, 0x35, "DARK GRAY PEARL"
), (45, 0x36, "BLACK"
), (45, 0x62, "SUPER CLEAR REDII"
), (45, 0x73, "DEEP MARINE BLUE PEARL"
), (45, 0x77, "MIDNIGHT PURPLE PEARL"
), (46, 0x31, "WHITE"
), (46, 0x34, "SONIC SILVER METALLIC"
), (46, 0x35, "DARK GRAY PEARL"
), (46, 0x36, "BLACK"
), (46, 0x62, "SUPER CLEAR REDII"
), (46, 0x73, "DEEP MARINE BLUE PEARL"
), (46, 0x77, "MIDNIGHT PURPLE PEARL"
), (47, 0x31, "WHITE"
), (47, 0x34, "SONIC SILVER METALLIC"
), (47, 0x35, "DARK GRAY PEARL"
), (47, 0x36, "BLACK"
), (47, 0x62, "SUPER CLEAR REDII"
), (47, 0x73, "DEEP MARINE BLUE PEARL"
), (47, 0x77, "MIDNIGHT PURPLE PEARL"
), (48, 0x31, "PEARL WHITE"
), (48, 0x34, "CLEAR SILVER"
), (48, 0x36, "SUPER BLACK"
), (48, 0x62, "SUPER RED"
), (48, 0x6D, "GREEN PEARL"
), (48, 0x73, "DEEP MARINE BLUE "
), (49, 0x31, "PEARL WHITE"
), (49, 0x34, "CLEAR SILVER"
), (49, 0x36, "SUPER BLACK"
), (49, 0x62, "SUPER RED"
), (49, 0x6D, "GREEN PEARL"
), (49, 0x73, "DEEP MARINE BLUE "
), (50, 0x31, "PEARL WHITE"
), (50, 0x34, "BLUISH SILVER"
), (50, 0x36, "SUPER BLACK"
), (50, 0x62, "SUPER CLEAR RED"
), (50, 0x70, "GREENISH BLUE"
), (50, 0x71, "PURPLISH BLUE"
), (51, 0x31, "PEARL WHITE"
), (51, 0x34, "BLUISH SILVER"
), (51, 0x36, "SUPER BLACK"
), (51, 0x62, "SUPER CLEAR RED"
), (51, 0x70, "GREENISH BLUE"
), (51, 0x71, "PURPLISH BLUE"
), (52, 0x31, "PEARL WHITE"
), (52, 0x33, "YELLOWISH SILVER 2TONE"
), (52, 0x35, "DARK GRAY "
), (52, 0x36, "SUPER BLACK"
), (52, 0x61, "WARM WHITE 2TONE"
), (52, 0x62, "SUPER RED"
), (52, 0x68, "CITRUS YELLOW 2TONE"
), (52, 0x6C, "LIME GREEN 2TONE"
), (52, 0x6D, "DARK GREEN"
), (52, 0x71, "VELVET BLUE"
), (52, 0x72, "PURPLISH SILVER 2TONE"
), (53, 0x31, "PEARL WHITE"
), (53, 0x33, "YELLOWISH SILVER 2TONE"
), (53, 0x35, "DARK GRAY "
), (53, 0x36, "SUPER BLACK"
), (53, 0x61, "WARM WHITE 2TONE"
), (53, 0x62, "SUPER RED"
), (53, 0x68, "CITRUS YELLOW 2TONE"
), (53, 0x6C, "LIME GREEN 2TONE"
), (53, 0x6D, "DARK GREEN"
), (53, 0x71, "VELVET BLUE"
), (53, 0x72, "PURPLISH SILVER 2TONE"
), (54, 0x34, "BLUISH SILVER 2TONE"
), (54, 0x36, "SUPER BLACK"
), (54, 0x61, "WARM WHITE 2TONE"
), (54, 0x62, "SUPER RED"
), (54, 0x65, "CRANBERRY RED"
), (54, 0x6C, "LIME GREEN 2TONE"
), (54, 0x6D, "DARK GREEN"
), (54, 0x71, "VELVET BLUE"
), (55, 0x34, "BLUISH SILVER 2TONE"
), (55, 0x36, "SUPER BLACK"
), (55, 0x61, "WARM WHITE 2TONE"
), (55, 0x62, "SUPER RED"
), (55, 0x65, "CRANBERRY RED"
), (55, 0x6C, "LIME GREEN 2TONE"
), (55, 0x6D, "DARK GREEN"
), (55, 0x71, "VELVET BLUE"
), (56, 0x31, "WHITE"
), (56, 0x34, "SILVER METALLIC"
), (56, 0x35, "DARK GRAY PEARL METALLIC"
), (56, 0x36, "SUPER BLACK"
), (56, 0x62, "RASPBERRY RED PEARL"
), (56, 0x6C, "DARK GREEN PEARL"
), (56, 0x71, "DARK BLUE"
), (57, 0x31, "WHITE"
), (57, 0x34, "SILVER METALLIC"
), (57, 0x35, "GRAY METALLIC"
), (57, 0x36, "SUPER BLACK"
), (57, 0x62, "WINE RED PEARL METALLIC"
), (57, 0x6C, "GREEN PEARL"
), (57, 0x71, "DARK BLUE"
), (58, 0x31, "WHITE"
), (58, 0x35, "PURPLISH GRAY"
), (58, 0x36, "SUPER BLACK"
), (58, 0x62, "SUPER RED"
), (58, 0x71, "DEEP MARINE BLUE"
), (59, 0x31, "WHITE"
), (59, 0x34, "SPARK SILVER METALLIC"
), (59, 0x36, "SUPER BLACK"
), (59, 0x62, "SUPER RED"
), (59, 0x75, "MIDNIGHT PURPLE PEARL"
), (60, 0x31, "WHITE"
), (60, 0x34, "SPARK SILVER METALLIC"
), (60, 0x36, "SUPER BLACK"
), (60, 0x62, "SUPER RED"
), (60, 0x75, "MIDNIGHT PURPLE PEARL"
), (61, 0x31, "MARBLE WHITE"
), (61, 0x35, "BLUE GRAY GRAPHITE PEARL"
), (61, 0x36, "SUPER BLACK"
), (61, 0x62, "ACTIVE RED"
), (69, 0x31, "GALAXY WHITE"
), (69, 0x34, "GRACE SILVER"
), (69, 0x36, "TOSCANA BLACK"
), (69, 0x62, "PASSION RED"
), (69, 0x65, "KUTANI RED"
), (69, 0x71, "FIJI BLUE"
), (70, 0x31, "GALAXY WHITE"
), (70, 0x34, "GRACE SILVER"
), (70, 0x36, "TOSCANA BLACK"
), (70, 0x62, "PASSION RED"
), (70, 0x65, "KUTANI RED"
), (70, 0x71, "FIJI BLUE"
), (71, 0x31, "GALAXY WHITE"
), (71, 0x34, "SYMPHONIC SILVER"
), (71, 0x36, "PYRENEES BLACK"
), (71, 0x62, "PASSION RED"
), (71, 0x71, "MARIANA BLUE"
), (72, 0x31, "GALAXY WHITE"
), (72, 0x34, "SYMPHONIC SILVER"
), (72, 0x36, "PYRENEES BLACK"
), (72, 0x62, "PASSION RED"
), (72, 0x71, "MARIANA BLUE"
), (73, 0x31, "GALAXY WHITE"
), (73, 0x34, "SYMPHONIC SILVER"
), (73, 0x36, "PYRENEES BLACK"
), (73, 0x62, "PASSION RED"
), (73, 0x71, "MARIANA BLUE"
), (74, 0x31, "GALAXY WHITE"
), (74, 0x34, "HAMILTON SILVER"
), (74, 0x36, "PYRENEES BLACK"
), (74, 0x62, "PASSION RED"
), (74, 0x71, "MARIANA BLUE"
), (74, 0x6C, "TIMBER GREEN "
), (75, 0x31, "GALAXY WHITE"
), (75, 0x34, "HAMILTON SILVER"
), (75, 0x36, "PYRENEES BLACK"
), (75, 0x62, "PASSION RED"
), (75, 0x71, "MARIANA BLUE"
), (75, 0x6C, "TIMBER GREEN "
), (76, 0x31, "SOFIA WHITE"
), (76, 0x34, "HAMILTON SILVER"
), (76, 0x36, "PYRENEES BLACK"
), (76, 0x62, "PALMA RED"
), (76, 0x65, "ROANNE RED"
), (76, 0x6D, "FINESSE GREEN"
), (76, 0x77, "TRIGGER MAUVE"
), (77, 0x31, "SOFIA WHITE"
), (77, 0x34, "HAMILTON SILVER"
), (77, 0x33, "HAMILTON SILVER/THURSTON GRAY 2TONE"
), (77, 0x36, "PYRENEES BLACK"
), (77, 0x35, "PYRENEES BLACK/THURSTON GRAY 2TONE"
), (77, 0x62, "PALMA RED"
), (77, 0x64, "PALMA RED/THURSTON GRAY 2TONE"
), (77, 0x65, "ROANNE RED"
), (77, 0x6D, "FINESSE GREEN"
), (77, 0x77, "TRIGGER MAUVE"
), (78, 0x36, "APPALACHIAN BLACK"
), (78, 0x62, "PASADENA RED"
), (79, 0x31, "SCOTIA WHITE"
), (79, 0x34, "STEEL SILVER"
), (79, 0x36, "PYRENEES BLACK"
), (79, 0x62, "PASSION RED"
), (79, 0x65, "IMPERIAL RED"
), (79, 0x71, "MOON LIGHT BLUE"
), (80, 0x31, "SCOTIA WHITE"
), (80, 0x34, "STEEL SILVER"
), (80, 0x36, "PYRENEES BLACK"
), (80, 0x62, "PASSION RED"
), (80, 0x65, "IMPERIAL RED"
), (80, 0x68, "DANDELION YELLOW"
), (80, 0x71, "MOON LIGHT BLUE"
), (81, 0x31, "SCOTIA WHITE"
), (81, 0x34, "SYMPHONIC SILVER"
), (81, 0x36, "PYRENEES BLACK"
), (81, 0x62, "PASSION RED"
), (81, 0x6C, "KILDER GREEN "
), (81, 0x71, "IJSSEL BLUE"
), (82, 0x31, "SCOTIA WHITE"
), (82, 0x34, "SYMPHONIC SILVER"
), (82, 0x36, "PYRENEES BLACK"
), (82, 0x62, "PASSION RED"
), (82, 0x6C, "KILDER GREEN "
), (82, 0x71, "IJSSEL BLUE"
), (83, 0x31, "SCOTIA WHITE"
), (83, 0x36, "PYRENEES BLACK"
), (83, 0x62, "PASSION RED"
), (84, 0x31, "SCOTIA WHITE"
), (84, 0x34, "QUEENS SILVER"
), (84, 0x36, "PYRENEES BLACK"
), (84, 0x62, "MONACO RED"
), (84, 0x68, "DANDELION YELLOW"
), (85, 0x31, "SCOTIA WHITE"
), (85, 0x34, "STEEL SILVER"
), (85, 0x36, "PYRENEES BLACK"
), (85, 0x62, "PALMA RED"
), (85, 0x71, "IJSSEL BLUE"
), (86, 0x31, "SCOTIA WHITE"
), (86, 0x34, "STEEL SILVER"
), (86, 0x36, "PYRENEES BLACK"
), (86, 0x62, "PALMA RED"
), (86, 0x65, "ROANNE RED"
), (86, 0x6D, "KILDER GREEN "
), (86, 0x71, "IJSSEL BLUE"
), (87, 0x31, "SCOTIA WHITE"
), (87, 0x34, "GRACE SILVER"
), (87, 0x35, "CORSE GRAY"
), (87, 0x36, "PYRENEES BLACK"
), (87, 0x62, "COLTON RED"
), (87, 0x68, "CHAMPAGNISH YELLOW"
), (87, 0x6C, "LOIRE GREEN"
), (87, 0x6D, "SAINTO AMOUR GREEN"
), (87, 0x71, "TWILIGHT BLUE"
), (91, 0x31, "GRANDPRIX WHITE"
), (91, 0x32, "NEUTRON WHITE PEARL"
), (91, 0x34, "SEBRING SILVER METALLIC"
), (91, 0x35, "KAISER SILVER METALLIC"
), (91, 0x36, "QUASAR GRAY PEARL"
), (91, 0x37, "BERLINA BLACK"
), (91, 0x64, "FORMULA RED"
), (91, 0x65, "CRANBERRY RED PEARL"
), (91, 0x68, "INDY YELLOW PEARL"
), (91, 0x6C, "ESTORIL TURQUOISE PEARL"
), (91, 0x6D, "SHALLOT GREEN PEARL"
), (91, 0x70, "BAYLEAF GREEN"
), (91, 0x73, "MONTE CARLO BLUE PEARL"
), (92, 0x31, "GRANDPRIX WHITE"
), (92, 0x32, "NEUTRON WHITE PEARL"
), (92, 0x33, "CHAMPIONSHIP WHITE"
), (92, 0x34, "SEBRING SILVER METALLIC"
), (92, 0x35, "KAISER SILVER METALLIC"
), (92, 0x36, "QUASAR GRAY PEARL"
), (92, 0x37, "BERLINA BLACK"
), (92, 0x64, "FORMULA RED"
), (92, 0x65, "CRANBERRY RED PEARL"
), (92, 0x68, "INDY YELLOW PEARL"
), (92, 0x6C, "ESTORIL TURQUOISE PEARL"
), (92, 0x6D, "SHALLOT GREEN PEARL"
), (92, 0x70, "BAYLEAF GREEN"
), (92, 0x73, "MONTE CARLO BLUE PEARL"
), (93, 0x31, "GRANDPRIX WHITE"
), (93, 0x32, "NEUTRON WHITE PEARL"
), (93, 0x33, "PLATINUM WHITE PEARL"
), (93, 0x34, "SEBRING SILVER METALLIC"
), (93, 0x35, "KAISER SILVER METALLIC"
), (93, 0x36, "MAGNUM GRAY PEARL"
), (93, 0x37, "BERLINA BLACK"
), (93, 0x62, "GRANDPRIX RED"
), (93, 0x64, "FORMULA RED"
), (93, 0x68, "INDY YELLOW PEARL"
), (93, 0x6C, "ESTORIL TURQUOISE PEARL"
), (93, 0x71, "PHOENIX BLUE"
), (93, 0x73, "MONTE CARLO BLUE PEARL"
), (93, 0x75, "MIDNIGHT PEARL"
), (94, 0x31, "GRANDPRIX WHITE"
), (94, 0x32, "NEUTRON WHITE PEARL"
), (94, 0x33, "PLATINUM WHITE PEARL"
), (94, 0x34, "SEBRING SILVER METALLIC"
), (94, 0x35, "KAISER SILVER METALLIC"
), (94, 0x36, "MAGNUM GRAY PEARL"
), (94, 0x37, "BERLINA BLACK"
), (94, 0x62, "GRANDPRIX RED"
), (94, 0x64, "FORMULA RED"
), (94, 0x67, "IMOLA ORANGE PEARL"
), (94, 0x68, "INDY YELLOW PEARL"
), (94, 0x6C, "ESTORIL TURQUOISE PEARL"
), (94, 0x71, "PHOENIX BLUE"
), (94, 0x73, "MONTE CARLO BLUE PEARL"
), (94, 0x75, "MIDNIGHT PEARL"
), (95, 0x31, "GRANDPRIX WHITE"
), (95, 0x32, "NEUTRON WHITE PEARL"
), (95, 0x33, "PLATINUM WHITE PEARL"
), (95, 0x34, "SEBRING SILVER METALLIC"
), (95, 0x35, "KAISER SILVER METALLIC"
), (95, 0x36, "MAGNUM GRAY PEARL"
), (95, 0x37, "BERLINA BLACK"
), (95, 0x62, "GRANDPRIX RED"
), (95, 0x64, "FORMULA RED"
), (95, 0x67, "IMOLA ORANGE PEARL"
), (95, 0x68, "INDY YELLOW PEARL"
), (95, 0x6C, "ESTORIL TURQUOISE PEARL"
), (95, 0x71, "PHOENIX BLUE"
), (95, 0x73, "MONTE CARLO BLUE PEARL"
), (95, 0x75, "MIDNIGHT PEARL"
), (96, 0x31, "FROST WHITE"
), (96, 0x34, "SEBRING SILVER METALLIC"
), (96, 0x35, "PHANTOM GRAY PEARL"
), (96, 0x36, "GRANADA BLACK PEARL"
), (96, 0x62, "MILANO RED"
), (96, 0x65, "CASSIS RED PEARL"
), (96, 0x70, "BRITTANY BLUE GREEN METALLIC"
), (96, 0x71, "COBALT BLUE PEARL"
), (97, 0x31, "FROST WHITE"
), (97, 0x34, "SEBRING SILVER METALLIC"
), (97, 0x35, "PHANTOM GRAY PEARL"
), (97, 0x36, "GRANADA BLACK PEARL"
), (97, 0x62, "MILANO RED"
), (97, 0x65, "CASSIS RED PEARL"
), (97, 0x70, "BRITTANY BLUE GREEN METALLIC"
), (97, 0x71, "COBALT BLUE PEARL"
), (98, 0x31, "TAFFETAS  WHITE"
), (98, 0x33, "WHITE DIAMOND PEARL"
), (98, 0x34, "ICEBERG SILVER METALLIC"
), (98, 0x36, "STARLIGHT BLACK PEARL"
), (98, 0x62, "SAN MARINO RED "
), (98, 0x65, "RUBY RED PEARL"
), (98, 0x6D, "EUCALYPTUS GREEN PEARL"
), (98, 0x73, "SUPER MARINE BLUE PEARL"
), (99, 0x31, "TAFFETAS  WHITE"
), (99, 0x33, "WHITE DIAMOND PEARL"
), (99, 0x34, "ICEBERG SILVER METALLIC"
), (99, 0x36, "STARLIGHT BLACK PEARL"
), (99, 0x62, "SAN MARINO RED "
), (99, 0x65, "RUBY RED PEARL"
), (100, 0x31, "FROST WHITE"
), (100, 0x34, "VOGUE SILVER METALLIC"
), (100, 0x36, "STAR LIGHT BLACK PEARL"
), (100, 0x62, "MILANO RED"
), (100, 0x65, "MATADOR RED PEARL"
), (100, 0x6D, "CYPRESS GREEN PEARL"
), (100, 0x71, "ADRIATIC BLUE PEARL"
), (100, 0x76, "DARK CURRANT PEARL"
), (101, 0x31, "CHAMPIONSHIP WHITE"
), (101, 0x34, "VOGUE SILVER METALLIC"
), (101, 0x36, "STAR LIGHT BLACK PEARL"
), (101, 0x62, "MILANO RED"
), (102, 0x31, "FROST WHITE"
), (102, 0x34, "VOGUE SILVER METALLIC"
), (102, 0x35, "PHANTOM GRAY PEARL"
), (102, 0x65, "MATADOR RED PEARL"
), (102, 0x6D, "CYPRESS GREEN PEARL"
), (102, 0x6F, "CYCLONE BLUE METALLIC"
), (103, 0x31, "FROST WHITE"
), (103, 0x34, "VOGUE SILVER METALLIC"
), (103, 0x36, "GRANADA BLACK PEARL"
), (103, 0x62, "MILANO RED"
), (103, 0x72, "DARK AMETHYST PEARL"
), (104, 0x31, "CHAMPIONSHIP WHITE"
), (104, 0x34, "VOGUE SILVER METALLIC"
), (104, 0x36, "STAR LIGHT BLACK PEARL"
), (105, 0x34, "VOGUE SILVER METALLIC"
), (105, 0x36, "FLINT BLACK METALLIC"
), (105, 0x62, "MILANO RED"
), (106, 0x34, "VOGUE SILVER METALLIC"
), (106, 0x36, "FLINT BLACK METALLIC"
), (106, 0x62, "MILANO RED"
), (106, 0x6C, "SAMBA GREEN PEARL"
), (106, 0x71, "CAPTIVA BLUE PEARL"
), (107, 0x31, "FROST WHITE"
), (107, 0x34, "VOGUE SILVER METALLIC"
), (107, 0x62, "MILANO RED"
), (107, 0x6C, "CYPRESS GREEN PEARL"
), (108, 0x31, "FROST WHITE"
), (108, 0x34, "VOGUE SILVER METALLIC"
), (108, 0x62, "MILANO RED"
), (108, 0x6C, "CYPRESS GREEN PEARL"
), (109, 0x36, "GRANADA BLACK PEARL"
), (109, 0x62, "MILANO RED"
), (109, 0x70, "TAHITIAN GREEN PEARL"
), (109, 0x72, "CAPTIVA BLUE PEARL"
), (110, 0x34, "VOGUE SILVER METALLIC"
), (110, 0x35, "PHANTOM GRAY PEARL"
), (110, 0x6D, "LAUSANNE GREEN PEARL"
), (110, 0x73, "HERBERT BLUE PEARL"
), (111, 0x31, "FROST WHITE"
), (111, 0x36, "FLINT BLACK METALLIC"
), (111, 0x62, "TORINO RED PEARL"
), (111, 0x6C, "BARCELONA GREEN PEARL"
), (112, 0x31, "TAFFETAS  WHITE"
), (112, 0x33, "SAPPHIRE SILVER METALLIC"
), (112, 0x34, "SEBRING SILVER METALLIC"
), (112, 0x36, "STARLIGHT BLACK PEARL"
), (112, 0x62, "SAN MARINO RED "
), (112, 0x6C, "CYPRESS GREEN PEARL"
), (112, 0x71, "SUPER MARINE BLUE PEARL"
), (112, 0x73, "PACIFIC BLUE PEARL"
), (113, 0x31, "FROST WHITE"
), (113, 0x34, "SOLARIS SILVER METALLIC"
), (113, 0x36, "STARLIGHT BLACK PEARL"
), (113, 0x65, "BORDEAUX RED PEARL"
), (113, 0x6D, "SHERWOOD GREEN PEARL"
), (118, 0x34, "SILVER STONE METALLIC"
), (118, 0x36, "BLACK FOREST MICA"
), (118, 0x62, "PASSION ROSE MICA"
), (118, 0x71, "CREEK BLUE MICA"
), (119, 0x34, "SILVER STONE METALLIC"
), (119, 0x36, "BLACK FOREST MICA"
), (119, 0x62, "PASSION ROSE MICA"
), (119, 0x71, "CREEK BLUE MICA"
), (120, 0x34, "SILVER STONE METALLIC"
), (120, 0x36, "BRILLIANT BLACK"
), (120, 0x62, "CLASSIC RED"
), (120, 0x70, "SPARKLE GREEN METALLIC"
), (120, 0x73, "INDIGO BLUE METALLIC"
), (121, 0x31, "CRYSTAL WHITE"
), (121, 0x34, "SILVER STONE METALLIC"
), (121, 0x62, "CLASSIC RED"
), (121, 0x68, "J Limited/SUNBURST YELLOW"
), (121, 0x71, "MARINER BLUE"
), (122, 0x36, "BRILLIANT BLACK"
), (122, 0x6D, "BRITISH RACING GREEN"
), (123, 0x35, "BRILLIANT BLACK"
), (123, 0x62, "CLASSIC RED"
), (124, 0x31, "CHASTE WHITE"
), (124, 0x34, "SILVER STONE METALLIC"
), (124, 0x35, "BRILLIANT BLACK"
), (124, 0x62, "CLASSIC RED"
), (124, 0x6C, "BRITISH RACING GREEN"
), (124, 0x70, "SR Limited/SPARKLE GREEN METALLIC"
), (124, 0x72, "G Limited/SATELLITE BLUE MICA"
), (124, 0x74, "B2 Limited/TWILIGHT BLUE MICA"
), (125, 0x31, "CHASTE WHITE"
), (125, 0x32, "typeII/CHASTE WHITE"
), (125, 0x35, "BRILLIANT BLACK"
), (125, 0x36, "typeII/BRILLIANT BLACK"
), (125, 0x6C, "BRITISH RACING GREEN"
), (125, 0x6D, "typeII/BRITISH RACING GREEN"
), (126, 0x31, "CHASTE WHITE"
), (126, 0x35, "BRILLIANT BLACK"
), (126, 0x62, "CLASSIC RED"
), (126, 0x65, "VR Limited A/ MICA MELROT"
), (126, 0x6E, "VR Limited B/EXCELLENT GREEN MICA"
), (126, 0x71, "LAGUNA BLUE METALLIC"
), (126, 0x73, "MONTEGO BLUE MICA"
), (126, 0x72, "R Limited/SATELLITE BLUE MICA"
), (127, 0x31, "CHASTE WHITE"
), (127, 0x34, "SILVER STONE METALLIC"
), (127, 0x36, "BRILLIANT BLACK"
), (127, 0x62, "VINTAGE RED"
), (127, 0x68, "COMPETITION YELLOW MICA"
), (128, 0x36, "BRILLIANT BLACK"
), (129, 0x31, "CHASTE WHITE"
), (129, 0x34, "SILVER STONE METALLIC"
), (129, 0x36, "BRILLIANT BLACK"
), (129, 0x62, "VINTAGE RED"
), (130, 0x31, "CHASTE WHITE"
), (130, 0x34, "SILVER STONE METALLIC"
), (130, 0x36, "BRILLIANT BLACK"
), (130, 0x62, "VINTAGE RED"
), (131, 0x31, "CHASTE WHITE"
), (131, 0x34, "SILVER STONE METALLIC"
), (131, 0x36, "BRILLIANT BLACK"
), (131, 0x62, "VINTAGE RED"
), (131, 0x68, "COMPETITION YELLOW MICA"
), (132, 0x31, "CRYSTAL WHITE"
), (132, 0x35, "SHADOW SILVER MICA"
), (132, 0x36, "BRILLIANT BLACK"
), (132, 0x62, "BLAZE RED"
), (132, 0x73, "BRAVE BLUE MICA"
), (133, 0x31, "CRYSTAL WHITE"
), (133, 0x35, "SHADOW SILVER MICA"
), (133, 0x36, "BRILLIANT BLACK"
), (133, 0x62, "BLAZE RED"
), (133, 0x73, "BRAVE BLUE MICA"
), (134, 0x31, "CHASTE WHITE"
), (134, 0x32, "CHASTE WHITE/BLACK BUMPER"
), (134, 0x34, "SILVER STONE METALLIC"
), (134, 0x35, "SILVER STONE METALLIC/BLACK BUMPER"
), (134, 0x62, "CLASSIC RED"
), (134, 0x63, "CLASSIC RED/BLACK BUMPER"
), (134, 0x64, "PASSION ROSE MICA"
), (134, 0x65, "PASSION ROSE MICA/BLACK BUMPER"
), (134, 0x6B, "SPARKLE GREEN METALLIC"
), (134, 0x6C, "SPARKLE GREEN METALLIC/BLACK BUMPER"
), (134, 0x6D, "EXCELLENT GREEN MICA"
), (134, 0x6E, "EXCELLENT GREEN MICA/BLACK BUMPER"
), (134, 0x71, "AQUARIUS BLUE MICA"
), (134, 0x72, "AQUARIUS BLUE MICA/BLACK BUMPER"
), (134, 0x73, "INDIGO BLUE METALLIC"
), (134, 0x74, "INDIGO BLUE METALIC/BLACK BUMPER"
), (135, 0x31, "CHASTE WHITE"
), (135, 0x34, "SILVER STONE METALLIC"
), (135, 0x62, "CLASSIC RED"
), (135, 0x64, "PASSION ROSE MICA"
), (135, 0x6B, "SPARKLE GREEN METALLIC"
), (135, 0x6D, "EXCELLENT GREEN MICA"
), (135, 0x71, "AQUARIUS BLUE MICA"
), (135, 0x73, "INDIGO BLUE METALLIC"
), (136, 0x31, "CHASTE WHITE"
), (136, 0x34, "SILVER STONE METALLIC"
), (136, 0x62, "CLASSIC RED"
), (136, 0x64, "PASSION ROSE MICA"
), (136, 0x6B, "SPARKLE GREEN METALLIC"
), (136, 0x6D, "EXCELLENT GREEN MICA"
), (136, 0x71, "AQUARIUS BLUE MICA"
), (136, 0x73, "INDIGO BLUE METALLIC"
), (141, 0x31, "WHITE MICA"
), (141, 0x34, "SILVER METALLIC/GRAY METALLIC 2TONE"
), (141, 0x35, "DARK GRAY METALLIC"
), (141, 0x36, "BLACK MICA"
), (141, 0x62, "RED MICA"
), (141, 0x65, "CRIMSON MICA"
), (142, 0x34, "LIGHT SILVER METALLIC"
), (142, 0x62, "BORDEAUX RED MICA"
), (142, 0x6C, "BRIGHT GREEN MICA"
), (143, 0x31, "PURE WHITE"
), (143, 0x34, "LIGHT SILVER METALLIC"
), (143, 0x35, "GRAY OPAL"
), (143, 0x36, "BLACK MICA"
), (143, 0x65, "DEEP RED MICA"
), (143, 0x68, "CASHMERE YELLOW"
), (143, 0x6D, "VINTAGE GREEN MICA"
), (143, 0x73, "ROYAL BLUE MICA"
), (144, 0x31, "PURE WHITE"
), (144, 0x34, "LIGHT SILVER METALLIC"
), (144, 0x35, "GRAY OPAL"
), (144, 0x36, "BLACK MICA"
), (144, 0x65, "DEEP RED MICA"
), (144, 0x68, "CASHMERE YELLOW"
), (144, 0x6D, "VINTAGE GREEN MICA"
), (144, 0x73, "ROYAL BLUE MICA"
), (145, 0x34, "LIGHT SILVER METALLIC"
), (145, 0x36, "BLACK MICA"
), (145, 0x62, "MATADOR RED"
), (145, 0x65, "CRIMSON MICA"
), (145, 0x73, "COSMIC BLUE MICA"
), (146, 0x34, "LIGHT SILVER METALLIC"
), (146, 0x36, "BLACK MICA"
), (146, 0x65, "CRIMSON MICA"
), (146, 0x6D, "FOREST GREEN MICA"
), (146, 0x73, "COSMIC BLUE MICA"
), (147, 0x31, "FEATHER WHITE"
), (147, 0x68, "CHASE YELLOW"
), (147, 0x71, "SONIC BLUE MICA"
), (148, 0x31, "FEATHER WHITE"
), (148, 0x34, "LIGHT SILVER METALLIC"
), (148, 0x36, "BLACK MICA"
), (148, 0x62, "ACTIVE RED"
), (148, 0x71, "ROYAL BLUE MICA"
), (149, 0x31, "FEATHER WHITE"
), (149, 0x34, "LIGHT SILVER METALLIC"
), (149, 0x36, "BLACK MICA"
), (149, 0x70, "SPORTS BLUE"
), (150, 0x34, "LIGHT SILVER METALLIC"
), (150, 0x36, "BLACK MICA"
), (150, 0x62, "ACTIVE RED"
), (150, 0x71, "ROYAL BLUE MICA"
), (151, 0x31, "FEATHER WHITE"
), (151, 0x34, "LIGHT SILVER METALLIC"
), (151, 0x36, "BLACK MICA"
), (151, 0x71, "ROYAL BLUE MICA"
), (151, 0x70, "SPORTS BLUE"
), (154, 0x31, "FEATHER WHITE"
), (154, 0x34, "LIGHT SILVER METALLIC"
), (154, 0x36, "BLACK MICA"
), (154, 0x62, "ACTIVE RED"
), (154, 0x71, "COSMIC BLUE MICA"
), (152, 0x31, "FEATHER WHITE"
), (152, 0x34, "LIGHT SILVER METALLIC"
), (152, 0x62, "ACTIVE RED"
), (155, 0x34, "LIGHT SILVER METALLIC"
), (155, 0x36, "BLACK MICA"
), (155, 0x62, "ACTIVE RED"
), (155, 0x71, "COSMIC BLUE MICA"
), (153, 0x31, "FEATHER WHITE"
), (153, 0x34, "LIGHT SILVER METALLIC"
), (153, 0x62, "ACTIVE RED"
), (159, 0x34, "SILVER"
), (159, 0x35, "DARK GRAY"
), (159, 0x36, "BLACK"
), (159, 0x62, "RED"
), (159, 0x65, "CRIMSON "
), (159, 0x68, "YELLOW"
), (159, 0x6D, "DARK GREEN"
), (160, 0x34, "SILVER"
), (160, 0x35, "DARK GRAY"
), (160, 0x36, "BLACK"
), (160, 0x62, "RED"
), (160, 0x65, "CRIMSON "
), (160, 0x68, "YELLOW"
), (160, 0x6D, "DARK GREEN"
), (162, 0x62, "RED"
), (162, 0x71, "BLUE with painted WHITE STRIPES"
), (163, 0x62, "RED"
), (163, 0x71, "BLUE with painted WHITE STRIPES"
), (164, 0x62, "RED"
), (167, 0x71, "ADMIRAL BLUE METALLIC"
), (168, 0x31, "ARCTIC WHITE"
), (168, 0x36, "BLACK"
), (168, 0x62, "TORCH RED"
), (168, 0x68, "COMPETITION YELLOW"
), (168, 0x6D, "POLO GREEN II METALLIC"
), (168, 0x70, "BRIGHT AQUA METALLIC"
), (168, 0x77, "DARK PURPLE METALLIC"
), (169, 0x31, "ARCTIC WHITE"
), (169, 0x34, "SEBRING SILVER METALLIC"
), (169, 0x36, "BLACK"
), (169, 0x62, "BRIGHT RED"
), (169, 0x65, "CAYENNE RED METALLIC"
), (169, 0x6C, "BRIGHT GREEN METALLIC"
), (169, 0x6D, "POLO GREEN METALLIC"
), (169, 0x70, "MYSTIC TEAL METALLIC"),
(169, 0x71, "MEDIUM QUASAR BLUE METALLIC"),
(169, 0x75, "BRIGHT PURPLE METALLIC"),
(171, 0x31, "OPAL WHITE"),
(171, 0x34, "SILVERSTONE METALLIC"),
(171, 0x35, "GREEN BLACK PEARL"),
(171, 0x36, "MOON RAKER BLACK METALLIC"),
(171, 0x62, "FORMULA RED PEARL"),
(171, 0x63, "PEARL GOLD"),
(171, 0x65, "CRIMSON STARMIST"),
(171, 0x67, "MACAO YELLOW"),
(171, 0x68, "MIDAS YELLOW PEARL"),
(171, 0x6A, "ASTON SAGE GREEN PEARL"),
(171, 0x6C, "COOPER GREEN METALLIC"),
(171, 0x6D, "BRITISH RACING GREEN"),
(171, 0x71, "COBALT BLUE "),
(171, 0x72, "SPACE BLUE PEARL"),
(171, 0x76, "RASPBERRY METALLIC"),
(173, 0x31, "OPAL WHITE"),
(173, 0x34, "SILVERSTONE METALLIC"),
(173, 0x35, "GREEN BLACK PEARL"),
(173, 0x36, "MOON RAKER BLACK METALLIC"),
(173, 0x62, "FORMULA RED PEARL"),
(173, 0x63, "PEARL GOLD"),
(173, 0x65, "CRIMSON STARMIST"),
(173, 0x67, "MACAO YELLOW"),
(173, 0x68, "MIDAS YELLOW PEARL"),
(173, 0x6C, "COOPER GREEN METALLIC"),
(173, 0x6D, "BRITISH RACING GREEN"),
(173, 0x71, "COBALT BLUE "),
(173, 0x72, "SPACE BLUE PEARL"),
(173, 0x76, "RASPBERRY METALLIC"),
(172, 0x67, "MACAO YELLOW"),
(172, 0x36, "MOON RAKER BLACK METALLIC"),
(172, 0x6C, "COOPER GREEN METALLIC"),
(176, 0x31, "CHASTE WHITE"),
(176, 0x34, "HIGHLIGHT SILVER METALLIC"),
(176, 0x36, "BRILLIANT BLACK"),
(176, 0x62, "CLASSIC RED"),
(176, 0x67, "EVOLUTION ORANGE MICA"),
(176, 0x67, "EVOLUTION ORANGE MICA"),
(176, 0x74, "TWILIGHT BLUE MICA"),
(177, 0x31, "ERMINE WHITE"),
(177, 0x34, "SILVER PEARL"),
(177, 0x36, "TUXEDO BLACK"),
(177, 0x62, "RALLY RED"),
(177, 0x63, "MARLBORO MAROON"),
(177, 0x68, "SUNFIRE YELLOW"),
(177, 0x6C, "GOODWOOD GREEN"),
(177, 0x6F, "ELKHART BLUE"),
(177, 0x71, "LYNNDALE BLUE"),
(177, 0x73, "MARINA BLUE")
        };

        private static readonly Dictionary<string, string> carNames = new()
        {
            { "acoen", "EUNOS COSMO 20BTYPE-ECCS" },
            { "acosn", "EUNOS COSMO 13BTYPE-SCCS" },
            { "afc7n", "RX-7 FC III" },
            { "afo7n", "RX-7 FD '91 Type-R" },
            { "afr7n", "RX-7 FC GT-X" },
            { "an16n", "EUNOS ROADSTER '89 NORMAL" },
            { "as16n", "EUNOS ROADSTER '92 S-SPECIAL" },
            { "av16n", "EUNOS ROADSTER '90 V-SPECIAL" },
            { "hcfon", "EG CIVIC '93 FERIO Si-R" },
            { "hcrxn", "CR-X EF-8 Si-R" },
            { "hcvon", "EG CIVIC '93 Si-R II" },
            { "hdeln", "CR-X del-sol '92 Vxi" },
            { "hdern", "CR-X del-sol '92 Si-R" },
            { "hnsrn", "NSX '92 TypeR" },
            { "hnsxn", "NSX '90" },
            { "hpren", "PRELUDE '91 Si" },
            { "hprvn", "PRELUDE '91 Si VTEC" },
            { "mfton", "FTO '94 GPX" },
            { "mftrn", "FTO '94 GR" },
            { "mgoon", "GTO '92" },
            { "mgotn", "GTO '92 Twin Turbo" },
            { "mgtmn", "GTO '95 MR" },
            { "mgton", "GTO '95 SR" },
            { "mgttn", "GTO '95 Twin Turbo" },
            { "mlnon", "LANCER EvolutionIII GSR" },
            { "mmgon", "MIRAGE '92 Cyborg R" },
            { "n180n", "180SX '94 Type X" },
            { "n432n", "R32SKYLINE GTS-4" },
            { "nm32n", "R32SKYLINE GTS25 TypeS" },
            { "nn32n", "SKYLINE GT-R NISMO (R32)" },
            { "nplon", "PULSAR '91 GTi-R" },
            { "npron", "PRIMERA '90 2.0Te" },
            { "nq13n", "S13 SILVIA '88 Q'S 1800cc" },
            { "nq14n", "S14 SILVIA '95 Q'S" },
            { "nq23n", "S13 SILVIA '91 Q'S 2000cc" },
            { "nr02n", "R32SKYLINE '89 GT-R" },
            { "nr32n", "R32SKYLINE '91 GT-R" },
            { "nr33n", "R33SKYLINE '95 GT-R" },
            { "ns13n", "S13 SILVIA '88 K'S 1800cc" },
            { "ns14n", "S14 SILVIA '95 K's" },
            { "ns23n", "S13 SILVIA '91 K'S 2000cc" },
            { "nt32n", "R32SKYLINE GTS-t TypeM" },
            { "nv12n", "R32SKYLINE GT-R Vspec" },
            { "nv22n", "R32SKYLINE GT-R Vspec II" },
            { "nv33n", "R33SKYLINE '95 GT-R Vspec" },
            { "sipsn", "IMPREZA '95 Wagon WRX-STi version II" },
            { "siptn", "IMPREZA '94 WagonWRX" },
            { "sipwn", "IMPREZA '94 Sedan WRX" },
            { "sipzn", "IMPREZA '95 Sedan WRX-STi version II" },
            { "slgnn", "LEGACY '93 TouringSportRS" },
            { "slgwn", "LEGACY '93 TouringWagonGT" },
            { "ssv4n", "ALCYONE SVX S4" },
            { "ssvxn", "ALCYINE SVX VersionL" },
            { "tlvon", "AE86 COROLLA LEVIN GT-APEX" },
            { "tm2sn", "MARKII '92 TOURER S" },
            { "tm2vn", "MARKII '92 TOURER V" },
            { "tsgtn", "MA70 SUPRA GT Turbo Limited" },
            { "tsonn", "SUPRA '95 RZ" },
            { "tsoon", "SOARER '95 2.5GT-T" },
            { "tsorn", "SUPRA '95 SZ-R" },
            { "tspon", "JZA70 SUPRA TwinTurbo-R" },
            { "ttron", "AE86 SPRINTER TRUENO GT-APEX" }
        };
    }
}