using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.Launches;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.Entities;

internal static class OrganisationContractBuilder
{
	internal static DelegatingContractBuilder<Organisation> WithOrganisation(this IBuilder builder)
	{
		const string organisationIdFieldName  = nameof(Organisation.Id);
		const string launchLinkTemplate       = $"/api/entities/{organisationIdFieldName}/launches";
		const string objectLinkTemplate       = $"/api/entities/{organisationIdFieldName}/objects";
		const string launchSystemLinkTemplate = $"/api/entities/{organisationIdFieldName}/launch-systems";
		const string launchSitesLinkTemplate  = $"/api/entities/{organisationIdFieldName}/launch-sites";
		const string hostCountryLinkTemplate  = $"/api/entities/{organisationIdFieldName}/host-country";


		object IdSelector(Organisation r) => r.Id;

		return builder.With<Organisation>("organisation")
					  .Id(nameof(Organisation.Id))
					  .HasMany<Launch>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.Launches)))
					  .Template(launchLinkTemplate, organisationIdFieldName, IdSelector)
					  .HasMany<DiscosObject>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.Objects)))
					  .Template(objectLinkTemplate, organisationIdFieldName, IdSelector)
					  .HasMany<LaunchSystem>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.LaunchSystems)))
					  .Template(launchSystemLinkTemplate, organisationIdFieldName, IdSelector)
					  .HasMany<LaunchSite>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.LaunchSites)))
					  .Template(launchSitesLinkTemplate, organisationIdFieldName, IdSelector);
		//.BelongsTo<Country>(AttributeUtilities.GetJsonPropertyName<Organisation>(nameof(Organisation.HostCountry)))
		//.Template(hostCountryLinkTemplate, organisationIdFieldName, IdSelector);
	}
}
