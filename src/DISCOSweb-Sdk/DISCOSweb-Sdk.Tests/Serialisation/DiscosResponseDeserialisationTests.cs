using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoFixture;
using AutoFixture.Kernel;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Models;
using DISCOSweb_Sdk.Models.ResponseModels;
using FluentAssertions;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Serialisation;

public class DiscosResponseDeserialisationTests
{
	private readonly Fixture _fixture;
	
	public DiscosResponseDeserialisationTests()
	{
		_fixture = new();
	}
	
	[Theory]
	[ClassData(typeof(TestDataGenerator))]
	public void CanRoundTripAllResponseTypes(Type modelType)
	{
		Type constructedResponseType = typeof(DiscosResponse<>).MakeGenericType(modelType);
		
		object? testObject = new SpecimenContext(_fixture).Resolve(constructedResponseType);
		
		string test = JsonSerializer.Serialize(testObject);
		object? res = JsonSerializer.Deserialize(test, constructedResponseType);
		
		res.Should().NotBeNull();
		res.Should().BeEquivalentTo(testObject);
	}


	[Fact]
	public void SanityCheckCanDeserialiseExampleBlob()
	{
		const string testJson = "{\n            \"type\": \"object\",\n            \"attributes\": {\n                \"shape\": \"Cyl + 1 Nozzle\",\n                \"xSectMin\": 0.731382404709789,\n                \"satno\": 44629,\n                \"depth\": 1.34,\n                \"objectClass\": \"Rocket Body\",\n                \"cosparId\": \"2019-068B\",\n                \"length\": 0.965,\n                \"height\": 1.34,\n                \"mass\": 202.0,\n                \"xSectMax\": 1.48560689010218,\n                \"vimpelId\": null,\n                \"xSectAvg\": 1.38128956744413,\n                \"name\": \"ORION 38 (Pegasus XL)\"\n            },\n            \"relationships\": {\n                \"states\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/states\",\n                        \"related\": \"/api/objects/61272/states\"\n                    }\n                },\n                \"initialOrbits\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/initial-orbits\",\n                        \"related\": \"/api/objects/61272/initial-orbits\"\n                    }\n                },\n                \"destinationOrbits\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/destination-orbits\",\n                        \"related\": \"/api/objects/61272/destination-orbits\"\n                    }\n                },\n                \"operators\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/operators\",\n                        \"related\": \"/api/objects/61272/operators\"\n                    }\n                },\n                \"launch\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/launch\",\n                        \"related\": \"/api/objects/61272/launch\"\n                    }\n                },\n                \"reentry\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/reentry\",\n                        \"related\": \"/api/objects/61272/reentry\"\n                    }\n                }\n            },\n            \"id\": \"61272\",\n            \"links\": {\n                \"self\": \"/api/objects/61272\"\n            }\n        }";
	    DiscosResponse<DiscosObject> testExpected = new()
																  {
																	  Id = 61272,
																	  Type = ResponseType.DiscosObject,
																	  Attributes = new()
																				   {
																					   Shape = "Cyl + 1 Nozzle",
																					   CrossSectionMinimum = 0.731382404709789,
																					   CrossSectionMaximum = 1.48560689010218,
																					   CrossSectionAverage = 1.38128956744413,
																					   SatNo = 44629,
																					   Depth = 1.34f,
																					   Length = 0.965f,
																					   Height = 1.34f,
																					   ObjectClass = ObjectClass.RocketBody,
																					   CosparId = "2019-068B",
																					   Mass = 202.0f,
																					   VimpelId = null,
																					   Name = "ORION 38 (Pegasus XL)"
																				   }
																  };
		DiscosResponse<DiscosObject>? res = JsonSerializer.Deserialize<DiscosResponse<DiscosObject>>(testJson);
		
		res.Should().NotBeNull();
		res.Should().BeEquivalentTo(testExpected);
	}
	
	private class TestDataGenerator: IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator() => typeof(ResponseType).Assembly.GetTypes()
																			.Where(t => t != typeof(DiscosModelBase) && t.IsAssignableTo(typeof(DiscosModelBase)))
																			.Cast<object>()
																			.Select(t => new []{t})
																			.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}


