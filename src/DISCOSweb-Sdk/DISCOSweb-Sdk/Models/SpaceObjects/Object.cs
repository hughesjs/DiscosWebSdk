using System.Diagnostics;
using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Enums;

namespace DISCOSweb_Sdk.Models.SpaceObjects;

/// <summary>
/// This is actually just an Object in DISCOS but for all that is unholy I'm not calling it that
/// </summary>
[DebuggerDisplay($"{nameof(CosparId)}: {nameof(Name)} -- {nameof(ObjectClass)}")]
public class SpaceObject
{
	[JsonPropertyName("name")]
	public string? Name { get; init; }
	
	[JsonPropertyName("cosparId")]
	public string? CosparId { get; init; }
	
	[JsonPropertyName("vimpelId")]
	public string? VimpelId { get; init; }

	[JsonPropertyName("satno")]
	public int? SatNo { get; init; }
	
	[JsonPropertyName("shape")]
	public string? Shape { get; init; }
	
	[JsonPropertyName("mass")]
	public string? Mass { get; init; }
	
	public Dimensions? Dimensions { get; init; }

	public CrossSectionDetails? CrossSection { get; init; }
	
	[JsonPropertyName("objectClass")]
	public ObjectClass? ObjectClass { get; init; }
}
