using System.Text.Json;
using SparkySparky.Models;

namespace SparkySparky.Tests;

public sealed class GeoJsonGeometryParserTests
{
    [Fact]
    public void TryParsePoint_WithValidPoint_ReturnsCoordinates()
    {
        var geometry = ParseGeometry("{\"type\":\"Point\",\"coordinates\":[12.5,-8.25]}");

        var parsed = GeoJsonGeometryParser.TryParsePoint(
            geometry,
            out var coordinates);

        Assert.True(parsed);
        Assert.Equal([12.5, -8.25], coordinates);
    }

    [Theory]
    [InlineData("LineString", "[ [1, 2], [3, 4] ]")]
    [InlineData("Point", "[1]")]
    [InlineData("Point", "[1, \"north\"]")]
    public void TryParsePoint_WithInvalidGeometry_ReturnsFalse(
        string type,
        string coordinatesJson)
    {
        var geometry = ParseGeometry(
            $"{{\"type\":\"{type}\",\"coordinates\":{coordinatesJson}}}");

        var parsed = GeoJsonGeometryParser.TryParsePoint(
            geometry,
            out var coordinates);

        Assert.False(parsed);
        Assert.Empty(coordinates);
    }

    [Fact]
    public void TryParsePolygon_WithClosedValidRing_ReturnsCoordinates()
    {
        var geometry = ParseGeometry(
            "{\"type\":\"Polygon\",\"coordinates\":[[[0,0],[4,0],[4,4],[0,0]]]}" );

        var parsed = GeoJsonGeometryParser.TryParsePolygon(
            geometry,
            out var coordinates);

        Assert.True(parsed);
        Assert.Single(coordinates);
        Assert.Equal(4, coordinates[0].Length);
        Assert.Equal([0, 0], coordinates[0][0]);
        Assert.Equal([0, 0], coordinates[0][^1]);
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("[[[0,0],[4,0],[4,4]]]")]
    [InlineData("[[[0,0],[4,0],[4,4],[1,1]]]")]
    public void TryParsePolygon_WithInvalidRing_ReturnsFalse(string coordinatesJson)
    {
        var geometry = ParseGeometry(
            $"{{\"type\":\"Polygon\",\"coordinates\":{coordinatesJson}}}");

        var parsed = GeoJsonGeometryParser.TryParsePolygon(
            geometry,
            out var coordinates);

        Assert.False(parsed);
        Assert.Empty(coordinates);
    }

    private static GeoJsonGeometry ParseGeometry(string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        return new GeoJsonGeometry
        {
            Type = root.GetProperty("type").GetString()!,
            Coordinates = root.GetProperty("coordinates").Clone()
        };
    }
}
