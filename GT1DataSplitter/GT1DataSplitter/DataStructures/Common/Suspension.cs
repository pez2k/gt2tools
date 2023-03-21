using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class Suspension : CsvDataStructure<SuspensionData, SuspensionCSVMap>
    {
        public Suspension()
        {
            Header = "SUSPENS";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x36);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x48
    public struct SuspensionData
    {
        public byte FrontUnknown;
        public byte FrontUnknown2;
        public byte FrontUnknown3;
        public byte FrontUnknown4;
        public byte FrontBumpRubber;
        public byte RearUnknown;
        public byte RearUnknown2;
        public byte RearUnknown3;
        public byte RearUnknown4;
        public byte RearBumpRubber;
        public byte FrontCamber;
        public byte FrontRideHeight;
        public byte FrontSpringRate;
        public byte FrontDamperBound;
        public byte FrontDamperRebound;
        public byte FrontDamperBound2Maybe;
        public byte FrontDamperRebound2Maybe;
        public byte RearCamber;
        public byte RearRideHeight;
        public byte RearSpringRate;
        public byte RearDamperBound;
        public byte RearDamperRebound;
        public byte RearDamperBound2Maybe;
        public byte RearDamperRebound2Maybe;
        public byte FrontCamberMin;
        public byte FrontCamberMax;
        public byte FrontRideHeightMin;
        public byte FrontRideHeightMax;
        public byte FrontSpringRateMin;
        public byte FrontSpringRateMax;
        public byte FrontDamperBoundMin;
        public byte FrontDamperBoundMax;
        public byte FrontDamperReboundMin;
        public byte FrontDamperReboundMax;
        public byte FrontDamperBound2MaybeMin;
        public byte FrontDamperBound2MaybeMax;
        public byte FrontDamperRebound2MaybeMin;
        public byte FrontDamperRebound2MaybeMax;
        public byte FrontDamperSteps;
        public byte RearCamberMin;
        public byte RearCamberMax;
        public byte RearRideHeightMin;
        public byte RearRideHeightMax;
        public byte RearSpringRateMin;
        public byte RearSpringRateMax;
        public byte RearDamperBoundMin;
        public byte RearDamperBoundMax;
        public byte RearDamperReboundMin;
        public byte RearDamperReboundMax;
        public byte RearDamperBound2MaybeMin;
        public byte RearDamperBound2MaybeMax;
        public byte RearDamperRebound2MaybeMin;
        public byte RearDamperRebound2MaybeMax;
        public byte RearDamperSteps;
        public ushort CarID;
        public byte Stage;
        public byte StageDuplicate;
        public ushort Padding;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
    }

    public sealed class SuspensionCSVMap : ClassMap<SuspensionData>
    {
        public SuspensionCSVMap(List<List<string>> tables)
        {
            Map(m => m.FrontUnknown);
            Map(m => m.FrontUnknown2);
            Map(m => m.FrontUnknown3);
            Map(m => m.FrontUnknown4);
            Map(m => m.FrontBumpRubber);
            Map(m => m.RearUnknown);
            Map(m => m.RearUnknown2);
            Map(m => m.RearUnknown3);
            Map(m => m.RearUnknown4);
            Map(m => m.RearBumpRubber);
            Map(m => m.FrontCamber);
            Map(m => m.FrontRideHeight);
            Map(m => m.FrontSpringRate);
            Map(m => m.FrontDamperBound);
            Map(m => m.FrontDamperRebound);
            Map(m => m.FrontDamperBound2Maybe);
            Map(m => m.FrontDamperRebound2Maybe);
            Map(m => m.RearCamber);
            Map(m => m.RearRideHeight);
            Map(m => m.RearSpringRate);
            Map(m => m.RearDamperBound);
            Map(m => m.RearDamperRebound);
            Map(m => m.RearDamperBound2Maybe);
            Map(m => m.RearDamperRebound2Maybe);
            Map(m => m.FrontCamberMin);
            Map(m => m.FrontCamberMax);
            Map(m => m.FrontRideHeightMin);
            Map(m => m.FrontRideHeightMax);
            Map(m => m.FrontSpringRateMin);
            Map(m => m.FrontSpringRateMax);
            Map(m => m.FrontDamperBoundMin);
            Map(m => m.FrontDamperBoundMax);
            Map(m => m.FrontDamperReboundMin);
            Map(m => m.FrontDamperReboundMax);
            Map(m => m.FrontDamperBound2MaybeMin);
            Map(m => m.FrontDamperBound2MaybeMax);
            Map(m => m.FrontDamperRebound2MaybeMin);
            Map(m => m.FrontDamperRebound2MaybeMax);
            Map(m => m.FrontDamperSteps);
            Map(m => m.RearCamberMin);
            Map(m => m.RearCamberMax);
            Map(m => m.RearRideHeightMin);
            Map(m => m.RearRideHeightMax);
            Map(m => m.RearSpringRateMin);
            Map(m => m.RearSpringRateMax);
            Map(m => m.RearDamperBoundMin);
            Map(m => m.RearDamperBoundMax);
            Map(m => m.RearDamperReboundMin);
            Map(m => m.RearDamperReboundMax);
            Map(m => m.RearDamperBound2MaybeMin);
            Map(m => m.RearDamperBound2MaybeMax);
            Map(m => m.RearDamperRebound2MaybeMin);
            Map(m => m.RearDamperRebound2MaybeMax);
            Map(m => m.RearDamperSteps);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.Stage);
            Map(m => m.StageDuplicate);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}