using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Mapping.JsonApi.DiscosObjects;

public class DiscosObjectClassTests : JsonApiMapperTestBase
{
	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		DiscosObjectClass oc = await FetchSingle<DiscosObjectClass>("6a6527d6-efbb-5500-abe3-594ac23d04ed");
		oc.Name.ShouldBe("Unknown");
	}

	public override async Task CanGetSingleWithLinks() => await CanGetSingleWithoutLinks(); // No Links

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<DiscosObjectClass> ocs = await FetchMultiple<DiscosObjectClass>();
		ocs.Count.ShouldBeGreaterThan(1);

		ocs.SingleOrDefault(ocs => ocs.Name == "Unknown").ShouldNotBeNull();
		ocs.SingleOrDefault(ocs => ocs.Name == "Payload").ShouldNotBeNull();
	}
}
