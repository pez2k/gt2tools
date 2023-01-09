using System;
using System.Collections.Generic;

namespace GT2.DataSplitter
{
    public class TypedData
    {
        public Type Type { get; }
        public List<DataStructure> Structures { get; }
        public int OrderOnDisk { get; }
        public bool IsLocalised { get; }

        public TypedData(Type type, int orderOnDisk, bool isLocalised)
        {
            Type = type;
            Structures = new List<DataStructure>();
            OrderOnDisk = orderOnDisk;
            IsLocalised = isLocalised;
        }
    }
}