using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscosWebSdk.Models.ResponseModels.Launches;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Mapping.JsonApi.Launches;

public class LaunchSystemMapperTests : JsonApiMapperTestBase
{
	private readonly LaunchSystem _launchSystem = new()
												  {
													  Id       = "13",
													  Name     = "Falcon",
													  Entities = null!,
													  Families = null!
												  };

	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		LaunchSystem system = await FetchSingle<LaunchSystem>(_launchSystem.Id);
		system = system with {Entities = null!, Families = null!};
		system.ShouldBeEquivalentTo(_launchSystem);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		string[]     includes    = {"entities", "families"};
		string       queryString = $"?include={string.Join(',', includes)}";
		LaunchSystem system      = await FetchSingle<LaunchSystem>(_launchSystem.Id, queryString);
		system.Entities.Count.ShouldBe(1);
		system.Entities.First().Name.ShouldBe("United States");
		system.Families.Count.ShouldBe(1);
		system.Families.First().Name.ShouldBe("Falcon");
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<LaunchSystem> systems = await FetchMultiple<LaunchSystem>();
		systems.Count.ShouldBeGreaterThan(1);
	}
}
