using System.Diagnostics;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.ResponseModels;

[DebuggerDisplay("{Name}")]
public abstract record DiscosModelBase
{
	[JsonPropertyName("name")]
	public virtual string Name { get; init; } = string.Empty;

	[JsonPropertyName("id")]
	public string Id { get; init; } = string.Empty;
}
