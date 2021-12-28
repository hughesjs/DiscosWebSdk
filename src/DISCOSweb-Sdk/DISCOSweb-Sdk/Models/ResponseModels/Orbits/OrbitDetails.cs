using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Enums;

namespace DISCOSweb_Sdk.Models.ResponseModels.Orbits;

public record OrbitDetails: DiscosModelBase
{
	[JsonPropertyName("RightAscensionAscendingNode")]
	public float? RightAscensionAscendingNode { get; init; }
	[JsonPropertyName("inc")]
	public float? Inclination { get; init; }
	[JsonPropertyName("sma")]
	public float? SemiMajorAxis { get; init; }
	[JsonPropertyName("ecc")]
	public float? Eccentricity { get; init; }
	[JsonPropertyName("aPer")]
	public float? ArgumentOfPeriapsis { get; init; }
	[JsonPropertyName("mAno")]
	public float? MeanAnomaly { get; init; }
	[JsonPropertyName("frame")]
	public OrbitalFrame? Frame { get; init; }
	[JsonPropertyName("epoch")]
	public DateTime? Epoch { get; init; }
}
