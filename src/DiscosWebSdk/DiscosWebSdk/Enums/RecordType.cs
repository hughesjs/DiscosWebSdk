using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DiscosWebSdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
public enum RecordType
{
	[EnumMember(Value = "country")]
	Country = 1,

	[EnumMember(Value = "destinationOrbit")]
	DestinationOrbit = 2,

	[EnumMember(Value = "engine")]
	Engine = 3,

	[EnumMember(Value = "fragmentation")]
	Fragmentation = 4,

	[EnumMember(Value = "fragmentationEventType")]
	FragmentationEventType = 5,

	[EnumMember(Value = "initialOrbit")]
	InitialOrbit = 6,

	[EnumMember(Value = "launch")]
	Launch = 7,

	[EnumMember(Value = "launchSite")]
	LaunchSite = 8,

	[EnumMember(Value = "launchSystem")]
	LaunchSystem = 9,

	[EnumMember(Value = "object")]
	Object = 10,

	[EnumMember(Value = "objectClass")]
	ObjectClass = 11,

	[EnumMember(Value = "organisation")]
	Organisation = 12,

	[EnumMember(Value = "propellant")]
	Propellant = 13,

	[EnumMember(Value = "reentry")]
	Reentry = 14,

	[EnumMember(Value = "stage")]
	Stage = 15,

	[EnumMember(Value = "vehicle")]
	Vehicle = 16,

	[EnumMember(Value = "vehicleFamily")]
	VehicleFamily = 17
}
