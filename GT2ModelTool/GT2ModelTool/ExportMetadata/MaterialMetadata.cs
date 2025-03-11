using System.Text.Json.Serialization;

namespace GT2.ModelTool.ExportMetadata
{
    public record MaterialMetadata
    {
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsUntextured { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PaletteIndex { get; set; }

        public int RenderOrder { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsBrakeLight { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsMatte { get; set; }
    }
}