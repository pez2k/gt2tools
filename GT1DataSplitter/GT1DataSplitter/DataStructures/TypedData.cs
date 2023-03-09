using System;
using System.Collections.Generic;

namespace GT1.DataSplitter
{
    public class TypedData
    {
        public Type Type { get; }
        public List<DataStructure> Structures { get; }
        public List<List<string>> StringTables { get; set; }
        public int OrderOnDisk { get; }
        public bool IsLocalised { get; }

        public TypedData(Type type, int orderOnDisk, bool isLocalised)
        {
            Type = type;
            Structures = new List<DataStructure>();
            StringTables = new List<List<string>>();
            OrderOnDisk = orderOnDisk;
            IsLocalised = isLocalised;
        }
    }
}