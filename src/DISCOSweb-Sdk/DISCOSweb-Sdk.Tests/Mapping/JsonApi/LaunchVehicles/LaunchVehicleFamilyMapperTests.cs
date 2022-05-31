using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.LaunchVehicles;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.LaunchVehicles;

public class LaunchVehicleFamilyMapperTests : JsonApiMapperTestBase
{

	private readonly LaunchVehicleFamily _family = new()
												   {
													   Id = "50",
													   Name = "Shuttle",
													   System = null!,
													   Vehicles = null!
												   };

	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		LaunchVehicleFamily family = await FetchSingle<LaunchVehicleFamily>(_family.Id);
		family = family with {System = null!, Vehicles = null!};
		family.ShouldBeEquivalentTo(_family);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		string[] includes = {"system", "vehicles"};
		string queryString = $"?include={string.Join(',', includes)}";
		LaunchVehicleFamily family = await FetchSingle<LaunchVehicleFamily>(_family.Id, queryString);
		family.Vehicles.Count.ShouldBe(6);
		family.Vehicles.First().Name.ShouldBe("Enterprise (OV-101)");
		family.System!.Name.ShouldBe("Space Shuttle");
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<LaunchVehicleFamily> families = await FetchMultiple<LaunchVehicleFamily>();
		families.Count.ShouldBeGreaterThan(1);
	}
}
