using System;
using System.Net.Http;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Extensions;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using DISCOSweb_Sdk.Models.ResponseModels.Propellants;
using DISCOSweb_Sdk.Models.ResponseModels.Reentries;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Extensions;

public class TypeExtensionsTests
{
	[Theory]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Depth))]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Height))]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Id))]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Launch))]
	[InlineData(typeof(DiscosObject), nameof(DiscosObject.Length))]
	[InlineData(typeof(Propellant), nameof(Propellant.Fuel))]
	[InlineData(typeof(Propellant), nameof(Propellant.Oxidiser))]
	[InlineData(typeof(Propellant), nameof(Propellant.Stages))]
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
	[InlineData(typeof(Propellant), "Gotta make you understand")]
	[InlineData(typeof(Propellant), "Never gonna give you up")]
	[InlineData(typeof(Propellant), "Never gonna let you down")]
	public void ThrowsIfFieldDoesntExist(Type type, string fieldName)
	{
		Should.Throw<MissingFieldException>(() => type.EnsureFieldExists(fieldName));
	}

	[Theory]
	[InlineData(typeof(DiscosObject), typeof(float?), nameof(DiscosObject.Depth))]
	[InlineData(typeof(DiscosObject), typeof(float?), nameof(DiscosObject.Height))]
	[InlineData(typeof(DiscosObject), typeof(string), nameof(DiscosObject.Id))]
	[InlineData(typeof(DiscosObject), typeof(Launch), nameof(DiscosObject.Launch))]
	[InlineData(typeof(DiscosObject), typeof(float?), nameof(DiscosObject.Length))]
	[InlineData(typeof(DiscosObject), typeof(float?), nameof(DiscosObject.Mass))]
	[InlineData(typeof(DiscosObject), typeof(Reentry), nameof(DiscosObject.Reentry))]
	public void CanEnsureFieldIsOfType(Type objectType, Type fieldType, string fieldName)
	{
		Should.NotThrow(() => objectType.EnsureFieldIsOfType(fieldName, fieldType));
	}

	[Theory]
	[InlineData(typeof(DiscosObject), typeof(HttpClient), nameof(DiscosObject.Reentry))]
	[InlineData(typeof(DiscosObject), typeof(Object), nameof(DiscosObject.Reentry))]
	[InlineData(typeof(DiscosObject), typeof(string), nameof(DiscosObject.Reentry))]
	[InlineData(typeof(DiscosObject), typeof(DiscosFunction), nameof(DiscosObject.Reentry))]
	public void ThrowsIfFieldIsNotOfType(Type objectType, Type fieldType, string fieldName)
	{
		Should.Throw<MissingFieldException>(() => objectType.EnsureFieldIsOfType(fieldName, fieldType));
	}
	
}
