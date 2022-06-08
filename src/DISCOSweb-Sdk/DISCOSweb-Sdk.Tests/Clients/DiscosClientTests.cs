using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Clients;
using DISCOSweb_Sdk.Models.ResponseModels;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.FragmentationEvent;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;
using DISCOSweb_Sdk.Models.ResponseModels.Orbits;
using DISCOSweb_Sdk.Models.ResponseModels.Propellants;
using DISCOSweb_Sdk.Models.ResponseModels.Reentries;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Clients;

public class DiscosClientTests
{
	private readonly string       _apiBase = Environment.GetEnvironmentVariable("DISCOS_API_URL") ?? "https://discosweb.esoc.esa.int/api/";
	private readonly DiscosClient _discosClient;

	public DiscosClientTests()
	{
		HttpClient innerClient = new();
		innerClient.BaseAddress                         = new(_apiBase);
		innerClient.DefaultRequestHeaders.Authorization = new("bearer", Environment.GetEnvironmentVariable("DISCOS_API_KEY"));
		_discosClient                                   = new(innerClient);
	}

	[Theory]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public void CanGetSingleOfEveryTypeWithoutQueryParams(Type objectType, string id)
	{
		MethodInfo unconstructedGetSingle = typeof(DiscosClient).GetMethod(nameof(DiscosClient.GetSingle))!;
		MethodInfo getSingle              = unconstructedGetSingle.MakeGenericMethod(objectType);

		Task          resTask     = (Task)getSingle.Invoke(_discosClient, new[] {id, string.Empty});
		PropertyInfo? resProperty = resTask.GetType().GetProperty("Result");

		resProperty.GetValue(resTask).ShouldNotBeNull();
	}

	[Theory]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public void CanGetMultipleOfEveryTypeWithoutQueryParams(Type objectType, string _)
	{
		MethodInfo unconstructedGetSingle = typeof(DiscosClient).GetMethod(nameof(DiscosClient.GetMultiple))!;
		MethodInfo getSingle              = unconstructedGetSingle.MakeGenericMethod(objectType);

		Task resTask;
		if (objectType == typeof(Country)) // No countries on first page of entities...
		{
			resTask = (Task)getSingle.Invoke(_discosClient, new[] {"?filter=contains(name,'United')"});
		}
		else {
			resTask = (Task)getSingle.Invoke(_discosClient, new[] {string.Empty});
		}
		
		PropertyInfo? resProperty = resTask!.GetType().GetProperty("Result");

		((IEnumerable)resProperty!.GetValue(resTask)!).Cast<object>().Count().ShouldBeGreaterThan(1);
	}
}


public class DiscosModelTypesTestDataGenerator : IEnumerable<object[]>
{
	private readonly Dictionary<Type, string> _ids = new()
													 {
														 {typeof(DiscosObject), "1"},
														 {typeof(DiscosObjectClass), "6a6527d6-efbb-5500-abe3-594ac23d04ed"},
														 {typeof(Country), "258"},
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
