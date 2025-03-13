namespace GT2.ModelTool.ExportMetadata
{
    public class ModelMetadata
    {
        public string ModelFilename { get; set; } = "";
        public ushort MenuFrontWheelRadius { get; set; }
        public ushort MenuFrontWheelWidth { get; set; }
        public ushort MenuRearWheelRadius { get; set; }
        public ushort MenuRearWheelWidth { get; set; }
        public WheelMetadata WheelFrontLeft { get; set; } = new();
        public WheelMetadata WheelFrontRight { get; set; } = new();
        public WheelMetadata WheelRearLeft { get; set; } = new();
        public WheelMetadata WheelRearRight { get; set; } = new();
        public LODMetadata LOD0 { get; set; } = new();
        public LODMetadata LOD1 { get; set; } = new();
        public LODMetadata LOD2 { get; set; } = new();
        public ShadowMetadata Shadow { get; set; } = new();
        public MaterialMetadata[] Materials { get; set; } = [];
    }
}