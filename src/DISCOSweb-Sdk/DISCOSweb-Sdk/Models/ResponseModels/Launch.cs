using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Models.ResponseModels;


public record Launch: DiscosModelBase
{
	[JsonPropertyName("flightNo")]
	public string? FlightNo { get; init; }
	[JsonPropertyName("epoch")]
	public DateTime? Epoch { get; init; }
	[JsonPropertyName("cosparLaunchNo")]
	public string? CosparLaunchNo { get; init; }
	[JsonPropertyName("failure")]
	public bool Failure { get; init; }
}
