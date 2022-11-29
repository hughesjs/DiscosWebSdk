using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.FragmentationEvent;
using DiscosWebSdk.Models.ResponseModels.Launches;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using DiscosWebSdk.Models.ResponseModels.Orbits;
using DiscosWebSdk.Models.ResponseModels.Propellants;
using DiscosWebSdk.Models.ResponseModels.Reentries;
using DiscosWebSdk.Queries.Builders;
using DiscosWebSdk.Services.BulkFetching;
using DiscosWebSdk.Tests.Misc;
using DiscosWebSdk.Tests.TestDataGenerators;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Services.BulkFetching;

public class ImmediateBulkFetchServiceTests
{
	private readonly string _apiBase = Environment.GetEnvironmentVariable("DISCOS_API_URL") ?? "https://discosweb.esoc.esa.int/api/";

	private static readonly List<TimeSpan> RetrySpans = new[] {1, 2, 5, 10, 30, 60, 60, 60, 60, 60, 60, 60, 60, 60}.Select(i => TimeSpan.FromSeconds(i)).ToList();
	private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy = HttpPolicyExtensions
																			   .HandleTransientHttpError()
																			   .OrResult(res => res.StatusCode is HttpStatusCode.TooManyRequests)
																			   .WaitAndRetryAsync(RetrySpans,
																								  (result, span, i, _) => { Console.WriteLine($"Will retry in {span.TotalSeconds}s due to {result.Result.ReasonPhrase}. Retry count: {i}"); }
																								 );
	private readonly ImmediateBulkFetchService _service;

	public ImmediateBulkFetchServiceTests()
	{
		HttpClient innerClient = new(new PollyRetryHandler(RetryPolicy, new HttpClientHandler()));

		innerClient.Timeout                             = TimeSpan.FromMinutes(20);
		innerClient.BaseAddress                         = new(_apiBase);
		innerClient.DefaultRequestHeaders.Authorization = new("bearer", Environment.GetEnvironmentVariable("DISCOS_API_KEY"));
		IDiscosClient discosClient = new DiscosClient(innerClient, NullLogger<DiscosClient>.Instance);
		_service = new(discosClient, new DiscosQueryBuilder(), NullLogger<ImmediateBulkFetchService>.Instance);
	}

	// Added because of a bug where the query builder wasn't being reset
	[Fact]
	public async Task CanFetchAllOfSomeSmallerTypesSequentially()
	{
		List<Type> types = new()
						   {typeof(Propellant), typeof(LaunchVehicleFamily), typeof(LaunchSite), typeof(LaunchVehicleStage)};

		foreach (Type t in types)
		{
			List<DiscosModelBase> res = await _service.GetAll(t);
			res.Count.ShouldBeGreaterThan(10);
		}
	}


	[Theory(Skip = "Skipping because this takes forever")]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public async Task CanGetAllOfEverything(Type objectType, string _)
	{
		int pagesFetched = 0;
		_service.DownloadStatusChanged += (_, _) => pagesFetched++;


		List<DiscosModelBase> res = await _service.GetAll(objectType, true);

		// Accurate as of 2022-08-21, realistically, these should only increase
		int pagesLowerBound = Activator.CreateInstance(objectType) switch
							  {
								  DiscosObject            => 600,
								  LaunchSystem            => 1,
								  Launch                  => 63,
								  LaunchSite              => 1,
								  DiscosObjectClass       => 1,
								  LaunchVehicleFamily     => 2,
								  LaunchVehicleStage      => 7,
								  LaunchVehicleEngine     => 9,
								  LaunchVehicle           => 4,
								  Propellant              => 1,
								  Reentry                 => 276,
								  InitialOrbitDetails     => 453,
								  DestinationOrbitDetails => 99,
								  FragmentationEvent      => 7,
								  Country                 => 1,
								  Organisation            => 1,
								  _                       => 1
							  };

		res.Count.ShouldBeGreaterThan(pagesLowerBound);
		pagesFetched.ShouldBe((res.Count - 1)/100 + 1);
	}

}
