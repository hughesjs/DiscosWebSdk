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
	public string? Latitude { get; init; } // TODO - Work out how to map this as a double

	[JsonPropertyName("longitude")]
	public string? Longitude { get; init; } // TODO - Work out how to map this as a double
	
	[JsonPropertyName("constraints")]
	public List<string>? Constraints { get; init; }
	
	[JsonPropertyName("launches")]
	public IReadOnlyList<Launch> Launches { get; init; } = ArraySegment<Launch>.Empty;
	
	[JsonPropertyName("operators")]
	public IReadOnlyList<Entity> Operators { get; init; } = ArraySegment<Entity>.Empty;
	
	
	// These are needed because the API returns these as strings and I can't see a 
	// Way to recast as part of the Hypermedia pipeline
	// internal string LongitudeAsString
	// {
	// 	init => Longitude = double.Parse(value);
	// }
	//
	// internal string LatitudeAsString
	// {
	// 	init => Latitude = double.Parse(value);
	// }
}
