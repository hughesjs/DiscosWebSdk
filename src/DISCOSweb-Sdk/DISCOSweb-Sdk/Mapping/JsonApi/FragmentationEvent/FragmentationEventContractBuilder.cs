using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.FragmentationEvent;

internal static class FragmentationEventContractBuilder
{
	internal static DelegatingContractBuilder<Models.ResponseModels.FragmentationEvent.FragmentationEvent> WithFragmentationEvent(this IBuilder builder)
	{
		return builder.With<Models.ResponseModels.FragmentationEvent.FragmentationEvent>("fragmentation")
					  .Id(nameof(Models.ResponseModels.FragmentationEvent.FragmentationEvent.Id))
					  .HasMany<DiscosObject>(AttributeUtilities.GetJsonPropertyName<Models.ResponseModels.FragmentationEvent.FragmentationEvent>(nameof(Models.ResponseModels.FragmentationEvent.FragmentationEvent.Objects)));

	}
}
