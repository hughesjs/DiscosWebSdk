using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DiscosWebSdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
public enum FilterOperation
{
	[EnumMember(Value = "and")]
	And,
	[EnumMember(Value = "or")]
	Or
}
