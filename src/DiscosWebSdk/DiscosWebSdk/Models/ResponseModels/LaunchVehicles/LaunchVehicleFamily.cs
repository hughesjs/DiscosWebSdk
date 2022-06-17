using System.Text.Json.Serialization;
using DiscosWebSdk.Models.ResponseModels.Launches;

namespace DiscosWebSdk.Models.ResponseModels.LaunchVehicles;

public record LaunchVehicleFamily : DiscosModelBase
{
	[JsonPropertyName("vehicles")]
	public IReadOnlyList<LaunchVehicle> Vehicles { get; init; } = ArraySegment<LaunchVehicle>.Empty;

	[JsonPropertyName("system")]
	public LaunchSystem? System { get; init; }
}
