namespace GT2.DataSplitter.GTDT.LicenseData
{
    public class TireCompoundLicense : DataStructureWithModel<Models.License.TireCompoundLicense>
    {
        public TireCompoundLicense() => Size = 0x40;

        public override Models.License.TireCompoundLicense MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.License.TireCompoundLicense
            {
                Data = rawData
            };
    }
}