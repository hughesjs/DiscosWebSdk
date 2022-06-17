using System.Text.Json.Serialization;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;

namespace DiscosWebSdk.Models.ResponseModels.Reentries;

public record Reentry : DiscosModelBase
{
	[JsonIgnore]
	public override string? Name => Epoch.ToString("O");

	[JsonPropertyName("epoch")]
	public DateTime Epoch { get; init; }

	[JsonPropertyName("objects")]
	public IReadOnlyList<DiscosObject> Objects { get; init; } = ArraySegment<DiscosObject>.Empty;
}
