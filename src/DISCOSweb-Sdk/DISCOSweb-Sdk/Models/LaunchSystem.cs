using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models;

public class LaunchSystem
{
	[JsonPropertyName("name")]
	public string Name { get; init; }
}
