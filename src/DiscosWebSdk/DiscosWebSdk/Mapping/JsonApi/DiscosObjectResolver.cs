using DiscosWebSdk.Mapping.JsonApi.DiscosObjects;
using DiscosWebSdk.Mapping.JsonApi.Entities;
using DiscosWebSdk.Mapping.JsonApi.FragmentationEvent;
using DiscosWebSdk.Mapping.JsonApi.Generic;
using DiscosWebSdk.Mapping.JsonApi.Launches;
using DiscosWebSdk.Mapping.JsonApi.LaunchVehicles;
using DiscosWebSdk.Mapping.JsonApi.Orbits;
using DiscosWebSdk.Mapping.JsonApi.Propellants;
using DiscosWebSdk.Mapping.JsonApi.Reentries;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using Hypermedia.Configuration;
using Hypermedia.Metadata;

namespace DiscosWebSdk.Mapping.JsonApi;

internal static class DiscosObjectResolver
{
	internal static IContractResolver CreateResolver() =>
		new Builder()
		   .WithDiscosObject()
		   //.WithCountry()
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
