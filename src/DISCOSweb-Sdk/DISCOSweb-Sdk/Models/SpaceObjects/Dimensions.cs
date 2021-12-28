using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.SpaceObjects;

public class Dimensions
{
	[JsonPropertyName("length")]
	public double Length { get; set; }
	[JsonPropertyName("height")]
	public double Height { get; set; }
	[JsonPropertyName("depth")]
	public double Depth { get; set; }
}
