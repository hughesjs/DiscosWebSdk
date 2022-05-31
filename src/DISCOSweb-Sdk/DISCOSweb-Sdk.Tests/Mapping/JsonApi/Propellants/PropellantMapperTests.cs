using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.Propellants;
using Shouldly;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.Propellants;

public class PropellantMapperTests: JsonApiMapperTestBase
{

	private readonly Propellant _prop = new()
										{
											Id = "1",
											Oxidiser = "LOX",
											Fuel = "Kerosene",
											SolidPropellant = null,
											Stages = null!
										};
	
	public override async Task CanGetSingleWithoutLinks()
	{
		Propellant rocketFuel = await FetchSingle<Propellant>(_prop.Id);
		rocketFuel = rocketFuel with {Stages = null!};
		rocketFuel.ShouldBeEquivalentTo(_prop);
	}

	public override async Task CanGetSingleWithLinks()
	{
		const string queryString = "?include=stages";
		Propellant rocketFuel = await FetchSingle<Propellant>(_prop.Id, queryString);
		rocketFuel.Stages.Count.ShouldBeGreaterThan(107); // 108 as of 2022-05-31
		rocketFuel.Stages.First().Name.ShouldBe("CZ-NGLV-300");
	}

	public override async Task CanGetMultiple()
	{
		IReadOnlyList<Propellant> props = await FetchMultiple<Propellant>();
		props.Count.ShouldBeGreaterThan(1);
	}
}
