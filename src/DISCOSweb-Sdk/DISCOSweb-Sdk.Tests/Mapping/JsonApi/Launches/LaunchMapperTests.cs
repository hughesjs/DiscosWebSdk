using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.Launches;

public class LaunchMapperTests: JsonApiMapperTestBase
{
	private readonly Launch _launch = new()
									  {
										  Id = "1",
										  CosparLaunchNo = "1961-001",
										  Epoch = new DateTime(1961, 01, 31, 20, 21, 19),
										  FlightNo = "70D",
										  Name = string.Empty,
										  Failure = false,
										  Objects = null!,
										  Vehicle = null!,
										  Entities = null!,
									  };
	
	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		Launch launch = await FetchSingle<Launch>(_launch.Id);
		launch = launch with {Objects = null!, Vehicle = null!, Entities = null!};
		launch.ShouldBeEquivalentTo(_launch);
	}
	
	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		string[] includes = {"objects", "vehicle", "entities", "site"};
		string queryString = $"?include={string.Join(',', includes)}";
		Launch launch = await FetchSingle<Launch>(_launch.Id, queryString);
		launch.Objects.Count.ShouldBe(2);
		launch.Entities.Count.ShouldBe(1);
		launch.Entities.First().Name.ShouldBe("United States");
		launch.Vehicle.Name.ShouldBe("Atlas LV-3A Agena A");
		launch.Site.Name.ShouldBe("Vandenberg AFB (WTR)");
		
		
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<Launch> launches = await FetchMultiple<Launch>();
		launches.Count.ShouldBeGreaterThan(1);
	}
}
