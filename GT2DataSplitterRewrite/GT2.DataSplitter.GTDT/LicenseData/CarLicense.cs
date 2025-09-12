namespace GT2.DataSplitter.GTDT.LicenseData
{
    public class CarLicense : DataStructure
    {
        public CarLicense() => Size = 0x60;

        public Models.License.CarLicense MapToModel() =>
            new Models.License.CarLicense
            {
                Data = rawData
            };
    }
}