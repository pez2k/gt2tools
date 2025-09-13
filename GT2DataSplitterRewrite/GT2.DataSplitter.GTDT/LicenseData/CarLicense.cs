namespace GT2.DataSplitter.GTDT.LicenseData
{
    public class CarLicense : DataStructureWithModel<Models.License.CarLicense>
    {
        public CarLicense() => Size = 0x60;

        public override Models.License.CarLicense MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.License.CarLicense
            {
                Data = rawData
            };
    }
}