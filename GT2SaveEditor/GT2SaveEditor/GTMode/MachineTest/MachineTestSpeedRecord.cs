using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode.MachineTest
{
    public class MachineTestSpeedRecord : MachineTestRecord
    {
        public int Speed { get; set; }

        protected override void ReadSpecificDataFromSave(Stream file) => Speed = file.ReadInt();

        protected override void WriteSpecificDataToSave(Stream file) => file.WriteInt(Speed);
    }
}