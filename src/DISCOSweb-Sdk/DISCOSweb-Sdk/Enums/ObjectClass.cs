using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
public enum ObjectClass
{
	[EnumMember(Value = "Rocket Body")]
	RocketBody,
	[EnumMember(Value = "Rocket Debris")]
	RocketDebris,
	[EnumMember(Value = "Rocket Fragmentation Debris")]
	RocketFragmentationDebris,
	[EnumMember(Value = "Rocket Mission Related Object")]
	RocketMissionRelatedObject,

	[EnumMember(Value = "Payload")]
	Payload,
	[EnumMember(Value = "Payload Debris")]
	PayloadDebris,
	[EnumMember(Value = "Payload Fragmentation Debris")]
	PayloadFragmentationDebris,
	[EnumMember(Value = "Payload Mission Related Object")]
	PayloadMissionRelatedObject,

	[EnumMember(Value = "Other Debris")]
	OtherDebris,
	[EnumMember(Value = "Other Mission Related Object")]
	OtherMissionRelatedObject,

	[EnumMember(Value = "Unknown")]
	Unknown
}
