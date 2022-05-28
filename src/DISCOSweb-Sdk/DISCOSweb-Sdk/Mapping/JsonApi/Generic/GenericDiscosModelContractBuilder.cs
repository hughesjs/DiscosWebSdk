using DISCOSweb_Sdk.Models.ResponseModels;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using Hypermedia.Configuration;

namespace DISCOSweb_Sdk.Mapping.JsonApi.Generic;

internal static class GenericDiscosModelContractBuilder
{
	internal static DelegatingContractBuilder<T> WithBasicObject<T>(this IBuilder builder) where T: DiscosModelBase
	{
		return builder.With<T>("objectClass")
					  .Id(nameof(DiscosModelBase.Id));
	}
}
