using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using DiscosWebSdk.Fixtures;
using DiscosWebSdk.Fixtures.AutoFixture;
using DiscosWebSdk.Fixtures.AutoFixture.Customizations;
using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Tests.TestDataGenerators;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Fixtures;

public class DiscosModelFixtureCustomizationNoLinksTests
{
	private readonly Fixture _fixture;

	public DiscosModelFixtureCustomizationNoLinksTests()
	{
		_fixture = new();
		_fixture.Customize(new DiscosModelFixtureCustomizationNoLinks());
	}
	
	[Theory]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public void CanCreateAllDiscosModels(Type t, string _)
	{
		DiscosModelBase model = (DiscosModelBase)new SpecimenContext(_fixture).Resolve(t);
		model.GetType().GetProperties().Select(p => p.GetValue(model)).ShouldNotBeNull();
	}
}
