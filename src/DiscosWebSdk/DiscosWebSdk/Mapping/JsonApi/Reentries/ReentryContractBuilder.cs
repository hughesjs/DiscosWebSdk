using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Reentries;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.Reentries;

internal static class ReentryContractBuilder
{
	internal static DelegatingContractBuilder<Reentry> WithReentry(this IBuilder builder)
	{
		const string reentryIdFieldName = nameof(Reentry.Id);
		const string objectLinkTemplate = $"/api/reentries/{reentryIdFieldName}/objects";
		object IdSelector(Reentry reentry) => reentry.Id;

		return builder.With<Reentry>("reentry")
					  .Id(nameof(Reentry.Id))
					  .HasMany<DiscosObject>(AttributeUtilities.GetJsonPropertyName<Reentry>(nameof(Reentry.Objects)))
					  .Template(objectLinkTemplate, reentryIdFieldName, IdSelector);
	}
}
