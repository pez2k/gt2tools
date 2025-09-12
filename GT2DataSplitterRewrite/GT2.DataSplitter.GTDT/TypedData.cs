namespace GT2.DataSplitter.GTDT
{
    public class TypedData
    {
        public Type Type { get; }
        public List<DataStructure> Structures { get; } = [];

        public TypedData(Type type) => Type = type;
    }
}