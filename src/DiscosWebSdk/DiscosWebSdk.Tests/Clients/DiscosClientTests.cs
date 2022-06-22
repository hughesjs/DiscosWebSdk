using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Extensions;
using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Tests.TestDataGenerators;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Clients;

public class DiscosClientTests
{
	private readonly string       _apiBase = Environment.GetEnvironmentVariable("DISCOS_API_URL") ?? "https://discosweb.esoc.esa.int/api/";
	private readonly IDiscosClient _discosClient;

	public DiscosClientTests()
	{
		HttpClient innerClient = new();
		innerClient.BaseAddress                         = new(_apiBase);
		innerClient.DefaultRequestHeaders.Authorization = new("bearer", Environment.GetEnvironmentVariable("DISCOS_API_KEY"));
		_discosClient                                   = new DiscosClient(innerClient);
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
		DiscosModelBase? res = await _discosClient.GetSingle(objectType, id);
		res.ShouldNotBeNull();
	}

	[Theory]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public async Task CanGetMultipleOfEveryTypeWithoutQueryParamsGeneric(Type objectType, string _)
	{
		MethodInfo unconstructedGetSingle = typeof(DiscosClient).GetMethods().Single(m => m.Name == nameof(DiscosClient.GetMultiple) && m.IsGenericMethod);
		MethodInfo getSingle              = unconstructedGetSingle.MakeGenericMethod(objectType);

		IReadOnlyList<DiscosModelBase?>? result;
		if (objectType == typeof(Country)) // No countries on first page of entities...
		{
			result = (IReadOnlyList<DiscosModelBase?>?)await getSingle.InvokeAsync(_discosClient, "?filter=contains(name,'United')");
		}
		else 
		{
			result = (IReadOnlyList<DiscosModelBase?>?)await getSingle.InvokeAsync(_discosClient, string.Empty);
		}
		result.ShouldNotBeNull();
		result.Count.ShouldBeGreaterThan(1);
		result.ShouldAllBe(r => r.GetType().IsAssignableTo(typeof(DiscosModelBase)));
	}
	
	[Theory]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public async Task CanGetMultipleOfEveryTypeWithoutQueryParamsNonGeneric(Type objectType, string _)
	{
		IReadOnlyList<DiscosModelBase?>? res; 
		if (objectType == typeof(Country)) // No countries on first page of entities...
		{
			res    = await _discosClient.GetMultiple(objectType, "?filter=contains(name,'United')");
		}
		else 
		{
			res = await _discosClient.GetMultiple(objectType);
		}
		res.ShouldNotBeNull();
		res.Count.ShouldBeGreaterThan(1);
		res.ShouldAllBe(r => r.GetType().IsAssignableTo(typeof(DiscosModelBase)));
	}
}
