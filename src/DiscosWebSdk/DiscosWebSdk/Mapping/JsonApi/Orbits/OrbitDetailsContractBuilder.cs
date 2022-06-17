using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Orbits;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.Orbits;

internal static class OrbitDetailsContractBuilder
{
	internal static DelegatingContractBuilder<DestinationOrbitDetails> WithDestinationOrbits(this IBuilder builder) => builder.WithOrbitDetails<DestinationOrbitDetails>("destinationOrbit", "destination-orbits");

	internal static DelegatingContractBuilder<InitialOrbitDetails> WithInitialOrbits(this IBuilder builder) => builder.WithOrbitDetails<InitialOrbitDetails>("initialOrbit", "initial-orbits");

	private static DelegatingContractBuilder<T> WithOrbitDetails<T>(this IBuilder builder, string name, string endpoint) where T : OrbitDetails
	{
		const string orbitIdFieldName   = nameof(OrbitDetails.Id);
		string       objectLinkTemplate = $"/api/{endpoint}/{orbitIdFieldName}/object";
		object IdSelector(OrbitDetails orbit) => orbit.Id;

		return builder.With<T>(name)
					  .Id(nameof(OrbitDetails.Id))
					  .BelongsTo<DiscosObject>(AttributeUtilities.GetJsonPropertyName<OrbitDetails>(nameof(OrbitDetails.Object)))
					  .Template(objectLinkTemplate, orbitIdFieldName, IdSelector)
					  .RenameFieldUsingJsonPropertyName(nameof(OrbitDetails.Eccentricity))
					  .RenameFieldUsingJsonPropertyName(nameof(OrbitDetails.ArgumentOfPeriapsis))
					  .RenameFieldUsingJsonPropertyName(nameof(OrbitDetails.Inclination))
					  .RenameFieldUsingJsonPropertyName(nameof(OrbitDetails.MeanAnomaly))
					  .RenameFieldUsingJsonPropertyName(nameof(OrbitDetails.RightAscensionAscendingNode))
					  .RenameFieldUsingJsonPropertyName(nameof(OrbitDetails.SemiMajorAxis));
	}
}
