using System;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Exceptions.Queries.Filters.FilterDefinitions;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using DISCOSweb_Sdk.Queries.Filters;
using DISCOSweb_Sdk.Tests.TestCaseModels;
using DISCOSweb_Sdk.Tests.TestDataGenerators;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Queries.Filters;

public class FilterDefinitionTests
{

	[Fact]
	public void CanGetStringFromNonGenericBase()
	{
		FilterDefinition<DiscosObject, string> filter = new(nameof(DiscosObject.Id), "123", DiscosFunction.Equal);
		// ReSharper disable once RedundantCast
		filter.ToString().ShouldBe(((FilterDefinition)filter).ToString());

	}

	[Fact]
	public void ThrowsIfFieldDoesntExist()
	{
		Should.Throw<InvalidPropertyOnFilterDefinitionException>(() => new FilterDefinition<DiscosObject, string>("42", null, DiscosFunction.Contains));
	}

	[Fact]
	public void ThrowsIfNonArrayParamIsOfWrongType()
	{
		Should.Throw<TypeMismatchOnFilterDefinitionException>(() => new FilterDefinition<DiscosObject, float>(nameof(DiscosObject.Id), 0f, DiscosFunction.Contains));
	}

	[Fact]
	public void DoesntThrowWhenElementTypeOfArrayParamMatchesMemberType()
	{
		Should.NotThrow(() => new FilterDefinition<DiscosObject, string[]>(nameof(DiscosObject.Name), new[] {"1", "2"}, DiscosFunction.Contains));
	}

	[Fact]
	public void ThrowsWhenArrayParamDoesntMatchFieldType()
	{
		Should.Throw<TypeMismatchOnFilterDefinitionException>(() => new FilterDefinition<DiscosObject, string[]>(nameof(DiscosObject.Height), new[] {"1", "2"}, DiscosFunction.Contains));
	}

	[Theory]
	[InlineData(DiscosFunction.Equal)]
	[InlineData(DiscosFunction.NotEqual)]
	[InlineData(DiscosFunction.LessThan)]
	[InlineData(DiscosFunction.LessThanOrEqual)]
	[InlineData(DiscosFunction.GreaterThan)]
	[InlineData(DiscosFunction.GreaterThanOrEqual)]
	public void ThrowsWhenArrayProvidedToNonSupportingMethod(DiscosFunction function)
	{
		Should.Throw<DiscosFunctionDoesntSupportArraysException>(() => new FilterDefinition<DiscosObject, float[]>(nameof(DiscosObject.Height), new[] {0f, 0f}, function));
	}

	[Fact]
	public void GeneratesCorrectStringForNullValue()
	{
		FilterDefinition<DiscosObject, string> filter = new(nameof(DiscosObject.Name), null, DiscosFunction.Equal);
		filter.ToString().ShouldBe("eq(name,null)");
	}

	[Theory]
	[InlineData(false, "false")]
	[InlineData(true,  "true")]
	public void GeneratesCorrectStringForBoolValue(bool isTrue, string expected)
	{
		FilterDefinition<Launch, bool> filter = new(nameof(Launch.Failure), isTrue, DiscosFunction.Equal);
		filter.ToString().ShouldBe($"eq(failure,{expected})");
	}

	[Fact]
	public void GeneratesCorrectStringForStringArrayValue()
	{
		FilterDefinition<DiscosObject, string[]> filter = new(nameof(DiscosObject.Id), new[] {"1", "2"}, DiscosFunction.Contains);
		filter.ToString().ShouldBe("contains(id,('1','2'))");
	}

	[Fact]
	public void GeneratesCorrectStringForNumericArrayValue()
	{
		FilterDefinition<DiscosObject, float[]> filter = new(nameof(DiscosObject.Height), new[] {0f, 0f}, DiscosFunction.Contains);
		filter.ToString().ShouldBe("contains(height,(0,0))");
	}


	[Theory]
	[ClassData(typeof(SingleFilterTestDataGenerator))]
	public void GeneratesCorrectStringForEveryPermutation(SingleFilterTestCase singleFilterTestCase)
	{
		Type             unconstructedFilterType = typeof(FilterDefinition<,>);
		Type             constructedFilterType   = unconstructedFilterType.MakeGenericType(singleFilterTestCase.ObjectType, singleFilterTestCase.ParamType);
		FilterDefinition filter                  = (FilterDefinition)Activator.CreateInstance(constructedFilterType, singleFilterTestCase.ParamName, singleFilterTestCase.ParamValue, singleFilterTestCase.Func)! ?? throw new("Couldn't create filter");

		filter.ToString().ShouldBe(singleFilterTestCase.Expected);
	}
}
