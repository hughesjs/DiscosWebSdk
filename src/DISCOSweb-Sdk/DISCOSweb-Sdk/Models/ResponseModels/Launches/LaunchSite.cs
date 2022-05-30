using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.SubObjects;
using JetBrains.Annotations;

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
	public double? LatitudeDegs => double.Parse(Latitude); // TODO - Work out how to map these directly

	[JsonPropertyName("longitude")]
	public double? LongitudeDegs => double.Parse(Longitude); // TODO - Work out how to map these directly
	
	[JsonPropertyName("constraints")]
	public List<string>? Constraints { get; init; }
	
	[JsonPropertyName("launches")]
	public IReadOnlyList<Launch> Launches { get; init; } = ArraySegment<Launch>.Empty;
	
	[JsonPropertyName("operators")]
	public IReadOnlyList<Entity> Operators { get; init; } = ArraySegment<Entity>.Empty;
	
	
	// These are needed because the API returns these as strings and I can't see a 
	// Way to recast as part of the Hypermedia pipeline
	private string Longitude { get; [UsedImplicitly] init; } = "0";

	private string Latitude { get; [UsedImplicitly] init; } = "0";
}
