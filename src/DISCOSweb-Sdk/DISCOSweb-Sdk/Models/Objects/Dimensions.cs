using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.Objects;

public class Dimensions
{
	[JsonPropertyName("length")]
	private float Length { get; set; }
	[JsonPropertyName("height")]
	private float Height { get; set; }
	[JsonPropertyName("depth")]
	private float Depth { get; set; }
}
