using System.Text.Json.Serialization;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;

namespace DiscosWebSdk.Models.ResponseModels.Launches;

public record Launch : DiscosModelBase
{
	[JsonPropertyName("flightNo")]
	public string? FlightNo { get; init; }

	[JsonPropertyName("epoch")]
	public DateTime? Epoch { get; init; }

	[JsonPropertyName("cosparLaunchNo")]
	public string? CosparLaunchNo { get; init; }

	[JsonPropertyName("failure")]
	public bool Failure { get; init; }

	[JsonPropertyName("objects")]
	public IReadOnlyList<DiscosObject> Objects { get; init; } = ArraySegment<DiscosObject>.Empty;

	[JsonPropertyName("entities")]
	public IReadOnlyList<Entity> Entities { get; init; } = ArraySegment<Entity>.Empty;

	[JsonPropertyName("vehicle")]
	public LaunchVehicle Vehicle { get; init; } = null!;

	[JsonPropertyName("site")]
	public LaunchSite Site { get; init; } = null!;
}
