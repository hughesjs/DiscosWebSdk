using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.Launches;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.Launches;

internal static class LaunchSiteContractBuilder
{
	internal static DelegatingContractBuilder<LaunchSite> WithLaunchSite(this IBuilder builder)
	{
		const string launchSiteIdFieldName           = nameof(LaunchSite.Id);
		const string launchSiteOperatorsLinkTemplate = $"/api/launch-sites/{launchSiteIdFieldName}/operators";
		const string launchesLinkTemplate            = $"/api/launch-sites/{launchSiteIdFieldName}/launches";
		object IdSelector(LaunchSite ls) => ls.Id;

		return builder.With<LaunchSite>("launchSite")
					  .Id(nameof(LaunchSite.Id))
					  .HasMany<Entity>(AttributeUtilities.GetJsonPropertyName<LaunchSite>(nameof(LaunchSite.Operators)))
					  .Template(launchSiteOperatorsLinkTemplate, launchSiteIdFieldName, IdSelector)
					  .HasMany<Launch>(AttributeUtilities.GetJsonPropertyName<LaunchSite>(nameof(LaunchSite.Launches)))
					  .Template(launchesLinkTemplate, launchSiteIdFieldName, IdSelector);
	}
}
