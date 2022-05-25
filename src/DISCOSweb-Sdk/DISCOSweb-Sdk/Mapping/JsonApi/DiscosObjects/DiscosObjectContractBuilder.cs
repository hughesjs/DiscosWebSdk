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
		const string objectIdFieldName = nameof(DiscosObject.Id);
		const string countryLinkTemplate = $"/api/objects/{objectIdFieldName}/states";
		const string destinationOrbitsLinkTemplate = $"/api/objects/{objectIdFieldName}/relationships/destination-orbits";
		const string initialOrbitsLinkTemplate = $"/api/objects/{objectIdFieldName}/relationships/initial-orbits";
		const string operatorsLinkTemplate = $"/api/objects/{objectIdFieldName}/relationships/initial-orbits";

		object IdSelector(DiscosObject r) => r.Id;


		return builder.With<DiscosObject>("object")
					  .Id(nameof(DiscosObject.Id))
					  .HasMany<Country>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.States)))
					  .Template(countryLinkTemplate, objectIdFieldName, IdSelector)
					  .HasMany<OrbitDetails>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.DestinationOrbits)))
					  .Template(destinationOrbitsLinkTemplate, objectIdFieldName, IdSelector)
					  .HasMany<OrbitDetails>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.InitialOrbits)))
					  .Template(initialOrbitsLinkTemplate, objectIdFieldName, IdSelector)
					  .HasMany<Organisation>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.Operators)))
					  .Template(operatorsLinkTemplate, objectIdFieldName, IdSelector);
	}
}
