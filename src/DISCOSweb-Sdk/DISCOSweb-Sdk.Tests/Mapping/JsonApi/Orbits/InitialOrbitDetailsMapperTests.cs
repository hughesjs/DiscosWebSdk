using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Models.ResponseModels.Orbits;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.Orbits;

public class InitialOrbitDetailsMapperTests: JsonApiMapperTestBase
{
	private readonly InitialOrbitDetails _initialOrbit = new()
														 {
															 Id = "1255",
															 Epoch = new DateTime(1965,10,29),
															 MeanAnomaly = null,
															 Inclination = 64.97f,
															 Frame = OrbitalFrame.J2000,
															 RightAscensionAscendingNode = null,
															 SemiMajorAxis = 6607000,
															 Eccentricity = 0.004f,
															 ArgumentOfPeriapsis = 41.0f,
															 Object = null!
														 };

	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		InitialOrbitDetails initialOrbit = await FetchSingle<InitialOrbitDetails>(_initialOrbit.Id);
		initialOrbit = initialOrbit with {Object = null!};
		initialOrbit.ShouldBeEquivalentTo(_initialOrbit);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		const string queryString = "?include=object";
		InitialOrbitDetails initialOrbit = await FetchSingle<InitialOrbitDetails>(_initialOrbit.Id, queryString);
		initialOrbit.Object!.Name.ShouldBe("Blok-I (Soyuz SL-4 (A-2))");
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<InitialOrbitDetails> orbits = await FetchMultiple<InitialOrbitDetails>();
		orbits.Count.ShouldBeGreaterThan(1);
	}
}
