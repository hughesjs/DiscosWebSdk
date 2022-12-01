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
												Id                  = "40367",
												Name                = "Dragon CRS-5",
												CosparId            = "2015-001A",
												Shape               = "Trunc Cone + Cyl + 2 Pan",
												CrossSectionAverage = 33.4694417460128,
												CrossSectionMaximum = 60.2525145841952,
												CrossSectionMinimum = 10.7521008569111,
												Mass                = 9688f,
												Length              = 3.7f,
												Depth               = 16.5f,
												Height              = 5.3f,
												SatNo               = 40370,
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
		discosResult.Operators.First().Name.ShouldBe("SpaceX");
		discosResult.InitialOrbits.First().Epoch.ShouldBe(new(2015,     01, 10));
		discosResult.DestinationOrbits.ShouldBeEmpty();
		discosResult.Launch!.CosparLaunchNo.ShouldBe("2015-001");
		discosResult.Reentry!.Epoch.ShouldBe(new(2015, 02, 11));
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<DiscosObject> discosResult = await FetchMultiple<DiscosObject>();
		discosResult.Count.ShouldBeGreaterThan(1);
	}
}
