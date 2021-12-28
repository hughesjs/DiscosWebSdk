using System.Diagnostics;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.ResponseModels.FragmentationEvent;

[DebuggerDisplay("{Epoch} -- {EventType}")]
public record FragmentationEvent: DiscosModelBase
{
	[JsonPropertyName("altitude")]
	public float? Altitude { get; init; }
	[JsonPropertyName("latitude")]
	public float? Latitude { get; init; }
	[JsonPropertyName("longitude")]
	public float? Longitude { get; init; }
	[JsonPropertyName("epoch")]
	public DateTime? Epoch { get; init; }
	[JsonPropertyName("comment")]
	public string? Comment { get; init; } 
	[JsonPropertyName("eventType")]
	public string? EventType { get; init; }

}
