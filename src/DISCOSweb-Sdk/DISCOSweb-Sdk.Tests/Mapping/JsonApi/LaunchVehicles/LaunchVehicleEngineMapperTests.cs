using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.LaunchVehicles;

public class LaunchVehicleEngineMapperTests: JsonApiMapperTestBase
{

	private readonly LaunchVehicleEngine _engine = new()
												   {
													   Id = "87381",
													   Name = "S200 LSB",
													   Diameter = 3.2f,
													   Height = 26.2f,
													   MaxIsp = 274.5f,
													   ThrustLevel = 5151000f,
													   Mass = 31000,
													   Vehicles = null!
												   };

	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		LaunchVehicleEngine engine = await FetchSingle<LaunchVehicleEngine>(_engine.Id);
		engine = engine with {Vehicles = null!};
		engine.ShouldBeEquivalentTo(_engine);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		string queryString = "?include=vehicles";
		LaunchVehicleEngine engine = await FetchSingle<LaunchVehicleEngine>(_engine.Id, queryString);
		engine.Vehicles.Count.ShouldBe(1);
		engine.Vehicles.First().Name.ShouldBe("GSLV Mk III");
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<LaunchVehicleEngine> engines = await FetchMultiple<LaunchVehicleEngine>();
		engines.Count.ShouldBeGreaterThan(1);
	}
}
