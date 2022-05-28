using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi.Entities;

public class EntityMapperTests: JsonApiMapperTestBase
{
	public class CountryMapperTests : EntityMapperTests
	{
		private readonly Country _uk = new()
									  {
										  Id = "258",
										  Name = "United Kingdom",
										  Launches = null!,
										  LaunchSites = null!,
										  LaunchSystems = null!,
										  Objects = null!
									  };
		
		[Fact]
		public async Task CanFetchSingleCountryWithoutLinks()
		{
			Country country = await FetchSingle<Country>(_uk.Id);
			country = country with {Launches = null!, LaunchSites = null!, LaunchSystems = null!, Objects = null!};
			country.ShouldBeEquivalentTo(_uk);
		}

		[Fact]
		public async Task CanFetchSingleCountryWithLinks()
		{
			string[] includes = {"launches", "launchSites", "launchSystems", "objects"};
			string queryString = $"?include={string.Join(',', includes)}";
			Country country = await FetchSingle<Country>(_uk.Id, queryString);
			country.LaunchSites.Count.ShouldBe(1);
			// Testing these because of awful workaround
			country.LaunchSites.First().LatitudeDegs.ShouldBe(-31.100000);
			country.LaunchSites.First().LongitudeDegs.ShouldBe(136.600000);
		}
		
		[Fact]
		public async Task CanFetchMultipleCountries()
		{
			IReadOnlyList<Country> countries = await FetchMultiple<Country>();
		}
	}


	public class OrganisationMapperTests: EntityMapperTests
	{
		[Fact]
		public async Task CanFetchSingleOrganisationWithoutLinks()
		{
			
		}

		[Fact]
		public async Task CanFetchSingleOrganisationWithLinks()
		{
			
		}

		[Fact]
		public async Task CanFetchMultipleCountries()
		{
			
		}
	}

	[Fact]
	public async Task CanFetchMixtureOfCountriesAndOrganisations()
	{
		
	}
}
