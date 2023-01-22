using System;
using System.Reflection;
using DiscosWebSdk.Extensions;

namespace DiscosWebSdk.Tests.Fixtures.AutoFixture.PropertySpecifications;

internal class DiscosModelPropertyComparer : IEquatable<PropertyInfo>
{
	public bool Equals(PropertyInfo? other) => other?.PropertyType.IsDiscosModel() ?? false;
}
