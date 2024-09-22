using System.IO;

namespace GT2.SaveEditor.Arcade
{
    public class ArcadeRecords
    {
        public ArcadeRecord[] Records { get; set; } = new ArcadeRecord[126]; // One per track in .crsinfo, including tracks not usable in Arcade - beware that US 1.0 for example has less

        public void ReadFromSave(Stream file)
        {
            for (int i = 0; i < Records.Length; i++)
            {
                Records[i] = new ArcadeRecord();
                Records[i].ReadFromSave(file);
            }
        }

        public void WriteToSave(Stream file)
        {
            for (int i = 0; i < Records.Length; i++)
            {
                Records[i].WriteToSave(file);
            }
        }
    }
}