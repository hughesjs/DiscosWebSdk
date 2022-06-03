using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
public enum DiscosFunction
{
	[EnumMember(Value = "eq")]
	Equal,
	[EnumMember(Value = "ne")]
	NotEqual,
	[EnumMember(Value = "lt")]
	LessThan,
	[EnumMember(Value = "gt")]
	GreaterThan,
	[EnumMember(Value = "le")]
	LessThanOrEqual,
	[EnumMember(Value = "ge")]
	GreaterThanOrEqual,
	[EnumMember(Value = "in")]
	Includes,
	[EnumMember(Value = "out")]
	DoesNotInclude,
	[EnumMember(Value = "contains")]
	Contains,
	[EnumMember(Value = "excludes")]
	Excludes,
	[EnumMember(Value = "icontains")]
	InsensitiveContains,
	[EnumMember(Value = "iexcludes")]
	InsensitiveExcludes
}
