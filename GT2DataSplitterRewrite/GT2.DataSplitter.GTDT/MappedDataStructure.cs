using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT
{
    public abstract class MappedDataStructure<TStructure> : DataStructure
    {
        protected TStructure? data;

        protected MappedDataStructure() => Size = Marshal.SizeOf<TStructure>();

        public override void Read(Stream infile)
        {
            base.Read(infile);

            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            data = (TStructure?)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TStructure));
            handle.Free();
        }

        public override void Write(Stream outfile)
        {
            if (data == null)
            {
                return;
            }

            int size = Marshal.SizeOf(data);
            rawData = new byte[size];

            IntPtr objectPointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, objectPointer, true);
            Marshal.Copy(objectPointer, rawData, 0, size);
            Marshal.FreeHGlobal(objectPointer);

            base.Write(outfile);
        }
    }
}