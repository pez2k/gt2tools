using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode.MachineTest
{
    using CarNameConversion;

    public abstract class MachineTestRecord
    {
        private readonly byte[] junkData = new byte[12]; // If the player's name is less than the maximum 11 characters, the Machine Test copies junk data after it and never zeroes it out,
                                                         // so to round trip saves we need to keep a copy of that junk to put back...

        public string CarName { get; set; } = "";
        public string Name { get; set; } = "";

        public void ReadFromSave(Stream file)
        {
            uint carNameHash = file.ReadUInt();
            CarName = carNameHash == 0 ? "" : carNameHash.ToCarName();
            ReadSpecificDataFromSave(file);
            long stringStart = file.Position;
            file.Read(junkData);
            file.Position = stringStart;
            Name = file.ReadCharacters();
            file.Position = stringStart + 12;
        }

        public void WriteToSave(Stream file)
        {
            file.WriteUInt(CarName == "" ? 0 : CarName.ToCarID());
            WriteSpecificDataToSave(file);
            long stringStart = file.Position;
            file.Write(junkData);
            file.Position = stringStart;
            file.WriteCharacters(Name);
            file.WriteByte(0);
            file.Position = stringStart + 12;
        }

        protected abstract void ReadSpecificDataFromSave(Stream file);

        protected abstract void WriteSpecificDataToSave(Stream file);
    }
}