namespace GT2.DataSplitter.GTDT.ArcadeData
{
    using Common;

    public class TireCompoundArcade : TireCompound
    {
        public override Models.Arcade.TireCompoundArcade MapToModel() =>
            new Models.Arcade.TireCompoundArcade
            {
                Data = rawData
            };
    }
}