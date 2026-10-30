using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace SparkySparky.Models;

public sealed class JsonElementBsonSerializer : SerializerBase<JsonElement>
{
    public override JsonElement Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        var value = BsonValueSerializer.Instance.Deserialize(context, args);
        using var document = JsonDocument.Parse(value.ToJson());
        return document.RootElement.Clone();
    }

    public override void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        JsonElement value)
    {
        WriteElement(context.Writer, value);
    }

    private static void WriteElement(IBsonWriter writer, JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    WriteElement(writer, item);
                }

                writer.WriteEndArray();
                break;
            case JsonValueKind.Object:
                writer.WriteStartDocument();
                foreach (var property in element.EnumerateObject())
                {
                    writer.WriteName(property.Name);
                    WriteElement(writer, property.Value);
                }

                writer.WriteEndDocument();
                break;
            case JsonValueKind.Number:
                if (element.TryGetInt64(out var integer))
                {
                    writer.WriteInt64(integer);
                }
                else
                {
                    writer.WriteDouble(element.GetDouble());
                }

                break;
            case JsonValueKind.String:
                writer.WriteString(element.GetString()!);
                break;
            case JsonValueKind.True:
                writer.WriteBoolean(true);
                break;
            case JsonValueKind.False:
                writer.WriteBoolean(false);
                break;
            case JsonValueKind.Null:
                writer.WriteNull();
                break;
            default:
                throw new BsonSerializationException(
                    $"Unsupported JSON value kind: {element.ValueKind}.");
        }
    }
}
