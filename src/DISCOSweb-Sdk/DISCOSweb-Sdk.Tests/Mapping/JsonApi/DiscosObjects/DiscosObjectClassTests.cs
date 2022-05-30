using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.DiscosObjects;

public class DiscosObjectClassTests: JsonApiMapperTestBase
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
		ocs.First().Name.ShouldBe("Unknown");
		ocs.Last().Name.ShouldBe("Other Debris");
	}
	
}
