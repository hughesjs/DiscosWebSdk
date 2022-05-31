using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Models.ResponseModels.Orbits;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.Orbits;

public class DestinationOrbitDetailsMapperTests: JsonApiMapperTestBase
{
	private readonly DestinationOrbitDetails _destination = new()
															{
																Id = "36260",
																ArgumentOfPeriapsis = 0f,
																Eccentricity = 0.00227f,
																SemiMajorAxis = 7217800,
																RightAscensionAscendingNode = 0.0f,
																Frame = OrbitalFrame.J2000,
																Epoch = new DateTime(1990,09,04),
																Inclination = 99.0f,
																MeanAnomaly = null!,
																Object = null!
															};

	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		DestinationOrbitDetails dest = await FetchSingle<DestinationOrbitDetails>(_destination.Id);
		dest = dest with {Object = null!};
		dest.ShouldBeEquivalentTo(_destination);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		const string queryString = "?include=object";
		DestinationOrbitDetails dest = await FetchSingle<DestinationOrbitDetails>(_destination.Id, queryString);
		dest.Object!.Name.ShouldBe("PRC 32 (Da Qui Weixing 2)");
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<DestinationOrbitDetails> destOrbits = await FetchMultiple<DestinationOrbitDetails>();
		destOrbits.Count.ShouldBeGreaterThan(1);
	}
}
