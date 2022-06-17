using System.Text.Json.Serialization;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;

namespace DiscosWebSdk.Models.ResponseModels.Launches;

public record LaunchSystem : DiscosModelBase
{
	[JsonPropertyName("families")]
	public IReadOnlyList<LaunchVehicleFamily> Families { get; init; } = ArraySegment<LaunchVehicleFamily>.Empty;

	[JsonPropertyName("entities")]
	public IReadOnlyList<Entity> Entities { get; init; } = ArraySegment<Entity>.Empty;
}
