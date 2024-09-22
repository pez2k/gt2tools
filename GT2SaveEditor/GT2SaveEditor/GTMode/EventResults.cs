using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode
{
    public class EventResults
    {
        private const int EventCount = 248;

        public EventResultEnum[] Results { get; set; } = new EventResultEnum[EventCount];

        public void ReadFromSave(Stream file)
        {
            for (int i = 0; i < EventCount / 2; i++)
            {
                byte resultPair = file.ReadSingleByte();
                Results[i] = (EventResultEnum)(resultPair & 0x0F);
                Results[i + 1] = (EventResultEnum)((resultPair & 0xF0) >> 4);
            }
            file.Position += 0x4;
        }

        public void WriteToSave(Stream file)
        {
            for (int i = 0; i < EventCount / 2; i++)
            {
                file.WriteByte((byte)((byte)Results[i + 1] << 4 | (byte)Results[i]));
            }
            file.Position += 0x4;
        }
    }
}