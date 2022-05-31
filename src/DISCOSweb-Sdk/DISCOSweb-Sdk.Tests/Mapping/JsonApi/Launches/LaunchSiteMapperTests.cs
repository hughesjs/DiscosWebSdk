using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using DISCOSweb_Sdk.Models.SubObjects;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.Launches;

public class LaunchSiteMapperTests : JsonApiMapperTestBase
{
	private readonly LaunchSite _site = new()
										{
											Id = "14",
											Altitude = 0,
											Azimuths = new()
													   {
														   new()
														   {
															   Display = "[90,191]",
															   LowerInc = true,
															   UpperInc = true,
															   Upper = 191.0f,
															   Lower = 90.0f,
															   Empty = false
														   }
													   },
											Constraints = new()
														  {
															  "Jan/Feb",
															  "Aug/Sept"
														  },
											Pads = new() {"Y", "O"},
											Name = "Tanegashima Space Center",
											Latitude = "30.400000",
											Longitude = "130.600000",
											Launches = null!,
											Operators = null!
										};
	
	[Fact]
	public override async Task CanGetSingleWithoutLinks()
	{
		LaunchSite site = await FetchSingle<LaunchSite>(_site.Id);
		site = site with {Launches = null!, Operators = null!};
		site.ShouldBeEquivalentTo(_site);
	}

	[Fact]
	public override async Task CanGetSingleWithLinks()
	{
		string[] includes = {"operators", "launches"};
		string queryString = $"?include={string.Join(',', includes)}";
		LaunchSite site = await FetchSingle<LaunchSite>(_site.Id, queryString);
		site.Operators.Count.ShouldBe(1);
		site.Operators.First().Name.ShouldBe("Japan");
		site.Launches.Count.ShouldBe(85);
		site.Launches.First().CosparLaunchNo.ShouldBe("2020-089");
	}

	[Fact]
	public override async Task CanGetMultiple()
	{
		IReadOnlyList<LaunchSite> launches = await FetchMultiple<LaunchSite>();
		launches.Count.ShouldBeGreaterThan(1);
	}
}
