using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode.MachineTest
{
    public class MachineTestTimeRecord : MachineTestRecord
    {
        public int Time { get; set; }

        protected override void ReadSpecificDataFromSave(Stream file) => Time = file.ReadInt();

        protected override void WriteSpecificDataToSave(Stream file) => file.WriteInt(Time);
    }
}