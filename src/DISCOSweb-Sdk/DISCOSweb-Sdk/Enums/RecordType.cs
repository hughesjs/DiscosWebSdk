using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))] 
public enum RecordType
{
        [JsonPropertyName("country")]
        Country = 1,

        [JsonPropertyName("destinationOrbit")]
        DestinationOrbit = 2,

        [JsonPropertyName("engine")]
        Engine = 3,

        [JsonPropertyName("fragmentation")]
        Fragmentation = 4,

        [JsonPropertyName("fragmentationEventType")]
        FragmentationEventType = 5,

        [JsonPropertyName("initialOrbit")]
        InitialOrbit = 6,

        [JsonPropertyName("launch")]
        Launch = 7,

        [JsonPropertyName("launchSite")]
        LaunchSite = 8,

        [JsonPropertyName("launchSystem")]
        LaunchSystem = 9,

        [JsonPropertyName("object")]
        Object = 10,

        [JsonPropertyName("objectClass")]
        ObjectClass = 11,

        [JsonPropertyName("organisation")]
        Organisation = 12,

        [JsonPropertyName("propellant")]
        Propellant = 13,

        [JsonPropertyName("reentry")]
        Reentry = 14,

        [JsonPropertyName("stage")]
        Stage = 15,

        [JsonPropertyName("vehicle")]
        Vehicle = 16,

        [JsonPropertyName("vehicleFamily")]
        VehicleFamily = 17
}
