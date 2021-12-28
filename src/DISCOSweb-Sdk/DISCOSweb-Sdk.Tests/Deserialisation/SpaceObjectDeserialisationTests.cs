using System;
using System.Text.Json;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Models;
using DISCOSweb_Sdk.Models.SpaceObjects;
//using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Deserialisation;

public class SpaceObjectDeserialisationTests
{
	private readonly string _testJson = "{\n            \"type\": \"object\",\n            \"attributes\": {\n                \"shape\": \"Cyl + 1 Nozzle\",\n                \"xSectMin\": 0.731382404709789,\n                \"satno\": 44629,\n                \"depth\": 1.34,\n                \"objectClass\": \"Rocket Body\",\n                \"cosparId\": \"2019-068B\",\n                \"length\": 0.965,\n                \"height\": 1.34,\n                \"mass\": 202.0,\n                \"xSectMax\": 1.48560689010218,\n                \"vimpelId\": null,\n                \"xSectAvg\": 1.38128956744413,\n                \"name\": \"ORION 38 (Pegasus XL)\"\n            },\n            \"relationships\": {\n                \"states\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/states\",\n                        \"related\": \"/api/objects/61272/states\"\n                    }\n                },\n                \"initialOrbits\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/initial-orbits\",\n                        \"related\": \"/api/objects/61272/initial-orbits\"\n                    }\n                },\n                \"destinationOrbits\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/destination-orbits\",\n                        \"related\": \"/api/objects/61272/destination-orbits\"\n                    }\n                },\n                \"operators\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/operators\",\n                        \"related\": \"/api/objects/61272/operators\"\n                    }\n                },\n                \"launch\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/launch\",\n                        \"related\": \"/api/objects/61272/launch\"\n                    }\n                },\n                \"reentry\": {\n                    \"links\": {\n                        \"self\": \"/api/objects/61272/relationships/reentry\",\n                        \"related\": \"/api/objects/61272/reentry\"\n                    }\n                }\n            },\n            \"id\": \"61272\",\n            \"links\": {\n                \"self\": \"/api/objects/61272\"\n            }\n        }";
	private readonly DiscosResponse<DiscosObject> _testExpected = new()
																  {
																	  Type = "object",
																	  Attributes = new()
																				   {
																					   Shape = "Cyl + 1 Nozzle",
																					   CrossSection = new()
																									  {
																										  Minimum = 0.731382404709789,
																										  Maximum = 1.48560689010218,
																										  Average = 1.38128956744413
																									  },
																					   SatNo = 44629,
																					   Dimensions = new()
																									{
																										Depth = 1.34,
																										Length = 0.965,
																										Height = 1.34
																									},
																					   ObjectClass = ObjectClass.RocketBody,
																					   CosparId = "2019-068B",
																					   Mass = 202.0f,
																					   VimpelId = null,
																					   Name = "ORION 38 (Pegasus XL)"
																				   }
																  };

	[Fact]
	public void CanDeserialiseTestObject()
	{
		DiscosResponse<DiscosObject>? res = JsonSerializer.Deserialize<DiscosResponse<DiscosObject>>(_testJson);
		;
		// res.ShouldNotBeNull();
		// res.ShouldBeEquivalentTo(_testExpected);
	}
}
