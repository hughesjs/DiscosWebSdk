using System.Diagnostics;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.ResponseModels;

[DebuggerDisplay("{Name}")]
public abstract record DiscosModelBase
{
	[JsonPropertyName("name")]
	public string? Name { get; init; }
}
