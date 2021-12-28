using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;

public record LaunchVehicleEngine: DiscosModelBase
{
	[JsonPropertyName("maxIsp")]
	public float? MaxIsp { get; init; }
	[JsonPropertyName("mass")]
	public float? Mass { get; init; }
	[JsonPropertyName("diameter")]
	public float? Diameter { get; init; }
	[JsonPropertyName("thrustLevel")]
	public float? ThrustLevel { get; init; }
	[JsonPropertyName("height")]
	public float? Height { get; init; }
}
