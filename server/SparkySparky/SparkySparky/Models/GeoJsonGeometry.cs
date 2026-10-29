using System.Text.Json;

namespace SparkySparky.Models;

public sealed class GeoJsonGeometry
{
    public required string Type { get; init; }

    // GeoJSON coordinate order is [longitude, latitude].
    public required JsonElement Coordinates { get; init; }
}