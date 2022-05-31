using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.ResponseModels.Entities;

public record Organisation: Entity
{
	
	[JsonPropertyName("hostCountry")]
	public Country? HostCountry { get; init; } 
}
