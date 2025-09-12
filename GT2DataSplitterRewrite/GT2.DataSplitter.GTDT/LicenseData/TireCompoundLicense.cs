namespace GT2.DataSplitter.GTDT.LicenseData
{
    using Common;

    public class TireCompoundLicense : TireCompound
    {
        public override Models.License.TireCompoundLicense MapToModel() =>
            new Models.License.TireCompoundLicense
            {
                Data = rawData
            };
    }
}