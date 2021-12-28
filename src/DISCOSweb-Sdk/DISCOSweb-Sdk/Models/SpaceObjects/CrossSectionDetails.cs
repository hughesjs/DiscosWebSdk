using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.SpaceObjects;

/// <summary>
/// Details of the object's cross sectional area in m2
/// </summary>
public record CrossSectionDetails
{
	[JsonPropertyName("xSectionMax")]
	public double Maximum { get; init; }
	[JsonPropertyName("xSectionMin")]
	public double Minimum { get; init; }
	[JsonPropertyName("xSectionAvg")]
	public double Average { get; init; }
}
