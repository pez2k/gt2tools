namespace GT2.DataSplitter.GTDT
{
    using Common;
    using GTModeRace;
    using Models;

    public class GTModeRaceFile : DataFileWithStrings<GTModeModel>
    {
        public GTModeRaceFile() : base(typeof(Event),
                                       typeof(EnemyCars),
                                       typeof(Regulations))
        {
        }
    }
}