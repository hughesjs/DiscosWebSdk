using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.Orbits;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.DiscosObjects;

internal static class DiscosObjectContractBuilder
{
	internal static DelegatingContractBuilder<DiscosObject> WithDiscosObject(this IBuilder builder)
	{
		const string countryLinkTemplate = "/api/objects/{id}/states";
		const string destinationOrbitsLinkTemplate = "/api/objects/{id}/relationships/destination-orbits";
		const string initialOrbitsLinkTemplate = "/api/objects/{id}/relationships/initial-orbits";
		const string operatorsLinkTemplate = "/api/objects/{id}/relationships/initial-orbits";
		const string idFieldName = nameof(DiscosObject.Id);
		object IdSelector(DiscosObject r) => r.Id;

		return builder.With<DiscosObject>("object")
					  .Id(nameof(DiscosObject.Id))
					  .HasMany<Country>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.States)))
					  .Template(countryLinkTemplate, idFieldName, IdSelector)
					  .HasMany<OrbitDetails>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.DestinationOrbits)))
					  .Template(destinationOrbitsLinkTemplate, idFieldName, IdSelector)
					  .HasMany<OrbitDetails>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.InitialOrbits)))
					  .Template(initialOrbitsLinkTemplate, idFieldName, IdSelector)
					  .HasMany<Organisation>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.Operators)))
					  .Template(operatorsLinkTemplate, idFieldName, IdSelector);
	}
}
