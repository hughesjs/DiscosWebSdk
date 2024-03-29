using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscosWebSdk.Exceptions;
using DiscosWebSdk.Models.ResponseModels.Entities;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Mapping.JsonApi.Entities;

public class EntityMapperTests : JsonApiMapperTestBase
{

	[Fact]
	public async Task CanFetchMixtureOfCountriesAndOrganisations()
	{
		IReadOnlyList<Entity> entities = await FetchMultiple<Entity>("?filter=contains(name,'United')");
		entities.Select(e => e.Name).ShouldContain("United Launch Alliance/Decatur");
		entities.Select(e => e.Name).ShouldContain("United Kingdom");
		entities.Select(e => e.Name).ShouldContain("United States");
	}

	public override async Task CanGetSingleWithoutLinks() => await new CountryMapperTests().CanGetSingleWithoutLinks();

	public override async Task CanGetSingleWithLinks() => await new OrganisationMapperTests().CanGetSingleWithLinks();

	public override async Task CanGetMultiple() => await CanFetchMixtureOfCountriesAndOrganisations();


	public class CountryMapperTests : EntityMapperTests
	{
		private readonly Country _uk = new()
									   {
										   Id            = "258",
										   Name          = "United Kingdom",
										   Launches      = null!,
										   LaunchSites   = null!,
										   LaunchSystems = null!,
										   Objects       = null!
									   };

		[Fact]
		public override async Task CanGetSingleWithoutLinks()
		{
			Country country = await FetchSingle<Country>(_uk.Id);
			country = country with {Launches = null!, LaunchSites = null!, LaunchSystems = null!, Objects = null!};
			country.ShouldBeEquivalentTo(_uk);
		}

		[Fact]
		public override async Task CanGetSingleWithLinks()
		{
			string[] includes    = {"launchSites", "launchSystems"};
			string   queryString = $"?include={string.Join(',', includes)}";
			Country  country     = await FetchSingle<Country>(_uk.Id, queryString);
			country.LaunchSites.Count.ShouldBe(2);
			// Testing these because of awful workaround
			country.LaunchSites.First().LatitudeDegs.ShouldBe(-31.100000);
			country.LaunchSites.First().LongitudeDegs.ShouldBe(136.600000);
		}

		[Fact]
		public override async Task CanGetMultiple()
		{
			IReadOnlyList<Country> countries = await FetchMultiple<Country>("?filter=contains(name,'Republic')");
			countries.Count.ShouldBeGreaterThan(2);
		}
	}


	public class OrganisationMapperTests : EntityMapperTests
	{
		private readonly Organisation _spaceX = new()
												{
													Id            = "738",
													Name          = "SpaceX",
													HostCountry   = null!,
													Launches      = null!,
													LaunchSites   = null!,
													LaunchSystems = null!,
													Objects       = null!
												};

		[Fact]
		public override async Task CanGetSingleWithoutLinks()
		{
			Organisation country = await FetchSingle<Organisation>(_spaceX.Id);
			country = country with {Launches = null!, LaunchSites = null!, LaunchSystems = null!, Objects = null!, HostCountry = null!};
			country.ShouldBeEquivalentTo(_spaceX);
		}

		[Fact]
		public override async Task CanGetSingleWithLinks()
		{
			string[]     includes    = {"launchSites", "launchSystems", "hostCountry"};
			string       queryString = $"?include={string.Join(',', includes)}";
			Organisation org         = await FetchSingle<Organisation>(_spaceX.Id, queryString);
			org.LaunchSites.Count.ShouldBe(0);
			org.HostCountry.Name.ShouldBe("United States");
		}

		[Fact]
		public override async Task CanGetMultiple()
		{
			IReadOnlyList<Organisation> orgs = await FetchMultiple<Organisation>();
			orgs.Count.ShouldBeGreaterThan(1);
		}
	}
}
