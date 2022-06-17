using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;

namespace DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;

public record LaunchVehicleFamily : DiscosModelBase
{
	[JsonPropertyName("vehicles")]
	public IReadOnlyList<LaunchVehicle> Vehicles { get; init; } = ArraySegment<LaunchVehicle>.Empty;

	[JsonPropertyName("system")]
	public LaunchSystem? System { get; init; }
}
