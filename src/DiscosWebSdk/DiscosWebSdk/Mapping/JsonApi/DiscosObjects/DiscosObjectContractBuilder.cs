using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.Launches;
using DiscosWebSdk.Models.ResponseModels.Orbits;
using DiscosWebSdk.Models.ResponseModels.Reentries;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.DiscosObjects;

internal static class DiscosObjectContractBuilder
{
	internal static DelegatingContractBuilder<DiscosObject> WithDiscosObject(this IBuilder builder)
	{
		const string objectIdFieldName             = nameof(DiscosObject.Id);
		const string countryLinkTemplate           = $"/api/objects/{objectIdFieldName}/states";
		const string destinationOrbitsLinkTemplate = $"/api/objects/{objectIdFieldName}/relationships/destination-orbits";
		const string initialOrbitsLinkTemplate     = $"/api/objects/{objectIdFieldName}/relationships/initial-orbits";
		const string operatorsLinkTemplate         = $"/api/objects/{objectIdFieldName}/relationships/operators";
		const string launchLinkTemplate            = $"/api/objects/{objectIdFieldName}/relationships/launch";
		const string reentryLinkTemplate           = $"/api/objects/{objectIdFieldName}/relationships/reentry";

		object IdSelector(DiscosObject r) => r.Id;

		return builder.With<DiscosObject>("object")
					  .Id(nameof(DiscosObject.Id))
					  .HasMany<Entity>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.States)))
					  .Template(countryLinkTemplate, objectIdFieldName, IdSelector)
					  .HasMany<DestinationOrbitDetails>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.DestinationOrbits)))
					  .Template(destinationOrbitsLinkTemplate, objectIdFieldName, IdSelector)
					  .HasMany<InitialOrbitDetails>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.InitialOrbits)))
					  .Template(initialOrbitsLinkTemplate, objectIdFieldName, IdSelector)
					  .HasMany<Entity>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.Operators)))
					  .Template(operatorsLinkTemplate, objectIdFieldName, IdSelector)
					  .BelongsTo<Launch>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.Launch)))
					  .Template(launchLinkTemplate, objectIdFieldName, IdSelector)
					  .BelongsTo<Reentry>(AttributeUtilities.GetJsonPropertyName<DiscosObject>(nameof(DiscosObject.Reentry)))
					  .Template(reentryLinkTemplate, objectIdFieldName, IdSelector)
					  .RenameFieldUsingJsonPropertyName(nameof(DiscosObject.CrossSectionMaximum))
					  .RenameFieldUsingJsonPropertyName(nameof(DiscosObject.CrossSectionMinimum))
					  .RenameFieldUsingJsonPropertyName(nameof(DiscosObject.CrossSectionAverage));
	}
}
