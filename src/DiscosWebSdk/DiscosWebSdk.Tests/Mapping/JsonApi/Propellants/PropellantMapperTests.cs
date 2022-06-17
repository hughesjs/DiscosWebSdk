using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscosWebSdk.Models.ResponseModels.Propellants;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Mapping.JsonApi.Propellants;

public class PropellantMapperTests : JsonApiMapperTestBase
{

	private readonly Propellant _prop = new()
										{
											Id              = "1",
											Oxidiser        = "LOX",
											Fuel            = "Kerosene",
											SolidPropellant = null,
											Stages          = null!
										};

	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		Propellant rocketFuel = await FetchSingle<Propellant>(_prop.Id);
		rocketFuel = rocketFuel with {Stages = null!};
		rocketFuel.ShouldBeEquivalentTo(_prop);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		const string queryString = "?include=stages";
		Propellant   rocketFuel  = await FetchSingle<Propellant>(_prop.Id, queryString);
		rocketFuel.Stages.Count.ShouldBeGreaterThan(107); // 108 as of 2022-05-31
		rocketFuel.Stages.First().Name.ShouldBe("CZ-NGLV-300");
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<Propellant> props = await FetchMultiple<Propellant>();
		props.Count.ShouldBeGreaterThan(1);
	}
}
