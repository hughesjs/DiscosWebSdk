using DISCOSweb_Sdk.Mapping.JsonApi.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.Orbits;
using Hypermedia.Configuration;
using Hypermedia.Metadata;

namespace DISCOSweb_Sdk.Mapping.JsonApi;

internal static class DiscosObjectResolver
{
	internal static IContractResolver CreateResolver()
	{
		return new Builder()
			  .WithDiscosObject()
			  .WithDiscosObjectClass()
			  .Build();
	}
	


}
