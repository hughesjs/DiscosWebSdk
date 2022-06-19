using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using DiscosWebSdk.Enums;
using DiscosWebSdk.Exceptions;
using DiscosWebSdk.Extensions;
using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.Launches;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using DiscosWebSdk.Models.ResponseModels.Orbits;
using DiscosWebSdk.Models.ResponseModels.Propellants;
using DiscosWebSdk.Models.ResponseModels.Reentries;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Extensions;

public class TypeExtensionsTests
{
	[Theory]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Depth))]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Height))]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Id))]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Launch))]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Length))]
	[InlineData(typeof(Propellant),   nameof(Propellant.Fuel))]
	[InlineData(typeof(Propellant),   nameof(Propellant.Oxidiser))]
	[InlineData(typeof(Propellant),   nameof(Propellant.Stages))]
	public void CanEnsureFieldExists(Type type, string fieldName)
	{
		Should.NotThrow(() => type.EnsureFieldExists(fieldName));
	}

	[Theory]
	[InlineData(typeof(DiscosObject), "We're no strangers to love")]
	[InlineData(typeof(DiscosObject), "You know the rules and so do I (do I)")]
	[InlineData(typeof(DiscosObject), "A full commitment's what I'm thinking of")]
	[InlineData(typeof(DiscosObject), "You wouldn't get this from any other guy")]
	[InlineData(typeof(DiscosObject), "I just wanna tell you how I'm feeling")]
	[InlineData(typeof(Propellant),   "Gotta make you understand")]
	[InlineData(typeof(Propellant),   "Never gonna give you up")]
	[InlineData(typeof(Propellant),   "Never gonna let you down")]
	public void ThrowsIfFieldDoesntExist(Type type, string fieldName)
	{
		Should.Throw<MissingMemberException>(() => type.EnsureFieldExists(fieldName));
	}

	[Theory]
	[InlineData(typeof(DiscosObject), typeof(float?),  nameof(DiscosObject.Depth))]
	[InlineData(typeof(DiscosObject), typeof(float?),  nameof(DiscosObject.Height))]
	[InlineData(typeof(DiscosObject), typeof(string),  nameof(DiscosObject.Id))]
	[InlineData(typeof(DiscosObject), typeof(Launch),  nameof(DiscosObject.Launch))]
	[InlineData(typeof(DiscosObject), typeof(float?),  nameof(DiscosObject.Length))]
	[InlineData(typeof(DiscosObject), typeof(float?),  nameof(DiscosObject.Mass))]
	[InlineData(typeof(DiscosObject), typeof(Reentry), nameof(DiscosObject.Reentry))]
	public void CanEnsureFieldIsOfType(Type objectType, Type fieldType, string fieldName)
	{
		Should.NotThrow(() => objectType.EnsureFieldIsOfType(fieldName, fieldType, true));
	}

	[Theory]
	[InlineData(typeof(DiscosObject), typeof(float), nameof(DiscosObject.Depth))]
	[InlineData(typeof(DiscosObject), typeof(float), nameof(DiscosObject.Height))]
	[InlineData(typeof(DiscosObject), typeof(float), nameof(DiscosObject.Length))]
	[InlineData(typeof(DiscosObject), typeof(float), nameof(DiscosObject.Mass))]
	public void CanEnsureFieldIsOfTypeIgnoringNullability(Type objectType, Type fieldType, string fieldName)
	{
		Should.NotThrow(() => objectType.EnsureFieldIsOfType(fieldName, fieldType));
	}

	[Theory]
	[InlineData(typeof(DiscosObject), typeof(HttpClient),     nameof(DiscosObject.Reentry))]
	[InlineData(typeof(DiscosObject), typeof(object),         nameof(DiscosObject.Reentry))]
	[InlineData(typeof(DiscosObject), typeof(string),         nameof(DiscosObject.Reentry))]
	[InlineData(typeof(DiscosObject), typeof(DiscosFunction), nameof(DiscosObject.Reentry))]
	public void ThrowsIfFieldIsNotOfType(Type objectType, Type fieldType, string fieldName)
	{
		Should.Throw<MemberParameterTypeMismatchException>(() => objectType.EnsureFieldIsOfType(fieldName, fieldType));
	}

	[Theory]
	[InlineData(typeof(byte),         true)]
	[InlineData(typeof(byte?),        true)]
	[InlineData(typeof(float),        true)]
	[InlineData(typeof(float?),       true)]
	[InlineData(typeof(double),       true)]
	[InlineData(typeof(double?),      true)]
	[InlineData(typeof(int),          true)]
	[InlineData(typeof(int?),         true)]
	[InlineData(typeof(long),         true)]
	[InlineData(typeof(long?),        true)]
	[InlineData(typeof(uint),         true)]
	[InlineData(typeof(uint?),        true)]
	[InlineData(typeof(ulong),        true)]
	[InlineData(typeof(ulong?),       true)]
	[InlineData(typeof(string),       false)]
	[InlineData(typeof(object),       false)]
	[InlineData(typeof(DiscosObject), false)]
	[InlineData(typeof(Launch),       false)]
	[InlineData(typeof(Reentry),      false)]
	public void CanDetermineWhetherNumericType(Type t, bool isNumeric)
	{
		bool res = t.IsNumericType();
		if (isNumeric)
		{
			res.ShouldBeTrue();
		}
		else
		{
			res.ShouldBeFalse();
		}
	}

	[Theory]
	[InlineData(typeof(string[]),                   true)]
	[InlineData(typeof(List<string>),               true)]
	[InlineData(typeof(Dictionary<object, double>), true)]
	[InlineData(typeof(ConcurrentBag<object>),      true)]
	[InlineData(typeof(string[][][][][]),           true)]
	[InlineData(typeof(ServiceNameCollection),      true)]
	[InlineData(typeof(string),                     false)]
	[InlineData(typeof(object),                     false)]
	[InlineData(typeof(Launch),                     false)]
	[InlineData(typeof(Reentry),                    false)]
	[InlineData(typeof(DiscosObject),               false)]
	public void CanDetermineWhetherCollectionType(Type t, bool isNumeric)
	{
		bool res = t.IsCollectionType();
		if (isNumeric)
		{
			res.ShouldBeTrue();
		}
		else
		{
			res.ShouldBeFalse();
		}
	}

	[Theory]
	[InlineData(typeof(DiscosObject),                           true)]
	[InlineData(typeof(DiscosObjectClass),                      true)]
	[InlineData(typeof(Launch),                                 true)]
	[InlineData(typeof(LaunchSite),                             true)]
	[InlineData(typeof(LaunchVehicle),                          true)]
	[InlineData(typeof(LaunchVehicleFamily),                    true)]
	[InlineData(typeof(Propellant),                             true)]
	[InlineData(typeof(Entity),                                 true)]
	[InlineData(typeof(Country),                                true)]
	[InlineData(typeof(Organisation),                           true)]
	[InlineData(typeof(LaunchVehicleEngine),                    true)]
	[InlineData(typeof(InitialOrbitDetails),                    true)]
	[InlineData(typeof(DestinationOrbitDetails),                true)]
	[InlineData(typeof(IReadOnlyList<DiscosObject>),            true)]
	[InlineData(typeof(IReadOnlyList<DiscosObjectClass>),       true)]
	[InlineData(typeof(IReadOnlyList<Launch>),                  true)]
	[InlineData(typeof(IReadOnlyList<LaunchSite>),              true)]
	[InlineData(typeof(IReadOnlyList<LaunchVehicle>),           true)]
	[InlineData(typeof(IReadOnlyList<LaunchVehicleFamily>),     true)]
	[InlineData(typeof(IReadOnlyList<Propellant>),              true)]
	[InlineData(typeof(IReadOnlyList<Entity>),                  true)]
	[InlineData(typeof(IReadOnlyList<Country>),                 true)]
	[InlineData(typeof(IReadOnlyList<Organisation>),            true)]
	[InlineData(typeof(IReadOnlyList<LaunchVehicleEngine>),     true)]
	[InlineData(typeof(IReadOnlyList<InitialOrbitDetails>),     true)]
	[InlineData(typeof(IReadOnlyList<DestinationOrbitDetails>), true)]
	[InlineData(typeof(string),                                 false)]
	[InlineData(typeof(object),                                 false)]
	[InlineData(typeof(bool),                                   false)]
	[InlineData(typeof(int),                                    false)]
	[InlineData(typeof(float),                                  false)]
	[InlineData(typeof(decimal),                                false)]
	[InlineData(typeof(double),                                 false)]
	[InlineData(typeof(IReadOnlyList<string>),                  false)]
	[InlineData(typeof(IReadOnlyList<object>),                  false)]
	[InlineData(typeof(IReadOnlyList<bool>),                    false)]
	[InlineData(typeof(IReadOnlyList<int>),                     false)]
	[InlineData(typeof(IReadOnlyList<float>),                   false)]
	[InlineData(typeof(IReadOnlyList<decimal>),                 false)]
	[InlineData(typeof(IReadOnlyList<double>),                  false)]
	public void CanDetermineWhetherDiscosModel(Type t, bool isDiscosModel)
	{
		bool res = t.IsDiscosModel();
		if (isDiscosModel)
		{
			res.ShouldBeTrue();
		}
		else
		{
			res.ShouldBeFalse();
		}
	}
}
