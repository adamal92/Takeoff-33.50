using System.Text.Json;

namespace SparkySparky.Models;

public static class GeoJsonGeometryParser
{
    public static bool TryParsePoint(
        GeoJsonGeometry geometry,
        out double[] coordinates)
    {
        coordinates = [];

        if (!string.Equals(geometry.Type, "Point", StringComparison.OrdinalIgnoreCase)
            || geometry.Coordinates.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        var values = geometry.Coordinates.EnumerateArray().ToArray();
        if (values.Length != 2
            || !values[0].TryGetDouble(out var longitude)
            || !values[1].TryGetDouble(out var latitude))
        {
            return false;
        }

        coordinates = [longitude, latitude];
        return true;
    }

    public static bool TryParsePolygon(
        GeoJsonGeometry geometry,
        out double[][][] coordinates)
    {
        coordinates = [];

        if (!string.Equals(geometry.Type, "Polygon", StringComparison.OrdinalIgnoreCase)
            || geometry.Coordinates.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        var rings = geometry.Coordinates.EnumerateArray().ToArray();
        if (rings.Length == 0)
        {
            return false;
        }

        var parsedRings = new double[rings.Length][][];
        for (var ringIndex = 0; ringIndex < rings.Length; ringIndex++)
        {
            if (!TryParseRing(rings[ringIndex], out var ring))
            {
                return false;
            }

            parsedRings[ringIndex] = ring;
        }

        coordinates = parsedRings;
        return true;
    }

    private static bool TryParseRing(
        JsonElement element,
        out double[][] ring)
    {
        ring = [];

        if (element.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        var positions = element.EnumerateArray().ToArray();
        if (positions.Length < 4)
        {
            return false;
        }

        var parsedPositions = new double[positions.Length][];
        for (var positionIndex = 0; positionIndex < positions.Length; positionIndex++)
        {
            if (!TryParsePosition(positions[positionIndex], out var position))
            {
                return false;
            }

            parsedPositions[positionIndex] = position;
        }

        var first = parsedPositions[0];
        var last = parsedPositions[^1];
        if (first[0] != last[0] || first[1] != last[1])
        {
            return false;
        }

        ring = parsedPositions;
        return true;
    }

    private static bool TryParsePosition(
        JsonElement element,
        out double[] position)
    {
        position = [];

        if (element.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        var values = element.EnumerateArray().ToArray();
        if (values.Length != 2
            || !values[0].TryGetDouble(out var longitude)
            || !values[1].TryGetDouble(out var latitude))
        {
            return false;
        }

        position = [longitude, latitude];
        return true;
    }
}
