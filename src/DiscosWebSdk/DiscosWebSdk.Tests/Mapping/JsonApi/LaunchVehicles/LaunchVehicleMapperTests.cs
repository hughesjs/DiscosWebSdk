using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Mapping.JsonApi.LaunchVehicles;

public class LaunchVehicleMapperTests : JsonApiMapperTestBase
{
	private readonly LaunchVehicle _vehicle = new()
											  {
												  Id                 = "207",
												  Name               = "Falcon 1+",
												  NumStages          = 2,
												  Height             = null,
												  EscCapacity        = null,
												  Diameter           = 1.7f,
												  GtoCapacity        = 430f,
												  SsoCapacity        = 430f,
												  Mass               = 30400f,
												  LeoCapacity        = 670f,
												  GeoCapacity        = null,
												  ThrustLevel        = 556000,
												  FailedLaunches     = 0,
												  SuccessfulLaunches = 2,
												  Stages             = null!,
												  Launches           = null!,
												  Family             = null!,
												  Engines            = null!
											  };


	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		LaunchVehicle vehicle = await FetchSingle<LaunchVehicle>(_vehicle.Id);
		vehicle = vehicle with {Stages = null!, Launches = null!, Family = null!, Engines = null!};
		vehicle.ShouldBeEquivalentTo(_vehicle);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		string[]      includes    = {"stages", "launches", "family", "engines"};
		string        queryString = $"?include={string.Join(',', includes)}";
		LaunchVehicle vehicle     = await FetchSingle<LaunchVehicle>(_vehicle.Id, queryString);
		vehicle.Launches.Count.ShouldBe(2);
		vehicle.Launches.First().CosparLaunchNo.ShouldBe("2008-048");
		vehicle.Stages.Count.ShouldBe(2);
		vehicle.Stages.First().Name.ShouldBe("Falcon 1 Merlin-1C");
		vehicle.Family!.Name.ShouldBe("Falcon");
		vehicle.Engines.Count.ShouldBe(2);
		vehicle.Engines.First().Name.ShouldBe("Merlin-1C");

	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<LaunchVehicle> vehicles = await FetchMultiple<LaunchVehicle>();
		vehicles.Count.ShouldBeGreaterThan(1);
	}
}
