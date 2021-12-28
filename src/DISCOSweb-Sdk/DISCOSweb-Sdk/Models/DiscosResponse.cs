using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models;

public class DiscosResponse<T>
{
	[JsonPropertyName("type")]
	public string Type { get; init; }
	
	[JsonPropertyName("attributes")]
	public T Attributes { get; init; }
	//TODO public List<Relationship> Relationships {get; init;}
}
