using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace OP // Asegúrate de que coincida con tu namespace principal
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override Vector3 Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            float[] data = JsonSerializer.Deserialize<float[]>(ref reader)!;
            return new Vector3(data[0], data[1], data[2]);
        }

        public override void Write(
            Utf8JsonWriter writer,
            Vector3 value,
            JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
    }

    public static class JsonHelper
    {
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                Converters = { new Vector3Converter() },
                WriteIndented = true
            };
        }

        public static T Deserialize<T>(string json)
        {
            var options = GetJsonSerializerOptions();
            return JsonSerializer.Deserialize<T>(json, options)!;
        }
    }
}