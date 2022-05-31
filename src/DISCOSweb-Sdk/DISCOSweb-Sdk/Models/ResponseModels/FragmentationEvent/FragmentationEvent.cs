using System.Diagnostics;
using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using JetBrains.Annotations;

namespace DISCOSweb_Sdk.Models.ResponseModels.FragmentationEvent;

[DebuggerDisplay("{Epoch} -- {EventType}")]
public record FragmentationEvent: DiscosModelBase
{
	[JsonPropertyName("altitude")]
	public float? Altitude { get; init; }
	[JsonPropertyName("latitude")]
	public float? LatitudeDegs => float.Parse(Latitude);

	[JsonPropertyName("longitude")]
	public float? LongitudeDegs => float.Parse(Longitude);
	[JsonPropertyName("epoch")]
	public DateTime? Epoch { get; init; }
	[JsonPropertyName("comment")]
	public string? Comment { get; init; } 
	[JsonPropertyName("eventType")]
	public string? EventType { get; init; }
	
	[JsonPropertyName("objects")]
	public IReadOnlyList<DiscosObject> Objects { get; init; } = ArraySegment<DiscosObject>.Empty;

	internal string Latitude { get; [UsedImplicitly] init; } = "0";
	internal string Longitude { get; [UsedImplicitly] init; } = "0";

}
