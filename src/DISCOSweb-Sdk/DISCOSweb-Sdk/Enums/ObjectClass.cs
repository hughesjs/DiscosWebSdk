using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ObjectClass
{
	[JsonPropertyName("Rocket Body")]
	RocketBody,
	[JsonPropertyName("Rocket Debris")]
	RocketDebris,
	[JsonPropertyName("Rocket Fragmentation Debris")]
	RocketFragmentationDebris,
	[JsonPropertyName("Rocket Mission Related Object")]
	RocketMissionRelatedObject,
	
	[JsonPropertyName("Payload")]
	Payload,
	[JsonPropertyName("Payload Debris")]
	PayloadDebris,
	[JsonPropertyName("Payload Fragmentation Debris")]
	PayloadFragmentationDebris,
	[JsonPropertyName("Payload Mission Related Object")]
	PayloadMissionRelatedObject,
	
	[JsonPropertyName("Other Debris")]
	OtherDebris,
	[JsonPropertyName("Other Mission Related Object")]
	OtherMissionRelatedObject,
	
	[JsonPropertyName("Unknown")]
	Unknown
}
