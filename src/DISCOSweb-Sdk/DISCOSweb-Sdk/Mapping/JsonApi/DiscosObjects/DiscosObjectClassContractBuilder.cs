using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.DiscosObjects;

internal static class DiscosObjectClassContractBuilder
{
	internal static DelegatingContractBuilder<DiscosObjectClass> WithDiscosObjectClass(this IBuilder builder)
	{
		return builder.With<DiscosObjectClass>()
					  .Id(nameof(DiscosObjectClass.Id));
	}
}
