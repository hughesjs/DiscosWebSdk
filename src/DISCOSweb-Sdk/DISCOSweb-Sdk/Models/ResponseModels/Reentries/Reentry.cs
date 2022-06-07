using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;

namespace DISCOSweb_Sdk.Models.ResponseModels.Reentries;

public record Reentry : DiscosModelBase
{
	[JsonIgnore]
	public override string? Name => Epoch.ToString("O");

	[JsonPropertyName("epoch")]
	public DateTime Epoch { get; init; }

	[JsonPropertyName("objects")]
	public IReadOnlyList<DiscosObject> Objects { get; init; } = ArraySegment<DiscosObject>.Empty;
}
