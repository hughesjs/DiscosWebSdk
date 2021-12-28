using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.JsonConverters;
using DISCOSweb_Sdk.Models.ResponseModels;

namespace DISCOSweb_Sdk.Models;

public record DiscosResponse<T> where T: DiscosModelBase
{
	[JsonPropertyName("type")]
	public ResponseType Type { get; init; }
	
	[JsonPropertyName("attributes")]
	public T Attributes { get; init; }
	
	[JsonPropertyName("id")]
	[JsonConverter(typeof(JsonStringIntConverter))]
	public int Id { get; init; }
}
