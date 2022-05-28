using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Mapping.JsonApi;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Hypermedia.Json;
using Hypermedia.JsonApi.Client;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Client;

public class ClientTests
{
	[Fact]
	public async Task CanFetchSputnik()
	{
		HttpClient client = new();
		client.DefaultRequestHeaders.Authorization = new ("bearer", Environment.GetEnvironmentVariable("discos-api-key"));
		var res = await client.GetAsync("https://discosweb.esoc.esa.int/api/objects/1");
		res.EnsureSuccessStatusCode();
		var content = await res.Content.ReadAsJsonApiAsync<DiscosObject>(DiscosObjectResolver.CreateResolver());
		;
	}
}
