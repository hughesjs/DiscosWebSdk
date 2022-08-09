using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Queries.Builders;
using DiscosWebSdk.Services.BulkFetching;
using DiscosWebSdk.Tests.Misc;
using DiscosWebSdk.Tests.TestDataGenerators;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Services.BulkFetching;


public class ImmediateBulkFetchServiceTests
{
	private readonly string        _apiBase = Environment.GetEnvironmentVariable("DISCOS_API_URL") ?? "https://discosweb.esoc.esa.int/api/";
	private readonly IDiscosClient _discosClient;

	private static readonly List<TimeSpan>                        RetrySpans  = new[] {1, 2, 5, 10, 30, 60, 60, 60, 60, 60, 60, 60, 60, 60}.Select(i => TimeSpan.FromSeconds(i)).ToList();
	private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy = HttpPolicyExtensions.HandleTransientHttpError().OrResult(res => res.StatusCode is HttpStatusCode.TooManyRequests).WaitAndRetryAsync(RetrySpans);

	public ImmediateBulkFetchServiceTests()
	{
		HttpClient innerClient = new(new PollyRetryHandler(RetryPolicy, new HttpClientHandler()));

		innerClient.Timeout                             = TimeSpan.FromMinutes(20);
		innerClient.BaseAddress                         = new(_apiBase);
		innerClient.DefaultRequestHeaders.Authorization = new("bearer", Environment.GetEnvironmentVariable("DISCOS_API_KEY"));
		_discosClient                                   = new DiscosClient(innerClient);
	}

	
	
	[Theory(Skip = "Takes forever to run on the real API due to rate limits.")]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public async Task CanGetAllOfEverything(Type objectType, string _)
	{
		int                       pagesFetched = 0;
		ImmediateBulkFetchService service      = new(_discosClient, new DiscosQueryBuilder());
		service.DownloadStatusChanged += (_, _) => pagesFetched++;
		

		List<DiscosModelBase> res = await service.GetAll(objectType);

		// TODO - Fill this out
		int pagesLowerBound = Activator.CreateInstance(objectType) switch
							  {
								  DiscosObject => 600,
								  _            => 1
							  };

		res.Count.ShouldBeGreaterThan(pagesLowerBound);
		pagesFetched.ShouldBe((res.Count - 1)/ 100 + 1);
	}
	
}
