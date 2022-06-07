using DISCOSweb_Sdk.Mapping.JsonApi.DiscosObjects;
using DISCOSweb_Sdk.Mapping.JsonApi.Entities;
using DISCOSweb_Sdk.Mapping.JsonApi.FragmentationEvent;
using DISCOSweb_Sdk.Mapping.JsonApi.Generic;
using DISCOSweb_Sdk.Mapping.JsonApi.Launches;
using DISCOSweb_Sdk.Mapping.JsonApi.LaunchVehicles;
using DISCOSweb_Sdk.Mapping.JsonApi.Orbits;
using DISCOSweb_Sdk.Mapping.JsonApi.Propellants;
using DISCOSweb_Sdk.Mapping.JsonApi.Reentries;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Hypermedia.Configuration;
using Hypermedia.Metadata;

namespace DISCOSweb_Sdk.Mapping.JsonApi;

internal static class DiscosObjectResolver
{
	internal static IContractResolver CreateResolver() =>
		new Builder()
		   .WithDiscosObject()
		   .WithCountry()
		   .WithOrganisation()
		   .WithFragmentationEvent()
		   .WithLaunch()
		   .WithLaunchSite()
		   .WithLaunchSystem()
		   .WithLaunchVehicle()
		   .WithLaunchVehicleEngine()
		   .WithLaunchVehicleFamily()
		   .WithLaunchVehicleStage()
		   .WithDestinationOrbits()
		   .WithInitialOrbits()
		   .WithPropellant()
		   .WithReentry()
		   .WithBasicObject<DiscosObjectClass>()
		   .Build();
}
