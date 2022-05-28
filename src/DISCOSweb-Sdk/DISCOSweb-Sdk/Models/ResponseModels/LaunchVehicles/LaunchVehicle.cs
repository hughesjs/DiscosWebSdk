using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;

namespace DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;

public record LaunchVehicle: DiscosModelBase
{
	[JsonPropertyName("failedLaunches")]
	public int? FailedLaunches { get; init; }
	[JsonPropertyName("successfulLaunches")]
	public int? SuccessfulLaunches { get; init; }
	[JsonPropertyName("numStages")]
	public int? NumStages { get; init; }
	
	[JsonPropertyName("geoCapacity")]
	public float? GeoCapacity { get; init; }
	[JsonPropertyName("escCapacity")]
	public float? EscCapacity { get; init; }
	[JsonPropertyName("ssoCapacity")]
	public float? SsoCapacity { get; init; }
	[JsonPropertyName("leoCapacity")]
	public float? LeoCapacity { get; init; }
	[JsonPropertyName("gtoCapacity")]
	public float? GtoCapacity { get; init; }
	
	[JsonPropertyName("height")]
	public float? Height { get; init; }
	[JsonPropertyName("diameter")]
	public float? Diameter { get; init; }
	[JsonPropertyName("mass")]
	public float? Mass { get; init; }
	[JsonPropertyName("thrustLevel")]
	public float? ThrustLevel { get; init; }
	
	[JsonPropertyName("stages")]
	public IReadOnlyList<LaunchVehicleStage> Stages { get; init; } = ArraySegment<LaunchVehicleStage>.Empty;
	[JsonPropertyName("launches")]
	public IReadOnlyList<Launch> Launches { get; init; } = ArraySegment<Launch>.Empty;
	[JsonPropertyName("engines")]
	public IReadOnlyList<LaunchVehicleEngine> Engines { get; init; } = ArraySegment<LaunchVehicleEngine>.Empty;
	[JsonPropertyName("family")]
	public LaunchVehicleFamily? Family { get; init; }
}
