using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Launches;

internal static class LaunchSystemContractBuilder
{

	internal static DelegatingContractBuilder<LaunchSystem> WithLaunchSystem(this IBuilder builder)
	{
		const string launchSystemIdFieldName = nameof(LaunchSystem.Id);
		const string entityLinkTemplate      = $"/api/launch-systems/{launchSystemIdFieldName}/entities";
		const string familyLinkTemplate      = $"/api/launch-systems/{launchSystemIdFieldName}/families";
		object IdSelector(LaunchSystem system) => system.Id;

		return builder.With<LaunchSystem>("launchSystem")
					  .Id(nameof(LaunchSystem.Id))
					  .HasMany<Entity>(AttributeUtilities.GetJsonPropertyName<LaunchSystem>(nameof(LaunchSystem.Entities)))
					  .Template(entityLinkTemplate, launchSystemIdFieldName, IdSelector)
					  .HasMany<LaunchVehicleFamily>(AttributeUtilities.GetJsonPropertyName<LaunchSystem>(nameof(LaunchSystem.Families)))
					  .Template(familyLinkTemplate, launchSystemIdFieldName, IdSelector);
	}
}
