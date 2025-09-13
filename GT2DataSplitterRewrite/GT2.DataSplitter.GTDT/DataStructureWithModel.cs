namespace GT2.DataSplitter.GTDT
{
    public abstract class DataStructureWithModel<TModel> : DataStructure
    {
        public abstract TModel MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii);
    }
}