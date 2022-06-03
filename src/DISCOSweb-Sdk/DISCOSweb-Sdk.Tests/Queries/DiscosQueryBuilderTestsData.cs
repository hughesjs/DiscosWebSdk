using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Extensions;
using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Models.ResponseModels;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;

namespace DISCOSweb_Sdk.Tests.Queries;

public partial class DiscosQueryBuilderTests
{
	/// <summary>
	/// Frankly ridiculous test data generator that should probably not exist
	/// This is an abomination unto nature
	/// </summary>
	private class CanAddSingleFilterTestDataGenerator : IEnumerable<object[]>
	{
		private readonly DiscosFunction[] _simpleFuncs =
		{
			DiscosFunction.Equal,
			DiscosFunction.NotEqual,
			DiscosFunction.GreaterThan,
			DiscosFunction.LessThan,
			DiscosFunction.GreaterThanOrEqual,
			DiscosFunction.LessThanOrEqual

		};

		private List<TestCase> GetCasesForEachPermutationOfSimpleFuncs()
		{
			int testNum = 0;
			List<TestCase> testCases = new();
			IEnumerable<Type> discosObjectTypes = typeof(DiscosObject).Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(DiscosModelBase)) && t != typeof(DiscosModelBase));
			foreach (Type objectType in discosObjectTypes)
			{
				IEnumerable<PropertyInfo> propertiesToTest = objectType.GetProperties().Where(p => p.PropertyType.IsPrimitive || p.PropertyType == typeof(string));
				foreach (PropertyInfo prop in propertiesToTest)
				{
					foreach (DiscosFunction func in _simpleFuncs)
					{
						object? fake = GenerateFake(prop);
						string res = GenerateRes(objectType, prop.Name, fake, func);
						testCases.Add(new(
										  objectType,
										  prop.PropertyType,
										  prop.Name,
										  fake,
										  func,
										  res,
										  testNum++));
						fake = null;
						if (prop.PropertyType != typeof(bool)) // bools are never null
						{
							res = GenerateRes(objectType, prop.Name, fake, func);
							testCases.Add(new(
											  objectType,
											  prop.PropertyType,
											  prop.Name,
											  null,
											  func,
											  res,
											  testNum++));
						}
					}

				}
			}
			return testCases;
		}

		private string GenerateRes(Type objectType, string propName, object? fake, DiscosFunction func)
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
			else
			{
				val = fake.ToString()!;
			}

			string propertyName = AttributeUtilities.GetJsonPropertyName(objectType.GetProperty(propName) ?? throw new("Test data generator issue"));

			return $"?filter={func.GetEnumMemberValue()}({propertyName},{val})";
		}

		private object GenerateFake(PropertyInfo prop)
		{
			Random random = new((int)DateTime.Now.Ticks);
			if (prop.PropertyType == typeof(string))
			{
				return Faker.Identification.UkNationalInsuranceNumber();
			}

			if (prop.PropertyType.IsNumericType())
			{
				return Convert.ChangeType(random.NextDouble(), prop.PropertyType);
			}

			if (prop.PropertyType == typeof(bool))
			{
				return Faker.Boolean.Random();
			}

			throw new("Forgot this case!");
		}

		public IEnumerator<object[]> GetEnumerator()
		{
			return GetCasesForEachPermutationOfSimpleFuncs().Select(tc => new object[] {tc}).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}


	public struct TestCase
	{
		public TestCase(Type objectType, Type paramType, string paramName, object paramValue, DiscosFunction func, string expected, int testNum = 0)
		{
			ObjectType = objectType;
			ParamType = paramType;
			ParamName = paramName;
			ParamValue = paramValue;
			Func = func;
			Expected = expected;
			TestNum = testNum;
		}

		public DiscosFunction Func { get; }
		public string Expected { get; }
		public string ParamName { get; }
		public object? ParamValue { get; }
		public Type ParamType { get; }
		public Type ObjectType { get; }
		public int TestNum { get; }
	}
}
