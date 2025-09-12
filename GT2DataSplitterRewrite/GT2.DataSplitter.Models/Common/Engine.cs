namespace GT2.DataSplitter.Models.Common
{
    public class Engine
    {
        public string CarId { get; set; } = "";
        public string LayoutName { get; set; } = "";
        public string ValvetrainName { get; set; } = "";
        public string Aspiration { get; set; } = "";
        public ushort SoundFile { get; set; }
        public short TorqueCurve1 { get; set; }
        public short TorqueCurve2 { get; set; }
        public short TorqueCurve3 { get; set; }
        public short TorqueCurve4 { get; set; }
        public short TorqueCurve5 { get; set; }
        public short TorqueCurve6 { get; set; }
        public short TorqueCurve7 { get; set; }
        public short TorqueCurve8 { get; set; }
        public short TorqueCurve9 { get; set; }
        public short TorqueCurve10 { get; set; }
        public short TorqueCurve11 { get; set; }
        public short TorqueCurve12 { get; set; }
        public short TorqueCurve13 { get; set; }
        public short TorqueCurve14 { get; set; }
        public short TorqueCurve15 { get; set; }
        public short TorqueCurve16 { get; set; }
        public string Displacement { get; set; } = "";
        public ushort DisplayedPower { get; set; }
        public ushort MaxPowerRPM { get; set; }
        public ushort DisplayedTorque { get; set; }
        public string MaxTorqueRPMName { get; set; } = "";
        public byte PowerMultiplier { get; set; }
        public byte ClutchReleaseRPM { get; set; }
        public byte IdleRPM { get; set; }
        public byte MaxRPM { get; set; }
        public byte RedlineRPM { get; set; }
        public byte TorqueCurveRPM1 { get; set; }
        public byte TorqueCurveRPM2 { get; set; }
        public byte TorqueCurveRPM3 { get; set; }
        public byte TorqueCurveRPM4 { get; set; }
        public byte TorqueCurveRPM5 { get; set; }
        public byte TorqueCurveRPM6 { get; set; }
        public byte TorqueCurveRPM7 { get; set; }
        public byte TorqueCurveRPM8 { get; set; }
        public byte TorqueCurveRPM9 { get; set; }
        public byte TorqueCurveRPM10 { get; set; }
        public byte TorqueCurveRPM11 { get; set; }
        public byte TorqueCurveRPM12 { get; set; }
        public byte TorqueCurveRPM13 { get; set; }
        public byte TorqueCurveRPM14 { get; set; }
        public byte TorqueCurveRPM15 { get; set; }
        public byte TorqueCurveRPM16 { get; set; }
        public byte TorqueCurvePoints { get; set; }
    }
}