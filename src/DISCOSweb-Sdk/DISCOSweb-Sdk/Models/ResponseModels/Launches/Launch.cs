using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;

namespace DISCOSweb_Sdk.Models.ResponseModels.Launches;


public record Launch: DiscosModelBase
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

/*
"relationships": {
                "objects": {
                    "links": {
                        "self": "/api/launches/6389/relationships/objects",
                        "related": "/api/launches/6389/objects"
                    }
                },
                "vehicle": {
                    "links": {
                        "self": "/api/launches/6389/relationships/vehicle",
                        "related": "/api/launches/6389/vehicle"
                    }
                },
                "site": {
                    "links": {
                        "self": "/api/launches/6389/relationships/site",
                        "related": "/api/launches/6389/site"
                    }
                },
                "entities": {
                    "links": {
                        "self": "/api/launches/6389/relationships/entities",
                        "related": "/api/launches/6389/entities"
                    }
                }
            },
*/
