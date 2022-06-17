using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;

namespace DiscosWebSdk.Models.ResponseModels.Propellants;

[DebuggerDisplay("{Name}")]
public record Propellant : DiscosModelBase
{
	[JsonIgnore] // This seems to be pretty much the only return type without a name, delegating to fuel-oxidiser to keep polymorphism
	public override string Name => GenerateName();

	[JsonPropertyName("fuel")]
	public string? Fuel { get; init; }

	[JsonPropertyName("oxidiser")]
	public string? Oxidiser { get; init; }

	[JsonPropertyName("solidPropellant")]
	public string? SolidPropellant { get; init; }

	[JsonPropertyName("stages")]
	public IReadOnlyList<LaunchVehicleStage> Stages { get; init; } = ArraySegment<LaunchVehicleStage>.Empty;

	private string GenerateName()
	{
		IEnumerable<string> parts = new[] {Fuel, Oxidiser, SolidPropellant}.Where(p => !string.IsNullOrEmpty(p)).Cast<string>();
		StringBuilder       sb    = new();
		sb.AppendJoin('-', parts);
		return sb.ToString();
	}
}
