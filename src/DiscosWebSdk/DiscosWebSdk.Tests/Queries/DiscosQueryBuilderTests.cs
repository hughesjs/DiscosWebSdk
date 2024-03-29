using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DiscosWebSdk.Enums;
using DiscosWebSdk.Exceptions.Queries.Filters.FilterTree;
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.Propellants;
using DiscosWebSdk.Queries.Builders;
using DiscosWebSdk.Queries.Filters;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Queries;


[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class DiscosQueryBuilderTests
{
	public class Generic
	{
		private readonly IDiscosQueryBuilder<DiscosObject> _builder;

		public Generic() => _builder = new DiscosQueryBuilder<DiscosObject>();

		[Fact]
		public void CanAddFilter()
		{
			FilterDefinition filter = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Name), "123", DiscosFunction.Equal);
			_builder.AddFilter(filter);
			_builder.Build().ShouldBe($"?filter={filter}");
		}

		[Fact]
		public void ThrowsIfFilterTObjectDoesntMatchBuilderType()
		{
			FilterDefinition filter = new FilterDefinition<Propellant, string>(nameof(DiscosObject.Name), "123", DiscosFunction.Equal);
			Should.Throw<InvalidFilterTreeException>(() => _builder.AddFilter(filter));
		}

		[Fact]
		public void CanAddCompoundAndFilter()
		{
			FilterDefinition f1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Name), "123", DiscosFunction.Equal);
			FilterDefinition f2 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Name), "321", DiscosFunction.Equal);

			_builder.AddFilter(f1).And().AddFilter(f2);
			_builder.Build().ShouldBe($"?filter=and({f1},{f2})");
		}

		[Fact]
		public void CanAddCompoundOrFilter()
		{
			FilterDefinition f1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Name), "123", DiscosFunction.Equal);
			FilterDefinition f2 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Name), "321", DiscosFunction.Equal);

			_builder.AddFilter(f1).Or().AddFilter(f2);
			_builder.Build().ShouldBe($"?filter=or({f1},{f2})");
		}


		[Fact]
		public void CanAddInclude()
		{
			_builder.AddInclude(nameof(DiscosObject.Reentry));
			_builder.Build().ShouldBe("?include=reentry");
		}

		[Theory]
		[InlineData(typeof(Entity))]
		[InlineData(typeof(Country))]
		[InlineData(typeof(Organisation))]
		public void ExcludesLaunchesAndObjectsFromAddingAllIncludesOnEntities(Type entityType)
		{
			Type               queryBuilderType = typeof(DiscosQueryBuilder<>).MakeGenericType(entityType);
			DiscosQueryBuilder builder          = Activator.CreateInstance(queryBuilderType) as DiscosQueryBuilder ?? throw new("Can't construct query builder");
			builder.AddAllIncludes(entityType);
			
			string query = builder.Build();
			query.ShouldNotContain("launches");
			query.ShouldNotContain("objects");
		}
		

		[Fact]
		public void ThrowsIfIncludeFieldDoesntExist()
		{
			Should.Throw<MissingMemberException>(() => _builder.AddInclude("Notice me Senpai!! >~<"));
		}

		[Fact]
		public void CanAddAllIncludes()
		{
			_builder.AddAllIncludes();
			string query = _builder.Build();
			query.StartsWith("?include=").ShouldBeTrue();
			query = query.Replace("?include=","");
			query.Split(',').ToList().ForEach(i => new [] {"states", "reentry","destinationOrbits","initialOrbits","operators","launch"}.ShouldContain(i));
		}

		[Fact]
		public void CanAddPageSize()
		{
			_builder.AddPageSize(555);
			_builder.Build().ShouldBe("?page[size]=555");
		}

		[Fact]
		public void CanAddPageNum()
		{
			_builder.AddPageNum(666);
			_builder.Build().ShouldBe("?page[number]=666");
		}

		[Fact]
		public void CanResetBuilder()
		{
			_builder.AddAllIncludes().Reset().Build().ShouldBeEmpty();
		}

		[Fact]
		public void BuildReturnsEmptyStringIfNothingAdded()
		{
			_builder.Build().ShouldBeEmpty();
		}

		[Fact]
		public void NonEmptyBuildPrependsQuestionMark()
		{
			_builder.AddPageNum(0);
			_builder.Build().First().ShouldBe('?');
		}

		[Fact]
		public void AmpersandIsAddedBetweenFields()
		{
			_builder.AddPageNum(1);
			_builder.AddPageSize(2);
			_builder.AddAllIncludes();
			_builder.Build().Count(c => c == '&').ShouldBe(2);
		}
	}

	public class NotGeneric
	{
		private readonly IDiscosQueryBuilder _builder;
		public NotGeneric() => _builder = new DiscosQueryBuilder();
		
		[Fact]
		public void CanAddAllIncludes()
		{
			_builder.AddAllIncludes(typeof(DiscosObject));
			string query = _builder.Build();
			query.StartsWith("?include=").ShouldBeTrue();
			query = query.Replace("?include=","");
			query.Split(',').ToList().ForEach(i => new [] {"states", "reentry","destinationOrbits","initialOrbits","operators","launch"}.ShouldContain(i));
		}

		[Fact]
		public void CanAddPageSize()
		{
			_builder.AddPageSize(555);
			_builder.Build().ShouldBe("?page[size]=555");
		}

		[Fact]
		public void CanAddPageNum()
		{
			_builder.AddPageNum(666);
			_builder.Build().ShouldBe("?page[number]=666");
		}

		
		[Fact]
		public void CanAddSingleInclude()
		{
			_builder.AddInclude(typeof(DiscosObject), nameof(DiscosObject.Reentry));
			_builder.Build().ShouldBe("?include=reentry");
		}
		
		[Fact]
		public void CanResetBuilder()
		{
			_builder.AddAllIncludes(typeof(DiscosObject)).Reset().Build().ShouldBeEmpty();
		}
		
		[Fact]
		public void BuildReturnsEmptyStringIfNothingAdded()
		{
			_builder.Build().ShouldBeEmpty();
		}

		[Fact]
		public void NonEmptyBuildPrependsQuestionMark()
		{
			_builder.AddPageNum(0);
			_builder.Build().First().ShouldBe('?');
		}

		[Fact]
		public void AmpersandIsAddedBetweenFields()
		{
			_builder.AddPageNum(1);
			_builder.AddPageSize(2);
			_builder.AddAllIncludes(typeof(DiscosObject));
			_builder.Build().Count(c => c == '&').ShouldBe(2);
		}
	}
}
