using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Orbits;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Orbits;

internal static class OrbitDetailsContractBuilder
{
	internal static DelegatingContractBuilder<DestinationOrbitDetails> WithDestinationOrbits(this IBuilder builder)
	{
		return builder.WithOrbitDetails<DestinationOrbitDetails>("destinationOrbit", "destination-orbits");
	}

	internal static DelegatingContractBuilder<InitialOrbitDetails> WithInitialOrbits(this IBuilder builder)
	{
		return builder.WithOrbitDetails<InitialOrbitDetails>("initialOrbit", "initial-orbits");
	}

	private static DelegatingContractBuilder<T> WithOrbitDetails<T>(this IBuilder builder, string name, string endpoint) where T: OrbitDetails
	{
		const string orbitIdFieldName = nameof(OrbitDetails.Id);
		string objectLinkTemplate = $"/api/{endpoint}/{orbitIdFieldName}/object";
		object IdSelector(OrbitDetails orbit) => orbit.Id;

		return builder.With<T>(name)
					  .Id(nameof(OrbitDetails.Id))
					  .BelongsTo<DiscosObject>(AttributeUtilities.GetJsonPropertyName<OrbitDetails>(nameof(OrbitDetails.Object)))
					  .Template(objectLinkTemplate, orbitIdFieldName, IdSelector)
					  .Field(nameof(OrbitDetails.Eccentricity)).Deserialization().Rename(AttributeUtilities.GetJsonPropertyName<OrbitDetails>(nameof(OrbitDetails.Eccentricity)))
					  .Field(nameof(OrbitDetails.ArgumentOfPeriapsis)).Deserialization().Rename(AttributeUtilities.GetJsonPropertyName<OrbitDetails>(nameof(OrbitDetails.ArgumentOfPeriapsis)))
					  .Field(nameof(OrbitDetails.Inclination)).Deserialization().Rename(AttributeUtilities.GetJsonPropertyName<OrbitDetails>(nameof(OrbitDetails.Inclination)))
					  .Field(nameof(OrbitDetails.MeanAnomaly)).Deserialization().Rename(AttributeUtilities.GetJsonPropertyName<OrbitDetails>(nameof(OrbitDetails.MeanAnomaly)))
					  .Field(nameof(OrbitDetails.RightAscensionAscendingNode)).Deserialization().Rename(AttributeUtilities.GetJsonPropertyName<OrbitDetails>(nameof(OrbitDetails.RightAscensionAscendingNode)))
					  .Field(nameof(OrbitDetails.SemiMajorAxis)).Deserialization().Rename(AttributeUtilities.GetJsonPropertyName<OrbitDetails>(nameof(OrbitDetails.SemiMajorAxis)));
	}
}
