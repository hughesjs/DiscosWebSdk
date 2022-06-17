using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.FragmentationEvent;

internal static class FragmentationEventContractBuilder
{
	internal static DelegatingContractBuilder<Models.ResponseModels.FragmentationEvent.FragmentationEvent> WithFragmentationEvent(this IBuilder builder) =>
		builder.With<Models.ResponseModels.FragmentationEvent.FragmentationEvent>("fragmentation")
			   .Id(nameof(Models.ResponseModels.FragmentationEvent.FragmentationEvent.Id))
			   .HasMany<DiscosObject>(AttributeUtilities.GetJsonPropertyName<Models.ResponseModels.FragmentationEvent.FragmentationEvent>(nameof(Models.ResponseModels.FragmentationEvent.FragmentationEvent.Objects)));
}
