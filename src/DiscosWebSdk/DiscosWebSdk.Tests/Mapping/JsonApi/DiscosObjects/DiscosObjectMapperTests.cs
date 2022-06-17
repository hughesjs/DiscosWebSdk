using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscosWebSdk.Enums;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Mapping.JsonApi.DiscosObjects;

public class DiscosObjectMapperTests : JsonApiMapperTestBase
{
	private readonly DiscosObject _object = new()
											{
												Id                  = "67013",
												Name                = "Starlink 2534",
												CosparId            = "2021-036AM",
												Shape               = "Box + 1 Pan",
												CrossSectionAverage = 13.5615,
												CrossSectionMaximum = 23.657,
												CrossSectionMinimum = 0.2311,
												Mass                = 260f,
												Length              = 3.7f,
												Depth               = 8.86f,
												Height              = 0.1f,
												SatNo               = 48311,
												ObjectClass         = ObjectClass.Payload,
												VimpelId            = null,
												States              = null!,
												DestinationOrbits   = null!,
												InitialOrbits       = null!,
												Launch              = null!,
												Operators           = null!,
												Reentry             = null!
											};

	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		DiscosObject discosResult = await FetchSingle<DiscosObject>(_object.Id);
		discosResult = discosResult with {States = null!, DestinationOrbits = null!, InitialOrbits = null!, Launch = null!, Operators = null!, Reentry = null!};
		discosResult.ShouldBeEquivalentTo(_object);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		string[]     includes     = {"states", "destinationOrbits", "initialOrbits", "launch", "operators", "reentry"};
		string       queryString  = $"?include={string.Join(',', includes)}";
		DiscosObject discosResult = await FetchSingle<DiscosObject>(_object.Id, queryString);
		discosResult.States.First().Name.ShouldBe("United States");
		discosResult.Operators.First().Name.ShouldBe("SpaceX Seattle");
		discosResult.InitialOrbits.First().Epoch.ShouldBe(new DateTime(2021,     04, 29));
		discosResult.DestinationOrbits.First().Epoch.ShouldBe(new DateTime(2021, 05, 09));
		discosResult.Launch.CosparLaunchNo.ShouldBe("2021-036");
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<DiscosObject> discosResult = await FetchMultiple<DiscosObject>();
		discosResult.Count.ShouldBeGreaterThan(1);
	}
}
