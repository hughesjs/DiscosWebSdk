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
			IReadOnlyList<Country> countries = await FetchMultiple<Country>("?filter=contains(name,'Republic')");
			countries.Count.ShouldBeGreaterThan(1);
		}
	}


	public class OrganisationMapperTests: EntityMapperTests
	{
		private readonly Organisation _spaceX = new()
												{
													Id = "738",
													Name = "SpaceX",
													HostCountry = null!,
													Launches = null!,
													LaunchSites = null!,
													LaunchSystems = null!,
													Objects = null!
												};
		[Fact]
		public async Task CanFetchSingleOrganisationWithoutLinks()
		{
			Organisation country = await FetchSingle<Organisation>(_spaceX.Id);
			country = country with {Launches = null!, LaunchSites = null!, LaunchSystems = null!, Objects = null!, HostCountry = null!};
			country.ShouldBeEquivalentTo(_spaceX);
		}

		[Fact]
		public async Task CanFetchSingleOrganisationWithLinks()
		{
			string[] includes = {"launches", "launchSites", "launchSystems", "objects", "hostCountry"};
			string queryString = $"?include={string.Join(',', includes)}";
			Organisation org = await FetchSingle<Organisation>(_spaceX.Id, queryString);
			org.LaunchSites.Count.ShouldBe(0);
			org.Launches.Count.ShouldBeGreaterThan(130); //135 as of 2022-05-28
			org.Objects.Count.ShouldBeGreaterThan(150); //159 as of 2022-05-28
		}

		[Fact]
		public async Task CanFetchMultipleCountries()
		{
			IReadOnlyList<Organisation> orgs = await FetchMultiple<Organisation>();
			orgs.Count.ShouldBeGreaterThan(1);
		}
	}

	[Fact]
	public async Task CanFetchMixtureOfCountriesAndOrganisations()
	{
		IReadOnlyList<Entity> entities = await FetchMultiple<Entity>("?filter=contains(name,'United')");
		entities.Select(e => e.Name).ShouldContain("United Launch Alliance/Decatur");
		entities.Select(e => e.Name).ShouldContain("United Kingdom");
		entities.Select(e => e.Name).ShouldContain("United States");
	}
}
