using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;
using DISCOSweb_Sdk.Models.ResponseModels.Propellants;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Propellants;

internal static class PropellantContractBuilder
{
	internal static DelegatingContractBuilder<Propellant> WithPropellant(this IBuilder builder)
	{
		const string propellantIdFieldName = nameof(Propellant.Id);
		const string stageLinkTemplate = $"/api/propellants/{propellantIdFieldName}/stages";
		object IdSelector(Propellant propellant) => propellant.Id;
		
		return builder.With<Propellant>("propellant")
					  .HasMany<LaunchVehicleStage>(AttributeUtilities.GetJsonPropertyName<Propellant>(nameof(Propellant.Id)))
					  .Template(stageLinkTemplate, propellantIdFieldName, IdSelector);
	}
}
