namespace GT2.ModelTool.ExportMetadata
{
    public record UnvalidatedMaterialMetadata
    {
        public string Name { get; set; } = "";
        public bool IsUntextured { get; set; }
        public double? PaletteIndex { get; set; }
        public double? RenderOrder { get; set; }
        public bool IsBrakeLight { get; set; }
        public bool IsMatte { get; set; }
    }
}