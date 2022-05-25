using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Launches;

internal static class LaunchContractBuilder
{
	internal static DelegatingContractBuilder<Launch> WithLaunch(this IBuilder builder)
	{
		const string launchIdFieldName = nameof(Launch.Id);
		const string entityLinkTemplate = $"/api/launches/{launchIdFieldName}/entities";
		const string launchSiteLinkTemplate = $"/api/launches/{launchIdFieldName}/site";
		const string objectLinkTemplate = $"/api/launches/{launchIdFieldName}/objects";
		const string launchVehicleLinkTemplate = $"/api/launches/{launchIdFieldName}/vehicle";
		object IdSelector(Launch launch) => launch.Id;
		
		return builder.With<Launch>("launch")
					  .Id(nameof(Launch.Id))
					  .HasMany<DiscosObject>(AttributeUtilities.GetJsonPropertyName<Launch>(nameof(Launch.Objects)))
					  .Template(objectLinkTemplate, launchIdFieldName, IdSelector)
					  .HasMany<Entity>(AttributeUtilities.GetJsonPropertyName<Launch>(nameof(Launch.Entities)))
					  .Template(entityLinkTemplate, launchIdFieldName, IdSelector)
					  .BelongsTo<LaunchVehicle>(AttributeUtilities.GetJsonPropertyName<Launch>(nameof(Launch.Vehicle)))
					  .Template(launchVehicleLinkTemplate, launchIdFieldName, IdSelector)
					  .BelongsTo<LaunchSite>(AttributeUtilities.GetJsonPropertyName<Launch>(nameof(Launch.Site)))
					  .Template(launchSiteLinkTemplate, launchIdFieldName, IdSelector);
	}
}
