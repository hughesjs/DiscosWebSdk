using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Orbits;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Orbits;

internal static class OrbitDetailsContractBuilder
{
	internal static DelegatingContractBuilder<OrbitDetails> WithOrbitDetails(this IBuilder builder)
	{
		return builder.WithOrbitDetails("initialOrbit")
					  .WithOrbitDetails("destinationOrbit");
	}

	private static DelegatingContractBuilder<OrbitDetails> WithOrbitDetails(this IBuilder builder, string name)
	{
		const string orbitIdFieldName = nameof(OrbitDetails.Id);
		const string objectLinkTemplate = $"/api/initial-orbits/{orbitIdFieldName}/object";
		object IdSelector(OrbitDetails orbit) => orbit.Id;

		return builder.With<OrbitDetails>(name)
					  .Id(nameof(OrbitDetails.Id))
					  .BelongsTo<DiscosObject>(AttributeUtilities.GetJsonPropertyName<OrbitDetails>(nameof(OrbitDetails.Object)))
					  .Template(objectLinkTemplate, orbitIdFieldName, IdSelector);
	}
}
