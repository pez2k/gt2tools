using System.Text.Json.Serialization;

namespace GT2.ModelTool.ExportMetadata
{
    public class ModelMetadata
    {
        public string ModelFilename { get; set; } = "";

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool AllowUnmappedMaterials { get; set; }

        public MenuWheelMetadata MenuWheels { get; set; } = new();
        public LODMetadata LOD0 { get; set; } = new();
        public LODMetadata LOD1 { get; set; } = new();
        public LODMetadata LOD2 { get; set; } = new();
        public ShadowMetadata Shadow { get; set; } = new();
        public MaterialMetadata[] Materials { get; set; } = [];
    }
}