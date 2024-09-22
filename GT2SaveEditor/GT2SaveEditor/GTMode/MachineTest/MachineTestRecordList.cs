using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode.MachineTest
{
    public class MachineTestRecordList<TRecord> where TRecord : MachineTestRecord, new()
    {
        public uint RecordCount { get; set; }
        public TRecord[] Records { get; set; } = new TRecord[8];

        public void ReadFromSave(Stream file)
        {
            RecordCount = file.ReadUInt();
            for (int i = 0; i < Records.Length; i++)
            {
                Records[i] = new TRecord();
                Records[i].ReadFromSave(file);
            }
        }

        public void WriteToSave(Stream file)
        {
            file.WriteUInt(RecordCount);
            for (int i = 0; i < Records.Length; i++)
            {
                Records[i].WriteToSave(file);
            }
        }
    }
}