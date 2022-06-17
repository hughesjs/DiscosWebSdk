using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using DiscosWebSdk.Models.ResponseModels.Propellants;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.Propellants;

internal static class PropellantContractBuilder
{
	internal static DelegatingContractBuilder<Propellant> WithPropellant(this IBuilder builder)
	{
		const string propellantIdFieldName = nameof(Propellant.Id);
		const string stageLinkTemplate     = $"/api/propellants/{propellantIdFieldName}/stages";
		object IdSelector(Propellant propellant) => propellant.Id;

		return builder.With<Propellant>("propellant")
					  .Id(nameof(Propellant.Id))
					  .HasMany<LaunchVehicleStage>(AttributeUtilities.GetJsonPropertyName<Propellant>(nameof(Propellant.Stages)))
					  .Template(stageLinkTemplate, propellantIdFieldName, IdSelector);
	}
}
