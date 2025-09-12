namespace GT2.DataSplitter.GTDT
{
    using StreamExtensions;

    public class DataFileWithStrings<TModel> : DataFile<TModel> where TModel : notnull
    {
        protected virtual int StringTableIndexPosition { get; } = -1;
        protected readonly ASCIIStringTable strings = new();

        public DataFileWithStrings(params Type[] dataTypes) : base(dataTypes)
        {
        }

        protected override void ReadDataFromFile(Stream file)
        {
            base.ReadDataFromFile(file);
            if (StringTableIndexPosition > 0)
            {
                file.Position = StringTableIndexPosition;
            }
            uint blockStart = file.ReadUInt();
            uint blockSize = file.ReadUInt(); // unused
            strings.Read(file, blockStart);
        }

        protected override void WriteDataToFile(Stream file)
        {
            base.WriteDataToFile(file);
            strings.Write(file, StringTableIndexPosition > 0 ? StringTableIndexPosition : file.Position);
        }

        public override void MapToModel(TModel model, UnicodeStringTable strings) => Mapper.MapToModel(data, model, strings, this.strings);
    }
}