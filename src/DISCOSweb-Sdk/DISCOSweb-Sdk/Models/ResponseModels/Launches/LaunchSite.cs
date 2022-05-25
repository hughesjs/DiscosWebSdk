using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.SubObjects;

namespace DISCOSweb_Sdk.Models.ResponseModels.Launches;

public record LaunchSite: DiscosModelBase
{
	[JsonPropertyName("altitude")]
	public float? Altitude { get; init; }
	
	[JsonPropertyName("pads")]
	public List<string>? Pads { get; init; }
	
	[JsonPropertyName("azimuths")]
	public List<Azimuth>? Azimuths { get; init; }
	
	[JsonPropertyName("latitude")]
	public double? Latitude { get; init; }
	
	[JsonPropertyName("longitude")]
	public double? Longitude { get; init; }
	
	[JsonPropertyName("constraints")]
	public List<string>? Constraints { get; init; }
	
	[JsonPropertyName("launches")]
	public IReadOnlyCollection<Launch> Launches { get; init; } = ArraySegment<Launch>.Empty;
	
	[JsonPropertyName("operators")]
	public IReadOnlyCollection<Entity> Operators { get; init; } = ArraySegment<Entity>.Empty;

}
