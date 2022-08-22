using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.FragmentationEvent;
using DiscosWebSdk.Models.ResponseModels.Launches;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using DiscosWebSdk.Models.ResponseModels.Orbits;
using DiscosWebSdk.Models.ResponseModels.Propellants;
using DiscosWebSdk.Models.ResponseModels.Reentries;

namespace DiscosWebSdk.Tests.TestDataGenerators;

public class    DiscosModelTypesTestDataGenerator : IEnumerable<object[]>
{
	private readonly Dictionary<Type, string> _ids = new()
													 {
														 {typeof(DiscosObject), "1"},
														 {typeof(DiscosObjectClass), "6a6527d6-efbb-5500-abe3-594ac23d04ed"},
						//								 {typeof(Country), "258"},
														 {typeof(Organisation), "1699"},
														 {typeof(Entity), "1"},
														 {typeof(FragmentationEvent), "86"},
														 {typeof(Launch), "1"},
														 {typeof(LaunchSite), "14"},
														 {typeof(LaunchSystem), "13"},
														 {typeof(LaunchVehicle), "207"},
														 {typeof(LaunchVehicleFamily), "50"},
														 {typeof(LaunchVehicleEngine), "87381"},
														 {typeof(LaunchVehicleStage), "328"},
														 {typeof(InitialOrbitDetails), "1255"},
														 {typeof(DestinationOrbitDetails), "36260"},
														 {typeof(Propellant), "1"},
														 {typeof(Reentry), "29338"}
													 };

	public IEnumerator<object[]> GetEnumerator()
	{
		return typeof(DiscosObject).Assembly
								   .GetTypes()
								   .Where(t => t.IsAssignableTo(typeof(DiscosModelBase)) && t != typeof(DiscosModelBase) && t != typeof(OrbitDetails))
								   .Select(o => new object[] {o, _ids[o]}).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
