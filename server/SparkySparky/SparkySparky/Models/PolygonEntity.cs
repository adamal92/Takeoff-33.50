using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MapApp.Server.Models;

public sealed class PolygonEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public required string Name { get; set; }

    public required GeoJsonGeometry Geometry { get; set; }

    public DateTime CreatedAt { get; set; }
}