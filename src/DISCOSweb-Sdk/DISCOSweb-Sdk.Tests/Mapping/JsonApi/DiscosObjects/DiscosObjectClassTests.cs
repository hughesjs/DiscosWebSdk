using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.DiscosObjects;

public class DiscosObjectClassTests: JsonApiMapperTestBase
{
	[Theory]
	[InlineData("6a6527d6-efbb-5500-abe3-594ac23d04ed", "Unknown")]
	[InlineData("f01daa15-6f3b-5f8e-b807-193129868488", "Rocket Debris")]
	[InlineData("42a120cd-9d3e-5226-82d2-4731b9f46ccb", "Payload Debris")]
	[InlineData("29991388-1bf3-50ce-a424-ff2cc2fca5af", "Payload Fragmentation Debris")]
	[InlineData("8c489317-9b02-5e18-9a9a-8bc50d05a1b6", "Payload")]
	public async Task CanFetchIndividualObjectClasses(string id, string expectedName)
	{
		DiscosObjectClass oc = await FetchSingle<DiscosObjectClass>(id);
		oc.Name.ShouldBe(expectedName);
	}

	[Fact]
	public async Task CanFetchMultipleObjectClasses()
	{
		IReadOnlyList<DiscosObjectClass> ocs = await FetchMultiple<DiscosObjectClass>();
		ocs.Count.ShouldBeGreaterThan(1);
		ocs.First().Name.ShouldBe("Unknown");
		ocs.Last().Name.ShouldBe("Other Debris");
	}
	
}
