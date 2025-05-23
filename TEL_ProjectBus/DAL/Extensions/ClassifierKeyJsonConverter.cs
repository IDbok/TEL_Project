namespace TEL_ProjectBus.DAL.Extensions;

using System.Text.Json;
using System.Text.Json.Serialization;
using TEL_ProjectBus.DAL.Entities.Common;

public class ClassifierKeyJsonConverter : JsonConverter<ClassifierKey>
{
	public override ClassifierKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// Assumes the JSON value is an integer
		if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int value))
		{
			return new ClassifierKey(value);
		}
		throw new JsonException($"Unable to convert JSON value to {nameof(ClassifierKey)}.");
	}

	public override void Write(Utf8JsonWriter writer, ClassifierKey value, JsonSerializerOptions options)
	{
		writer.WriteNumberValue(value.Value);
	}
}

