using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.Reentries;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.Reentries;

public class ReentryMapperTests: JsonApiMapperTestBase
{
	private readonly Reentry _reentry = new()
										{
											Id = "29338",
											Epoch = new(2022,03,21),
											Objects = null!
										};
	
	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		Reentry reentry = await FetchSingle<Reentry>(_reentry.Id);
		reentry = reentry with {Objects = null!};
		reentry.ShouldBeEquivalentTo(_reentry);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		const string queryString = "?include=objects";
		Reentry reentry = await FetchSingle<Reentry>(_reentry.Id, queryString);
		reentry.Objects.Count.ShouldBe(1);
		reentry.Objects.First().Name.ShouldBe("Vostok 8A92M-2 (Vostok SL-3 (A-1))");

	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<Reentry> reentries = await FetchMultiple<Reentry>();
		reentries.Count.ShouldBeGreaterThan(1);
	}
}
