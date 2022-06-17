using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DiscosWebSdk.Enums;
using DiscosWebSdk.Extensions;
using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Tests.TestCaseModels;
using Faker;
using Boolean = Faker.Boolean;

namespace DiscosWebSdk.Tests.TestDataGenerators;

/// <summary>
///     Frankly ridiculous test data generator that should probably not exist.
///     This is an abomination unto nature.
///     But... It was quite fun to write so here we are.
///     Some of these queries are invalid for type reasons... I *might* fix this later.
/// </summary>
internal class SingleFilterTestDataGenerator : IEnumerable<object[]>
{

	private readonly DiscosFunction[] _arrayFields =
	{
		DiscosFunction.Includes,
		DiscosFunction.DoesNotInclude,
		DiscosFunction.Contains,
		DiscosFunction.Excludes
	};

	private readonly DiscosFunction[] _simpleFuncs =
	{
		DiscosFunction.Equal,
		DiscosFunction.NotEqual,
		DiscosFunction.GreaterThan,
		DiscosFunction.LessThan,
		DiscosFunction.GreaterThanOrEqual,
		DiscosFunction.LessThanOrEqual,
		DiscosFunction.Contains,
		DiscosFunction.Excludes
	};
	private int _testNumber;

	public IEnumerator<object[]> GetEnumerator()
	{
		List<SingleFilterTestCase> testCases = new();
		testCases.AddRange(GetCasesForEachPermutationOfSimpleFuncs());
		testCases.AddRange(GetCasesForEachPermutationOfArrayFuncs());
		return testCases.Select(tc => new object[] {tc}).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private List<SingleFilterTestCase> GetCasesForEachPermutationOfSimpleFuncs()
	{
		List<SingleFilterTestCase>       testCases    = new();
		Dictionary<Type, PropertyInfo[]> permutations = GetDiscosObjectsPrimitivePropertiesPermutations();
		foreach (KeyValuePair<Type, PropertyInfo[]> permutation in permutations)
		{
			Type objectType = permutation.Key;
			foreach (PropertyInfo prop in permutation.Value)
			{
				foreach (DiscosFunction func in _simpleFuncs)
				{
					testCases.Add(GenerateSimpleTestCase(prop, objectType, func));
					if (prop.PropertyType != typeof(bool)) // bools are never null
					{
						testCases.Add(GenerateSimpleTestCaseForNullValue(prop, objectType, func));
					}
				}

			}
		}
		return testCases;
	}

	private SingleFilterTestCase GenerateSimpleTestCase(PropertyInfo prop, Type objectType, DiscosFunction func)
	{
		object fake = GenerateFake(prop);
		string res  = GenerateRes(objectType, prop.Name, fake, func);
		return new(
				   objectType,
				   prop.PropertyType,
				   prop.Name,
				   fake,
				   func,
				   res,
				   _testNumber++);
	}

	private SingleFilterTestCase GenerateSimpleTestCaseForNullValue(PropertyInfo prop, Type objectType, DiscosFunction func)
	{
		string res = GenerateRes(objectType, prop.Name, null, func);
		return new(
				   objectType,
				   prop.PropertyType,
				   prop.Name,
				   null,
				   func,
				   res,
				   _testNumber++);
	}

	private List<SingleFilterTestCase> GetCasesForEachPermutationOfArrayFuncs()
	{
		List<SingleFilterTestCase>       testCases    = new();
		Dictionary<Type, PropertyInfo[]> permutations = GetDiscosObjectsPrimitivePropertiesPermutations();
		foreach (KeyValuePair<Type, PropertyInfo[]> permutation in permutations)
		{
			Type objectType = permutation.Key;
			foreach (PropertyInfo prop in permutation.Value.Where(p => p.PropertyType != typeof(bool)))
			{
				foreach (DiscosFunction func in _arrayFields)
				{
					testCases.Add(GenerateArrayTestCase(prop, objectType, func));
				}
			}
		}
		return testCases;
	}

	private SingleFilterTestCase GenerateArrayTestCase(PropertyInfo prop, Type objectType, DiscosFunction func)
	{
		object fake = GenerateFakeArray(prop);
		string res  = GenerateRes(objectType, prop.Name, fake, func);
		return new(
				   objectType,
				   prop.PropertyType.MakeArrayType(),
				   prop.Name,
				   fake,
				   func,
				   res,
				   _testNumber++
				  );
	}

	private object GenerateFakeArray(PropertyInfo prop)
	{
		const int arrSize = 5;
		Array     arr     = Array.CreateInstance(prop.PropertyType, 5);
		for (int i = 0; i < arrSize; i++)
		{
			arr.SetValue(GenerateFake(prop), i);
		}
		return arr;
	}

	private string GenerateRes(Type objectType, string propName, object? fake, DiscosFunction func)
	{
		string val = GetStringRepresentation(fake);

		string propertyName = AttributeUtilities.GetJsonPropertyName(objectType.GetProperty(propName) ?? throw new("Test data generator issue"));

		return $"{func.GetEnumMemberValue()}({propertyName},{val})";
	}

	private string GetStringRepresentation(object? fake)
	{
		string val = string.Empty;
		if (fake is null)
		{
			val = "null";
		}
		else if (fake is string)
		{
			val = fake is string ? $"'{fake}'" : fake.ToString() ?? throw new("Missed something!");
		}
		else if (fake is bool isTrue)
		{
			val = isTrue ? "true" : "false";
		}
		else if (fake.GetType().IsCollectionType())
		{
			val += '(';
			List<string> components = new();
			foreach (object item in (IEnumerable)fake)
			{
				components.Add(GetStringRepresentation(item));
			}
			val += string.Join(',', components);
			val += ')';
		}
		else
		{
			val = fake.ToString()!;
		}
		return val;
	}

	private object GenerateFake(PropertyInfo prop)
	{
		Random random = new((int)DateTime.Now.Ticks);
		if (prop.PropertyType == typeof(string))
		{
			return Identification.UkNationalInsuranceNumber();
		}

		if (prop.PropertyType.IsNumericType())
		{
			return Convert.ChangeType(random.NextDouble(), prop.PropertyType);
		}

		if (prop.PropertyType == typeof(bool))
		{
			return Boolean.Random();
		}

		throw new("Forgot this case!");
	}

	private Dictionary<Type, PropertyInfo[]> GetDiscosObjectsPrimitivePropertiesPermutations()
	{
		Dictionary<Type, PropertyInfo[]> dict              = new();
		IEnumerable<Type>                discosObjectTypes = typeof(DiscosObject).Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(DiscosModelBase)) && t != typeof(DiscosModelBase));
		foreach (Type objectType in discosObjectTypes)
		{
			IEnumerable<PropertyInfo> propertiesToTest = objectType.GetProperties().Where(p => p.PropertyType.IsPrimitive || p.PropertyType == typeof(string));
			dict.Add(objectType, propertiesToTest.ToArray());
		}
		return dict;
	}
}
