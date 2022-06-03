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
	/// Frankly ridiculous test data generator that should probably not exist.
	/// This is an abomination unto nature.
	/// But... It was quite fun to write so here we are.
	/// </summary>
	private class CanAddSingleFilterTestDataGenerator : IEnumerable<object[]>
	{
		private int _testNumber;
		
		private readonly DiscosFunction[] _simpleFuncs =
		{
			DiscosFunction.Equal,
			DiscosFunction.NotEqual,
			DiscosFunction.GreaterThan,
			DiscosFunction.LessThan,
			DiscosFunction.GreaterThanOrEqual,
			DiscosFunction.LessThanOrEqual
		};

		private readonly DiscosFunction[] _arrayFields =
		{
			DiscosFunction.Includes,
			DiscosFunction.DoesNotInclude
		};

		private readonly DiscosFunction[] _hybridFields =
		{
			DiscosFunction.Contains,
			DiscosFunction.Excludes
		};
		public CanAddSingleFilterTestDataGenerator()
		{
			_testNumber = 0;
		}

		private List<TestCase> GetCasesForEachPermutationOfSimpleFuncs()
		{
			List<TestCase> testCases = new();
			Dictionary<Type, PropertyInfo[]> permutations = GetDiscosObjectsPrimitivePropertiesPermutations();
			foreach (KeyValuePair<Type, PropertyInfo[]> permutation in permutations)
			{
				Type objectType = permutation.Key;
				foreach (PropertyInfo prop in permutation.Value)
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
										  _testNumber++));
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
											  _testNumber++));
						}
					}

				}
			}
			return testCases;
		}

		private List<TestCase> GetCasesForEachPermutationOfArrayFuncs()
		{
			List<TestCase> testCases = new();
			Dictionary<Type, PropertyInfo[]> permutations = GetDiscosObjectsPrimitivePropertiesPermutations();
			foreach (KeyValuePair<Type, PropertyInfo[]> permutation in permutations)
			{
				Type objectType = permutation.Key;
				foreach (PropertyInfo prop in permutation.Value.Where(p => p.PropertyType != typeof(bool)))
				{
					foreach (DiscosFunction func in _arrayFields)
					{
						object fake = GenerateFakeArray(prop);
						
						string res = GenerateRes(objectType, prop.Name, fake, func);
						testCases.Add(new(
										  objectType,
										  prop.PropertyType.MakeArrayType(),
										  prop.Name,
										  fake,
										  func,
										  res,
										  _testNumber++));
					}
				}
			}
			return testCases;
		}

		private object GenerateFakeArray(PropertyInfo prop)
		{
			const int arrSize = 5;
			Array arr = Array.CreateInstance(prop.PropertyType, 5);
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

			return $"?filter={func.GetEnumMemberValue()}({propertyName},{val})";
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

		private Dictionary<Type, PropertyInfo[]> GetDiscosObjectsPrimitivePropertiesPermutations()
		{
			Dictionary<Type, PropertyInfo[]> dict = new();
			IEnumerable<Type> discosObjectTypes = typeof(DiscosObject).Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(DiscosModelBase)) && t != typeof(DiscosModelBase));
			foreach (Type objectType in discosObjectTypes)
			{
				IEnumerable<PropertyInfo> propertiesToTest = objectType.GetProperties().Where(p => p.PropertyType.IsPrimitive || p.PropertyType == typeof(string));
				dict.Add(objectType, propertiesToTest.ToArray());
			}
			return dict;
		}

		public IEnumerator<object[]> GetEnumerator()
		{
			List<TestCase> testCases = new();
			testCases.AddRange(GetCasesForEachPermutationOfSimpleFuncs());
			testCases.AddRange(GetCasesForEachPermutationOfArrayFuncs());
			return testCases.Select(tc => new object[] {tc}).GetEnumerator();
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
