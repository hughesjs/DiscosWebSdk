using DISCOSweb_Sdk.Mapping.JsonApi.DiscosObjects;
using DISCOSweb_Sdk.Mapping.JsonApi.Entities;
using DISCOSweb_Sdk.Mapping.JsonApi.FragmentationEvent;
using DISCOSweb_Sdk.Mapping.JsonApi.Generic;
using DISCOSweb_Sdk.Mapping.JsonApi.Launches;
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
			  .WithCountry()
			  .WithOrganisation()
			  .WithFragmentationEvent()
			  .WithLaunch()
			  .WithLaunchSite()
			  .WithLaunchSystem()
			  .WithBasicObject<DiscosObjectClass>()
			  .Build();
	}
	


}
