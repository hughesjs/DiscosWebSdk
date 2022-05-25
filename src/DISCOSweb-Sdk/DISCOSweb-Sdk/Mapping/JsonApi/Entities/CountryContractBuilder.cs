using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Entities;

internal static class CountryContractBuilder
{
	internal static DelegatingContractBuilder<Country> WithCountry(this IBuilder builder)
	{
		const string idFieldName = nameof(Country.Id);
		const string launchLinkTemplate = $"/api/entities/{idFieldName}/launches";
		const string objectLinkTemplate = $"/api/entities/{idFieldName}/objects";
		const string launchSystemLinkTemplate = $"/api/entities/{idFieldName}/launch-systems";
		const string launchSiteLinkTemplate = $"/api/entities/{idFieldName}/launch-sites";
		object IdSelector(Country r) => r.Id;

		return builder.With<Country>("country")
					  .Id(nameof(Country.Id))
					  .HasMany<Launch>(AttributeUtilities.GetJsonPropertyName<Country>(nameof(Country.Launches)))
					  .Template(launchLinkTemplate, idFieldName, IdSelector)
					  .HasMany<DiscosObject>(AttributeUtilities.GetJsonPropertyName<Country>(nameof(Country.Objects)))
					  .Template(objectLinkTemplate, idFieldName, IdSelector)
					  .HasMany<LaunchSystem>(AttributeUtilities.GetJsonPropertyName<Country>(nameof(Country.LaunchSystems)))
					  .Template(launchSystemLinkTemplate, idFieldName, IdSelector)
					  .HasMany<LaunchSite>(AttributeUtilities.GetJsonPropertyName<Country>(nameof(Country.LaunchSites)))
					  .Template(launchSiteLinkTemplate, idFieldName, IdSelector);
	}
}
