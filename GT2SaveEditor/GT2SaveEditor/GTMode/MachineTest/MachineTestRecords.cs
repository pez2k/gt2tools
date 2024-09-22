using System.IO;

namespace GT2.SaveEditor.GTMode.MachineTest
{
    public class MachineTestRecords
    {
        public MachineTestRecordList<MachineTestTimeRecord> MachineTest400mRecords { get; set; } = new();
        public MachineTestRecordList<MachineTestTimeRecord> MachineTest1000mRecords { get; set; } = new();
        public MachineTestRecordList<MachineTestSpeedRecord> MachineTestMaxSpeedRecords { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            MachineTest400mRecords.ReadFromSave(file);
            MachineTest1000mRecords.ReadFromSave(file);
            MachineTestMaxSpeedRecords.ReadFromSave(file);
        }

        public void WriteToSave(Stream file)
        {
            MachineTest400mRecords.WriteToSave(file);
            MachineTest1000mRecords.WriteToSave(file);
            MachineTestMaxSpeedRecords.WriteToSave(file);
        }
    }
}