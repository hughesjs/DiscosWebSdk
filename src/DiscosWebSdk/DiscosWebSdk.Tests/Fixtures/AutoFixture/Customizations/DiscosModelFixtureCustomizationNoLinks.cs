using AutoFixture;
using AutoFixture.Kernel;
using DiscosWebSdk.Tests.Fixtures.AutoFixture.PropertySpecifications;

namespace DiscosWebSdk.Tests.Fixtures.AutoFixture.Customizations;

public class DiscosModelFixtureCustomizationNoLinks: ICustomization
{
	public void Customize(IFixture fixture)
	{
		Omitter omitter = new(new PropertySpecification(new DiscosModelPropertyComparer()));
		fixture.Customizations.Add(omitter);
	}
}
