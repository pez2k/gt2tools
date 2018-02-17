using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GT2DataSplitter
{
    public class Car : CarDataStructure
    {
        public Car() : base(0x48)
        {
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Utils.GetCarName(data.ReadUInt()) + ".dat";
        }

        /*public override void ExportStructure(byte[] structure, FileStream output)
        {
            GCHandle handle = GCHandle.Alloc(structure, GCHandleType.Pinned);
            Data = (StructureData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(StructureData));
            handle.Free();

            var newData = new List<string>();

            Type dataType = typeof(StructureData);
            foreach (FieldInfo field in dataType.GetFields())
            {
                newData.Add(field.GetValue(Data).ToString());
            }

            base.ExportStructure(structure, output);
        }*/

        public StructureData Data { get; set; }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct StructureData
        {
            public uint CarId; // standard thing (0)
            public ushort Brakes; // (4)
            public uint IsSpecial; // 1 for special cars, 0 for not
            public ushort WeightReduction; // (a)
            public ushort Body; // (c)
            public ushort WeightDistribution;// (e)
            public ushort Engine; // (10)
            public ushort Unknown1; // 12
            public ushort Unknown2; // 14
            public ushort Unknown3; // 16
            public ushort Unknown4; // 18
            public ushort NATuning; // 1a - this and the last one are probably 4 shorts, but they're always 0 so it doesn't really matter
            public ushort TurboKit; // 1c
            public ushort Drivetrain; // 1e
            public ushort Flywheel; // 20
            public ushort Clutch; // 22
            public ushort Unknown5; // 24
            public ushort Differential; // 26
            public ushort Transmission; // 28
            public ushort Suspension; // 2a
            public ushort Unknown6; // 2c
            public ushort Unknown7; // 2e
            public ushort FrontTyres; // 30
            public ushort RearTyres; // 32
            public ushort Unknown8; // 34
            public ushort Unknown9; // 36
            public ushort RimsCode3; // 38
            public ushort ManufacturerID; // 0x3a
            public ushort NameFirstPart; // 0x3c unistrdb index
            public ushort NameSecondPart; // 0x3e unistrdb index
            public byte IsSpecial2; // 0x40 (1 for special  cars, 0 for not)
            public byte Year; // 0x41 
            public ushort Unknown10; // 42
            public uint Price; // 0x44
        }

    }
}
