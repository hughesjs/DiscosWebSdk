using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;

namespace DISCOSweb_Sdk.Models.ResponseModels.Launches;

public record LaunchSystem : DiscosModelBase
{
	[JsonPropertyName("families")]
	public IReadOnlyList<LaunchVehicleFamily> Families { get; init; } = ArraySegment<LaunchVehicleFamily>.Empty;

	[JsonPropertyName("entities")]
	public IReadOnlyList<Entity> Entities { get; init; } = ArraySegment<Entity>.Empty;
}
