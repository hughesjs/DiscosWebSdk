using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
public enum OrbitalFrame
{
	[EnumMember(Value = "J2000")]
	J2000,
	[EnumMember(Value = "True Equator, Mean Equinox")]
	TrueEquatorMeanEquinox
}
