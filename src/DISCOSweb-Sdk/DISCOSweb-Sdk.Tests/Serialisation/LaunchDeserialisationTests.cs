using System;
using System.Text.Json;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Models;
using FluentAssertions;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Deserialisation;

public class LaunchDeserialisationTests
{
	private const string TestJson = "{\r\n            \"type\": \"launch\",\r\n            \"attributes\": {\r\n                \"flightNo\": \"H-IIA-43\",\r\n                \"epoch\": \"2020-11-29T07:26:24+00:00\",\r\n                \"cosparLaunchNo\": \"2020-089\",\r\n                \"failure\": false\r\n            },\r\n            \"relationships\": {\r\n                \"site\": {\r\n                    \"links\": {\r\n                        \"self\": \"/api/launches/6389/relationships/site\",\r\n                        \"related\": \"/api/launches/6389/site\"\r\n                    }\r\n                },\r\n                \"objects\": {\r\n                    \"links\": {\r\n                        \"self\": \"/api/launches/6389/relationships/objects\",\r\n                        \"related\": \"/api/launches/6389/objects\"\r\n                    }\r\n                },\r\n                \"vehicle\": {\r\n                    \"links\": {\r\n                        \"self\": \"/api/launches/6389/relationships/vehicle\",\r\n                        \"related\": \"/api/launches/6389/vehicle\"\r\n                    }\r\n                },\r\n                \"entities\": {\r\n                    \"links\": {\r\n                        \"self\": \"/api/launches/6389/relationships/entities\",\r\n                        \"related\": \"/api/launches/6389/entities\"\r\n                    }\r\n                }\r\n            },\r\n            \"id\": \"6389\",\r\n            \"links\": {\r\n                \"self\": \"/api/launches/6389\"\r\n            }}";
	private readonly DiscosResponse<Launch> _testExpected = new()
																  {
																	  Id = 6389,
																	  Type = ResponseType.Launch,
																	  Attributes = new()
																				   {
																					  FlightNo = "H-IIA-43",
																					  Epoch = DateTime.Parse("2020-11-29T07:26:24+00:00"),
																					  CosparLaunchNo = "2020-089",
																					  Failure = false
																				   }
																  };

	[Fact]
	public void CanDeserialiseTestObject()
	{
		DiscosResponse<Launch>? res = JsonSerializer.Deserialize<DiscosResponse<Launch>>(TestJson);

		res.Should().NotBeNull();
		res.Should().BeEquivalentTo(_testExpected);
	}
}
