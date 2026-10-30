using System.Text.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace SparkySparky.Models;

public sealed class GeoJsonGeometry
{
    [BsonElement("type")]
    public required string Type { get; init; }

    [BsonElement("coordinates")]
    [BsonSerializer(typeof(JsonElementBsonSerializer))]
    public required JsonElement Coordinates { get; init; }
}