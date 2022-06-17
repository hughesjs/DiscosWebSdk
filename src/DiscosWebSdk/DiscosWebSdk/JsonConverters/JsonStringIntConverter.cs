using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscosWebSdk.JsonConverters;

public class JsonStringIntConverter : JsonConverter<int>
{
	public override int Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String)
		{
			return reader.GetInt32();
		}

		ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
		if (Utf8Parser.TryParse(span, out int number, out int bytesConsumed) && span.Length == bytesConsumed)
		{
			return number;
		}

		return int.TryParse(reader.GetString(), out number) ? number : reader.GetInt32();

	}

	public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
}
