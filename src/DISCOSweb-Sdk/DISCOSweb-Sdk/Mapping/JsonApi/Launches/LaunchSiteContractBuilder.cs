using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Launches;

internal static class LaunchSiteContractBuilder
{
	internal static DelegatingContractBuilder<LaunchSite> WithLaunchSite(this IBuilder builder)
	{
		const string launchSiteIdFieldName = nameof(LaunchSite.Id);
		const string launchSiteOperatorsLinkTemplate = $"/api/launch-sites/{launchSiteIdFieldName}/operators";
		const string launchesLinkTemplate = $"/api/launch-sites/{launchSiteIdFieldName}/launches";
		object IdSelector(LaunchSite ls) => ls.Id;

		return builder.With<LaunchSite>("launchSite")
					  .HasMany<Entity>(AttributeUtilities.GetJsonPropertyName<LaunchSite>(nameof(LaunchSite.Operators)))
					  .Template(launchSiteOperatorsLinkTemplate, launchSiteIdFieldName, IdSelector)
					  .HasMany<Launch>(AttributeUtilities.GetJsonPropertyName<LaunchSite>(nameof(LaunchSite.Launches)))
					  .Template(launchesLinkTemplate, launchSiteIdFieldName, IdSelector);
		// .Field(nameof(LaunchSite.Latitude)).Deserialization().Rename(nameof(LaunchSite.LatitudeAsString))
		// .Field(nameof(LaunchSite.Longitude)).Deserialization().Rename(nameof(LaunchSite.LongitudeAsString));
	}
}
