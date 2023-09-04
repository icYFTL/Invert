using InvertApi.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InvertApi.Converters;

public class CommandTypeConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(BaseCommand).IsAssignableFrom(objectType);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        var jsonObject = JObject.Load(reader);
        var typeToken = jsonObject["Type"];

        if (typeToken != null)
        {
            var typeName = typeToken.Value<string>();
            var targetType = Type.GetType($"InvertApi.Commands.{typeName}");

            if (targetType != null && typeof(BaseCommand).IsAssignableFrom(targetType))
            {
                return jsonObject.ToObject(targetType, serializer);
            }
        }

        return jsonObject.ToObject(typeof(BaseCommand), serializer);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}