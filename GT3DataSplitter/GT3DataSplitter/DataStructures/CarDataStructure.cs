using System.IO;

namespace GT3.DataSplitter
{
    using StreamExtensions;

    public class CarDataStructure : DataStructure
    {
        public bool HasId { get; set; } = true;

        public override string CreateOutputFilename(byte[] data)
        {
            if (HasId)
            {
                ulong hexID = data.ReadULong();
                string id = Program.IDStrings.Get(hexID);
                if (!Directory.Exists(Name))
                {
                    Directory.CreateDirectory(Name);
                }
                return $"{Path.Combine(Name, id)}.dat";
            }

            string number = Directory.GetFiles(Name).Length.ToString();

            for (int i = number.Length; i < 4; i++)
            {
                number = "0" + number;
            }

            return Name + "\\" + number + "0.dat";
        }
    }
}
