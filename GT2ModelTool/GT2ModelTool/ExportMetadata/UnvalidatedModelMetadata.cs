namespace GT2.ModelTool.ExportMetadata
{
    public class UnvalidatedModelMetadata
    {
        public string ModelFilename { get; set; } = "";
        public bool AllowUnmappedMaterials { get; set; }
        public bool MergeOverlappingFaces { get; set; }
        public UnvalidatedMenuWheelMetadata MenuWheels { get; set; } = new();
        public UnvalidatedLODMetadata LOD0 { get; set; } = new();
        public UnvalidatedLODMetadata LOD1 { get; set; } = new();
        public UnvalidatedLODMetadata LOD2 { get; set; } = new();
        public UnvalidatedShadowMetadata Shadow { get; set; } = new();
        public UnvalidatedMaterialMetadata[] Materials { get; set; } = [];
    }
}