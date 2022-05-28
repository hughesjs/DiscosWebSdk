using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Mapping.JsonApi;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Hypermedia.JsonApi.Client;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.MapperTests;

public class JsonApiMapperTests
{
	[Fact]
	public async Task CanFetchSputnikIgnoringLinks()
	{
		DiscosObject expectedSputnik = new()
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
		
		foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
		{
			Console.WriteLine($"{environmentVariable.Key}: {environmentVariable.Value}");
		}
		HttpClient client = new();
		client.DefaultRequestHeaders.Authorization = new ("bearer", Environment.GetEnvironmentVariable("DISCOS_API_KEY"));
		HttpResponseMessage res = await client.GetAsync("https://discosweb.esoc.esa.int/api/objects/1");
		res.EnsureSuccessStatusCode();
		DiscosObject discosResult = await res.Content.ReadAsJsonApiAsync<DiscosObject>(DiscosObjectResolver.CreateResolver());
		discosResult = discosResult with {States = null!, Operators = null!, DestinationOrbits = null!, InitialOrbits = null!};
		discosResult.ShouldBeEquivalentTo(expectedSputnik);
	}
}



