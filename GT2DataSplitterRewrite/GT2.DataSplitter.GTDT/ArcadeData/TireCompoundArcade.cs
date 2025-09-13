namespace GT2.DataSplitter.GTDT.ArcadeData
{
    public class TireCompoundArcade : DataStructureWithModel<Models.Arcade.TireCompoundArcade>
    {
        public TireCompoundArcade() => Size = 0x40;

        public override Models.Arcade.TireCompoundArcade MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Arcade.TireCompoundArcade
            {
                Data = rawData
            };
    }
}