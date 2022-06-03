using System;
using System.Collections.Generic;
using System.Reflection;
using DISCOSweb_Sdk.Extensions;
using DISCOSweb_Sdk.Queries;
using DISCOSweb_Sdk.Tests.TestCaseModels;
using DISCOSweb_Sdk.Tests.TestDataGenerators;
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
	public void CanAddSingleFilter(SingleFilterTestCase singleFilterTestCase)
	{
		_testOutputHelper.WriteLine(singleFilterTestCase.TestNum.ToString());
		Type unconstructedBuilderType = typeof(DiscosQueryBuilder<>);
		Type constructedBuilderType = unconstructedBuilderType.MakeGenericType(singleFilterTestCase.ObjectType);
		object builder = Activator.CreateInstance(constructedBuilderType) ?? throw new("Couldn't create builder");

		Type unconstructedFilterType = typeof(FilterDefinition<,>);
		Type constructedFilterType = unconstructedFilterType.MakeGenericType(singleFilterTestCase.ObjectType, singleFilterTestCase.ParamType);
		object filter = Activator.CreateInstance(constructedFilterType, singleFilterTestCase.ParamName, singleFilterTestCase.ParamValue, singleFilterTestCase.Func) ?? throw new("Couldn't create filter");
		
		MethodInfo addFilterMethod = constructedBuilderType.GetMethod(nameof(DiscosQueryBuilder<object>.AddFilter)) ?? throw new("Couldn't find AddFilter method");
		MethodInfo getQueryStringMethod = constructedBuilderType.GetMethod(nameof(DiscosQueryBuilder<object>.GetQueryString)) ?? throw new("Couldn't find GetQueryString method");

		addFilterMethod.Invoke(builder, new[] {filter});
		getQueryStringMethod.Invoke(builder, null).ShouldBe(singleFilterTestCase.Expected);
	}
	
	[Fact]
	public void ThrowsIfFieldDoesntExist()
	{
		
	}
}



