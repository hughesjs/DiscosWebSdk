using System;
using System.Reflection;
using DISCOSweb_Sdk.Queries;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace DISCOSweb_Sdk.Tests.Queries;

public partial class DiscosQueryBuilderTests
{
	private readonly ITestOutputHelper _testOutputHelper;
	public DiscosQueryBuilderTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Theory]
	//[MemberData(nameof(CanAddSingleFilterTestData))]
	[ClassData(typeof(CanAddSingleFilterTestDataGenerator))]
	public void CanAddSingleFilter(TestCase testCase)
	{
		_testOutputHelper.WriteLine(testCase.TestNum.ToString());
		Type unconstructedBuilderType = typeof(DiscosQueryBuilder<>);
		Type constructedBuilderType = unconstructedBuilderType.MakeGenericType(testCase.ObjectType);
		object builder = Activator.CreateInstance(constructedBuilderType) ?? throw new("Couldn't create builder");

		Type unconstructedFilterType = typeof(FilterDefinition<,>);
		Type constructedFilterType = unconstructedFilterType.MakeGenericType(testCase.ObjectType, testCase.ParamType);
		object filter = Activator.CreateInstance(constructedFilterType, testCase.ParamName, testCase.ParamValue, testCase.Func) ?? throw new("Couldn't create filter");
		
		MethodInfo addFilterMethod = constructedBuilderType.GetMethod(nameof(DiscosQueryBuilder<object>.AddFilter)) ?? throw new("Couldn't find AddFilter method");
		MethodInfo getQueryStringMethod = constructedBuilderType.GetMethod(nameof(DiscosQueryBuilder<object>.GetQueryString)) ?? throw new("Couldn't find GetQueryString method");

		addFilterMethod.Invoke(builder, new[] {filter});
		getQueryStringMethod.Invoke(builder, null).ShouldBe(testCase.Expected);
	}
	
	[Fact]
	public void ThrowsIfFieldDoesntExist()
	{
		
	}
}



