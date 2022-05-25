using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Entities;

internal static class OrganisationContractBuilder
{
	internal static DelegatingContractBuilder<Organisation> WithOrganisation(this IBuilder builder)
	{
		const string idFieldName = nameof(Country.Id);
		const string launchLinkTemplate = $"/api/entities/{idFieldName}/launches";
		const string objectLinkTemplate = $"/api/entities/{idFieldName}/objects";
		const string launchSystemLinkTemplate = $"/api/entities/{idFieldName}/launch-systems";
		const string launchSiteLinkTemplate = $"/api/entities/{idFieldName}/launch-sites";
		const string hostCountryLinkTemplate = $"/api/entities/{idFieldName}/host-country";
		object IdSelector(Organisation r) => r.Id;

		return builder.With<Organisation>("organisation")
					  .Id(nameof(Organisation.Id))
					  .HasMany<Launch>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.Launches)))
					  .Template(launchLinkTemplate, idFieldName, IdSelector)
					  .HasMany<DiscosObject>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.Objects)))
					  .Template(objectLinkTemplate, idFieldName, IdSelector)
					  .HasMany<LaunchSystem>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.LaunchSystems)))
					  .Template(launchSystemLinkTemplate, idFieldName, IdSelector)
					  .HasMany<LaunchSite>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.LaunchSites)))
					  .Template(launchSiteLinkTemplate, idFieldName, IdSelector)
					  .BelongsTo<Country>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.HostCountry)))
					  .Template(hostCountryLinkTemplate, idFieldName, IdSelector);
	}
}
