using BadgermoleGeoAtlas;
using NetTopologySuite.Geometries;
using SharpKml.Engine;

namespace BadgermoleGeoAtlas
{
    /// <summary>
    /// Represents generic geographic data.
    /// </summary>

    // 1. Define a common contract
    public interface IGeography
    {
        string FormatName { get; }
    }

    // 2. Create strongly-typed wrappers for each format
    public record GeoJsonGeometry(string Json) : IGeography
    {
        public string FormatName => "GeoJSON";
    }

    public record KmlGeometry(string Xml) : IGeography
    {
        public string FormatName => "KML";
    }

    public record XyzGeometry(double X, double Y, double? Z = null) : IGeography
    {
        public string FormatName => "XYZ";
    }

    // 3. Use it in your classes or services
    public class MapFeature
    {
        public required string Id { get; set; }
        public required IGeography Location { get; set; }
    }

    public class SpatialEntity<TGeography> where TGeography : IGeography
    {
        public required string Name { get; set; }
        public required TGeography Geometry { get; set; }
    }

// Usage
//var pointEntity = new SpatialEntity<XyzGeometry>
//{
//    Name = "Origin",
//    Geometry = new XyzGeometry(0, 0, 0)
//};

    }

//string output = feature.Location switch
//{
//    GeoJsonGeometry g => $"Parsing JSON: {g.Json}",
//    KmlGeometry k => $"Parsing XML: {k.Xml}",
//    XyzGeometry(var x, var y, var z) => $"Coords: {x}, {y}, {z}",
//    _ => "Unknown format"
//};
