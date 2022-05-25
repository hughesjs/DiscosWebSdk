using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;

namespace DISCOSweb_Sdk.Models.ResponseModels.Entities;

public record Organisation: Entity
{
	
	[JsonPropertyName("hostCountry")]
	public Country? HostCountry { get; init; } 
}
