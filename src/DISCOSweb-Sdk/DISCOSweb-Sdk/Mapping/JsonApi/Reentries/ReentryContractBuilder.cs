using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Reentries;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Reentries;

internal static class ReentryContractBuilder
{
	internal static DelegatingContractBuilder<Reentry> WithReentry(this IBuilder builder)
	{
		const string reentryIdFieldName = nameof(Reentry.Id);
		const string objectLinkTemplate = $"/api/reentries/{reentryIdFieldName}/objects";
		object IdSelector(Reentry reentry) => reentry.Id;

		return builder.With<Reentry>("reentry")
					  .HasMany<DiscosObject>(AttributeUtilities.GetJsonPropertyName<Reentry>(nameof(Reentry.Objects)))
					  .Template(objectLinkTemplate, reentryIdFieldName, IdSelector);
	}
}
