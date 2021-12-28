using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.ResponseModels.Reentries;

public record Reentry: DiscosModelBase
{
	[JsonIgnore]
	public override string? Name => Epoch.ToString("O");
	
	[JsonPropertyName("epoch")]
	public DateTime Epoch { get; init; }
}
