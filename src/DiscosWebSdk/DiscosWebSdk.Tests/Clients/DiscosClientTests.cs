using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Extensions;
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
	public async Task CanGetSingleOfEveryTypeWithoutQueryParamsGeneric(Type objectType, string id)
	{
		MethodInfo unconstructedGetSingle = typeof(DiscosClient).GetMethods().Single(m => m.Name == nameof(DiscosClient.GetSingle) && m.IsGenericMethod);
		MethodInfo getSingle              = unconstructedGetSingle.MakeGenericMethod(objectType);
		object? res = await getSingle.InvokeAsync(_discosClient, id, string.Empty);

		res.ShouldNotBeNull();
	}
	
	[Theory]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public async Task CanGetSingleOfEveryTypeWithoutQueryParamsNonGeneric(Type objectType, string id)
	{
		object? res = await _discosClient.GetSingle(objectType, id);
		res.ShouldNotBeNull();
	}

	[Theory]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public async Task CanGetMultipleOfEveryTypeWithoutQueryParams(Type objectType, string _)
	{
		MethodInfo unconstructedGetSingle = typeof(DiscosClient).GetMethod(nameof(DiscosClient.GetMultiple))!;
		MethodInfo getSingle              = unconstructedGetSingle.MakeGenericMethod(objectType);

		object? result;
		if (objectType == typeof(Country)) // No countries on first page of entities...
		{
			result = await getSingle.InvokeAsync(_discosClient, "?filter=contains(name,'United')");
		}
		else 
		{
			result = await getSingle.InvokeAsync(_discosClient, string.Empty);
		}
		result.ShouldNotBeNull();
		((IEnumerable)result).Cast<object>().Count().ShouldBeGreaterThan(1);
	}
}
