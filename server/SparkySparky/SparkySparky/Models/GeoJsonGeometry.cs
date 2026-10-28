namespace SparkySparky.Models;

public sealed class GeoJsonGeometry
{
    public required string Type { get; init; }

    // GeoJSON coordinate order is [longitude, latitude].
    public required double[][] Coordinates { get; init; }
}