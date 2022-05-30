using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.FragmentationEvent;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.FragmentationEvents;

public class FragmentationEventMapperTests: JsonApiMapperTestBase
{
	private readonly FragmentationEvent _fragmentationEvent = new()
															  {
																  Id = "86",
																  EventType = "Cosmos 699 Class (EORSAT)",
																  Epoch = new DateTime(1975,04,17),
																  Altitude = 440,
																  Comment = "Cosmos 699 was the first of a new type spacecraft. Many members of this class have experienced breakups. Beginning in 1988 old spacecraft have been commanded to lower perigee at end of life, resulting in an accelerated natural decay with fewer fragmentations. For several spacecraft, two distinct events have been detected and observational data suggest that the spacecraft remain essentially intact after each event. In all but one case, breakups occur after spacecraft has ceased orbit maintenance and entered natural decay. Debris are sometimes highly unidirectional. In the case of Cosmos 699, the spacecraft had been in a regime of natural decay for one month at the time of the event.",
																  Name = string.Empty,
																  Objects = null!,
																  Latitude = "1.0",
																  Longitude = "278.0"
															  };
	
	[Fact]
	public async Task CanFetchIndividualFragmentationEventWithoutLinks()
	{
		FragmentationEvent frag = await FetchSingle<FragmentationEvent>(_fragmentationEvent.Id);
		frag = frag with {Objects = null!};
		frag.ShouldBeEquivalentTo(_fragmentationEvent);
	}

	[Fact]
	public async Task CanFetchIndividualFragmentationEventWithLinks()
	{
		const string query = "?include=objects";
		FragmentationEvent frag = await FetchSingle<FragmentationEvent>(_fragmentationEvent.Id, query);
		frag.Objects.Count.ShouldBe(1);
		frag.Objects.First().Name.ShouldBe("Cosmos-699");
	}
	
	[Fact]
	public async Task CanFetchMultipleFragmentationEvents()
	{
		IReadOnlyList<FragmentationEvent> fragEvents = await FetchMultiple<FragmentationEvent>();
		fragEvents.Count.ShouldBeGreaterThan(1);
	}
}
