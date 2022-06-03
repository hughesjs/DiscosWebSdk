using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
public enum NodeOperation
{
	[EnumMember(Value = "and")]
	And,
	[EnumMember(Value = "or")]
	Or
}
