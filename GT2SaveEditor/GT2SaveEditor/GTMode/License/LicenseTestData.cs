using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode.License
{
    public class LicenseTestData
    {
        public LicenseTestResultEnum BestResult { get; set; }
        public LicenseTestRecord[] Records { get; set; } = new LicenseTestRecord[5];

        public void ReadFromSave(Stream file)
        {
            file.Position += 0x1;
            BestResult = (LicenseTestResultEnum)file.ReadSingleByte();
            file.Position += 0x2; // Mystery value

            for (int i = 0; i < 5; i++)
            {
                Records[i] = new LicenseTestRecord();
                Records[i].ReadTimeAndSpeedFromSave(file);
            }

            for (int i = 0; i < 5; i++)
            {
                Records[i].ReadNameFromSave(file);
            }
        }

        public void WriteToSave(Stream file)
        {
            file.Position += 0x1;
            file.WriteByte((byte)BestResult);
            file.Position += 0x2; // Mystery value

            for (int i = 0; i < 5; i++)
            {
                Records[i].WriteTimeAndSpeedToSave(file);
            }

            for (int i = 0; i < 5; i++)
            {
                Records[i].WriteNameToSave(file);
            }
        }
    }
}