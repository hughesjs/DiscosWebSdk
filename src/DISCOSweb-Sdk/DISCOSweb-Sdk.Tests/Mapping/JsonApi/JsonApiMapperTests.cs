using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Mapping.JsonApi;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Hypermedia.JsonApi.Client;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi;

public class JsonApiMapperTests
{
	private readonly DiscosObject _expectedSputnik = new()
													 {
														 Id = "1",
														 SatNo = 1,
														 Name = "Sputnik (8K71PS) Blok-A",
														 CosparId = "1957-001A",
														 Shape = "Cyl",
														 CrossSectionAverage = 59.8316320876176,
														 CrossSectionMaximum = 72.9933461154505,
														 CrossSectionMinimum = 5.30929158456675,
														 Mass = 3964.32f,
														 Depth = 28.0f,
														 Length = 2.6f,
														 Height = 28.0f,
														 ObjectClass = ObjectClass.RocketBody,
														 VimpelId = null,
														 States = null!,
														 InitialOrbits = null!,
														 DestinationOrbits = null!,
														 Operators = null!
													 };

	[Fact]
	public async Task CanFetchSputnikIgnoringLinks()
	{
		DiscosObject discosResult = await FetchObject("1");
		discosResult.ShouldBeEquivalentTo(_expectedSputnik);
	}

	[Fact]
	public async Task CanFetchSputnikIncludingLinks()
	{
		string[] includes = {"states", "destinationOrbits", "initialOrbits", "launch", "operators", "reentry"};
		string queryString = $"?include={includes.Aggregate((a, e) => $"{a},{e}")}";
		DiscosObject discosResult = await FetchObject("1", queryString);
		discosResult.States.First().Name.ShouldBe("USSR, Union of Soviet Socialist Republics");
		discosResult.Operators.First().Name.ShouldBe("Strategic Rocket Forces");
		discosResult.InitialOrbits.First().Epoch.ShouldBe(new DateTime(1957,11,04));
	}

	[Fact]
	public async Task CanFetchManyObjects()
	{
		IList<DiscosObject> discosResult = await FetchObjects();
		discosResult.Count.ShouldBeGreaterThan(1);
	}

	private async Task<DiscosObject> FetchObject(string id = "", string queryString = "")
	{
		HttpClient client = new();
		client.DefaultRequestHeaders.Authorization = new("bearer", Environment.GetEnvironmentVariable("DISCOS_API_KEY"));
		HttpResponseMessage res = await client.GetAsync($"https://discosweb.esoc.esa.int/api/objects/{id}{queryString}");
		res.EnsureSuccessStatusCode();
		return await res.Content.ReadAsJsonApiAsync<DiscosObject>(DiscosObjectResolver.CreateResolver());
	}

	private async Task<IList<DiscosObject>> FetchObjects(string queryString = "")
	{
		HttpClient client = new();
		client.DefaultRequestHeaders.Authorization = new("bearer", Environment.GetEnvironmentVariable("DISCOS_API_KEY"));
		HttpResponseMessage res = await client.GetAsync($"https://discosweb.esoc.esa.int/api/objects{queryString}");
		res.EnsureSuccessStatusCode();
		return await res.Content.ReadAsJsonApiManyAsync<DiscosObject>(DiscosObjectResolver.CreateResolver());
	}

}
