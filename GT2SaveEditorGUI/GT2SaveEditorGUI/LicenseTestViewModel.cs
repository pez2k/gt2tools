using GT2.SaveEditor.GTMode.License;

namespace GT2.SaveEditor.GUI
{
    public class LicenseTestViewModel
    {
        public string Name { get; set; }
        public LicenseTestData Data { get; set; }

        public LicenseTestViewModel(string name, LicenseTestData data)
        {
            Name = name;
            Data = data;
        }
    }
}