using System.Diagnostics;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.SubObjects;

[DebuggerDisplay("{Display}")]
public record Azimuth
{
	[JsonPropertyName("empty")]
	public bool Empty { get; init; }

	[JsonPropertyName("upperInc")]
	public bool UpperInc { get; init; }

	[JsonPropertyName("lowerInc")]
	public bool LowerInc { get; init; }

	[JsonPropertyName("upper")]
	public float? Upper { get; init; }

	[JsonPropertyName("lower")]
	public float? Lower { get; init; }

	[JsonPropertyName("display")]
	public string? Display { get; init; }
}
