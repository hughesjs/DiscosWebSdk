using System.Diagnostics;
using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Enums;

namespace DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;

/// <summary>
/// This is actually just an Object in DISCOS but for all that is unholy I'm not calling it that
/// </summary>
[DebuggerDisplay("{CosparId}: {Name} -- {ObjectClass}")]
public record DiscosObject : DiscosModelBase
{
	[JsonPropertyName("cosparId")]
	public string? CosparId { get; init; }
	
	[JsonPropertyName("vimpelId")]
	public string? VimpelId { get; init; }

	[JsonPropertyName("satno")]
	public int? SatNo { get; init; }
	
	[JsonPropertyName("shape")]
	public string? Shape { get; init; }
	
	[JsonPropertyName("mass")]
	public float? Mass { get; init; }
	
	[JsonPropertyName("length")]
	public float? Length { get; init; }
	[JsonPropertyName("height")]
	public float? Height { get; init; }
	[JsonPropertyName("depth")]
	public float? Depth { get; init; }

	[JsonPropertyName("xSectMax")]
	public double CrossSectionMaximum { get; init; }
	[JsonPropertyName("xSectMin")]
	public double CrossSectionMinimum { get; init; }
	[JsonPropertyName("xSectAvg")]
	public double CrossSectionAverage { get; init; }
	
	[JsonPropertyName("objectClass")]
	public ObjectClass? ObjectClass { get; init; }
}
