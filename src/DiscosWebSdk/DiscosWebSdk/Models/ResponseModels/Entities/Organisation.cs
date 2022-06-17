using System.Text.Json.Serialization;

namespace DiscosWebSdk.Models.ResponseModels.Entities;

public record Organisation : Entity
{

	[JsonPropertyName("hostCountry")]
	public Country? HostCountry { get; init; }
}
