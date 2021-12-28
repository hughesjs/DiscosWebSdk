using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Attributes;
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
		
	    JsonSerializerOptions options = new();
		options.Converters.Add(new JsonStringEnumConverterWithAttributeSupport());
		
		var res = JsonSerializer.Deserialize(json, constructed, options);

		constructed.GetProperty("TestEnum").GetValue(res).ShouldBe(expected);
	}


	private class TestDataGenerator: IEnumerable<object[]>
	{

		public IEnumerator<object[]> GetEnumerator()
		{
			var allEnums = typeof(RecordType).Assembly.GetTypes().Where(t => t.IsEnum);
			var testEnums = allEnums.Where(e => e.GetCustomAttributes().Any(a => a is EnumWithCustomSerialiser));
			var enumMemberInfo = testEnums.SelectMany(e => e.GetFields(BindingFlags.Static | BindingFlags.Public));
			IEnumerable<object[]> enumsWithNames = enumMemberInfo.Select(f =>
																		 {
																			return new[]
																					{
																						f.GetValue(null)!,
																						((EnumMemberAttribute)f.GetCustomAttributes().First(a => a is EnumMemberAttribute)).Value!
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
