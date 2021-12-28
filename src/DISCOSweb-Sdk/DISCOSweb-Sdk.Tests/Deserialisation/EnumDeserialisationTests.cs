using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Enums;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Deserialisation;

public class EnumDeserialisationTests
{
	[Theory]
	[ClassData(typeof(TestDataGenerator))]
	public void CanDeserialiseEnumsWithCustomJsonStrings(Enum expected, string jsonName)
	{
		jsonName.ShouldNotBeNullOrEmpty();
		ReadOnlySpan<char> json = $"{{\"TestEnum\":\"{jsonName}\"}}";

		Type constructed = typeof(TestEnumWrapper<>).MakeGenericType(expected.GetType());
		
		var res = JsonSerializer.Deserialize(json, constructed);

		constructed.GetProperty("TestEnum").GetValue(res).ShouldBe(expected);
	}


	private class TestDataGenerator: IEnumerable<object[]>
	{

		public IEnumerator<object[]> GetEnumerator()
		{
			var allEnums = typeof(RecordType).Assembly.GetTypes().Where(t => t.IsEnum);
			var testEnums = allEnums.Where(e => e.GetCustomAttributes().Any(a => a is JsonConverterAttribute converterAttribute && converterAttribute.ConverterType == typeof(JsonStringEnumConverter)));
			var enumMemberInfo = testEnums.SelectMany(e => e.GetMembers(BindingFlags.Static | BindingFlags.Public));
			IEnumerable<object[]> enumsWithNames = enumMemberInfo.Select(e =>
																		 {
																			 Enum.TryParse(e.Name, out RecordType res);
																			 return new object[]
																					{
																						res,
																						((JsonPropertyNameAttribute)e.GetCustomAttributes().First(a => a is JsonPropertyNameAttribute)).Name
																					};
																		 });
			return enumsWithNames.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}


	private class TestEnumWrapper<T> where T: struct
	{
		public T TestEnum { get; set; }
	}
}
