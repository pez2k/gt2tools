using System.IO;

namespace GT2.SaveEditor.GTMode.License
{
    public class LicenseData
    {
        public LicenseTestData[] Tests { get; set; } = new LicenseTestData[10];

        public void ReadFromSave(Stream file)
        {
            for (int i = 0; i < 10; i++)
            {
                Tests[i] = new LicenseTestData();
                Tests[i].ReadFromSave(file);
            }
        }

        public void WriteToSave(Stream file)
        {
            for (int i = 0; i < 10; i++)
            {
                Tests[i].WriteToSave(file);
            }
        }
    }
}