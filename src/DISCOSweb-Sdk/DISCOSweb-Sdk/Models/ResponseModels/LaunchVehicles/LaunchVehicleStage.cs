using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.Propellants;

namespace DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;

public record LaunchVehicleStage: DiscosModelBase
{
	[JsonPropertyName("solidPropellantMass")]
	public float? SolidPropellantMass { get; init; }
	
	[JsonPropertyName("dryMass")]
	public float? DryMass { get; init; }
	
	[JsonPropertyName("wetMass")]
	public float? WetMass { get; init; }
	
	[JsonPropertyName("diameter")]
	public float? Diameter { get; init; }
	
	[JsonPropertyName("height")]
	public float? Height { get; init; }
	
	[JsonPropertyName("fuelMass")]
	public float? FuelMass { get; init; }
	
	[JsonPropertyName("span")]
	public float? Span { get; init; }
	
	[JsonPropertyName("burnTime")]
	public float? BurnTime { get; init; }
	
	[JsonPropertyName("oxidiserMass")]
	public float? OxidiserMass { get; init; }
	
	[JsonPropertyName("propellant")]
	public Propellant? Propellant { get; init; }
	
	[JsonPropertyName("vehicles")]
	public IReadOnlyCollection<LaunchVehicle> Vehicles { get; init; } = ArraySegment<LaunchVehicle>.Empty;
}
