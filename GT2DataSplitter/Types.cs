using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT2DataSplitter
{
    public unsafe struct CarBrakes
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarBrakeBalanceController
    {
        public fixed byte test[0x10];
    }

    public unsafe struct CarSteering
    {
        public fixed byte test[0x18]; // TODO
    }

    public unsafe struct CarDimensions
    {
        public fixed byte test[0x14];
    }

    public unsafe struct CarWeightReduction
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarRacingModification
    {
        public fixed byte test[0x1C];
    }

    public unsafe struct CarEngineData
    {
        public fixed byte test[0x4C];
    }

    public unsafe struct CarPortGrinding
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarEngineBalancing
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarDisplacementIncrease
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarChip
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarNATuning
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarTurboTuning
    {
        public fixed byte test[0x14];
    }

    public unsafe struct CarDrivetrain
    {
        public fixed byte test[0x10];
    }

    public unsafe struct CarFlywheel
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarClutch
    {
        public fixed byte test[0x10];
    }

    public unsafe struct CarPropshaft
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarGearbox
    {
        public fixed byte test[0x24];
    }

    public unsafe struct CarSuspension
    {
        public fixed byte test[0x4C];
    }

    public unsafe struct CarIntercooler
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarMuffler
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarLSD
    {
        public fixed byte test[0x20];
    }

    public unsafe struct CarTyres
    {
        public fixed byte test[0x10];
    }

    public unsafe struct CarRearTyres
    {
        public fixed byte test[0xC];
    }

    public unsafe struct CarUnknown1
    {
        public fixed byte test[0x364];
    }

    public unsafe struct CarUnknown2
    {
        public fixed byte test[0x500];
    }

    public unsafe struct CarUnknown3
    {
        public fixed byte test[0x8];
    }

    public unsafe struct CarUnknown4
    {
        public fixed byte test[0x30];
    }

    public unsafe struct CarUnknown5
    {
        public fixed byte test[0x30];
    }

    public unsafe struct CarUnknown6
    {
        public fixed byte test[0x628];
    }

    public unsafe struct CarData
    {
        public fixed byte test[0x48];
    }
}
