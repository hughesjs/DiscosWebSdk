using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Tests.TestDataGenerators;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Clients;

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
