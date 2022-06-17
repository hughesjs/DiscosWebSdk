using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Mapping.JsonApi.LaunchVehicles;

public class LaunchVehicleStageMapperTests : JsonApiMapperTestBase
{
	private readonly LaunchVehicleStage _stage = new()
												 {
													 Id                  = "328",
													 Name                = "Soyuz-2.1V Blok-A",
													 BurnTime            = 225.0f,
													 SolidPropellantMass = null!,
													 Height              = 27.77f,
													 OxidiserMass        = 88950f,
													 DryMass             = 10200f,
													 WetMass             = 129000f,
													 Span                = null!,
													 Diameter            = 2.95f,
													 FuelMass            = 29840f,
													 Vehicles            = null!
												 };


	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		LaunchVehicleStage stage = await FetchSingle<LaunchVehicleStage>(_stage.Id);
		stage = stage with {Vehicles = null!};
		stage.ShouldBeEquivalentTo(_stage);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		const string       queryString = "?include=vehicles";
		LaunchVehicleStage stage       = await FetchSingle<LaunchVehicleStage>(_stage.Id, queryString);
		stage.Vehicles.Count.ShouldBe(2);
		stage.Vehicles.First().Name.ShouldBe("Soyuz-2-1V Volga");

	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<LaunchVehicleStage> stages = await FetchMultiple<LaunchVehicleStage>();
		stages.Count.ShouldBeGreaterThan(1);
	}
}
