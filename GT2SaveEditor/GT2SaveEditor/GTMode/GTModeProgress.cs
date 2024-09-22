using System.IO;

namespace GT2.SaveEditor.GTMode
{
    using Garage;
    using License;
    using MachineTest;

    public class GTModeProgress
    {
        public LicenseData SLicense { get; set; } = new();
        public LicenseData IALicense { get; set; } = new();
        public LicenseData IBLicense { get; set; } = new();
        public LicenseData ICLicense { get; set; } = new();
        public LicenseData ALicense { get; set; } = new();
        public LicenseData BLicense { get; set; } = new();
        public MachineTestRecords MachineTestRecords { get; set; } = new();
        public GarageData Garage { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            SLicense.ReadFromSave(file);
            IALicense.ReadFromSave(file);
            IBLicense.ReadFromSave(file);
            ICLicense.ReadFromSave(file);
            ALicense.ReadFromSave(file);
            BLicense.ReadFromSave(file);
            MachineTestRecords.ReadFromSave(file);
            Garage.ReadFromSave(file);
        }

        public void WriteToSave(Stream file)
        {
            SLicense.WriteToSave(file);
            IALicense.WriteToSave(file);
            IBLicense.WriteToSave(file);
            ICLicense.WriteToSave(file);
            ALicense.WriteToSave(file);
            BLicense.WriteToSave(file);
            MachineTestRecords.WriteToSave(file);
            Garage.WriteToSave(file);
        }
    }
}