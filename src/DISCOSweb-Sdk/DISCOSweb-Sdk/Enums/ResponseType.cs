using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
public enum ResponseType
{
	[EnumMember(Value = "object")]
	DiscosObject,
	[EnumMember(Value = "launchSystem")]
	LaunchSystem,
	[EnumMember(Value = "launch")]
	Launch,
	[EnumMember(Value = "launchSite")]
	LaunchSite,
	[EnumMember(Value = "objectClass")]
	ObjectClass,
	[EnumMember(Value = "vehicleFamily")]
	VehicleFamily,
	[EnumMember(Value = "stage")]
	Stage,
	[EnumMember(Value = "engine")]
	Engine,
	[EnumMember(Value = "vehicle")]
	Vehicle,
	[EnumMember(Value = "propellant")]
	Propellant,
	[EnumMember(Value = "reentry")]
	Reentry,
	[EnumMember(Value = "initialOrbit")]
	InitialOrbit,
	[EnumMember(Value = "destinationOrbit")]
	DestinationOrbit,
	[EnumMember(Value = "fragmentationEventType")]
	FragmentationEventType,
	[EnumMember(Value = "country")]
	Country,
	[EnumMember(Value = "organisation")]
	Organisation
	
}
