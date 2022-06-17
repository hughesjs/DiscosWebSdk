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
		const string countryIdFieldName       = nameof(Country.Id);
		const string launchLinkTemplate       = $"/api/entities/{countryIdFieldName}/launches";
		const string objectLinkTemplate       = $"/api/entities/{countryIdFieldName}/objects";
		const string launchSystemLinkTemplate = $"/api/entities/{countryIdFieldName}/launch-systems";
		const string launchSitesLinkTemplate  = $"/api/entities/{countryIdFieldName}/launch-sites";

		object IdSelector(Country r) => r.Id;

		return builder.With<Country>("country")
					  .Id(nameof(Country.Id))
					  .HasMany<Launch>(AttributeUtilities.GetJsonPropertyName<Country>(nameof(Country.Launches)))
					  .Template(launchLinkTemplate, countryIdFieldName, IdSelector)
					  .HasMany<DiscosObject>(AttributeUtilities.GetJsonPropertyName<Country>(nameof(Country.Objects)))
					  .Template(objectLinkTemplate, countryIdFieldName, IdSelector)
					  .HasMany<LaunchSystem>(AttributeUtilities.GetJsonPropertyName<Country>(nameof(Country.LaunchSystems)))
					  .Template(launchSystemLinkTemplate, countryIdFieldName, IdSelector)
					  .HasMany<LaunchSite>(AttributeUtilities.GetJsonPropertyName<Country>(nameof(Country.LaunchSites)))
					  .Template(launchSitesLinkTemplate, countryIdFieldName, IdSelector);
	}
}
