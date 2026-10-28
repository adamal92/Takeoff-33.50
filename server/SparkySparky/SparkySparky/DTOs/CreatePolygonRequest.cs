using System.ComponentModel.DataAnnotations;
using SparkySparky.Models;

namespace SparkySparky.DTOs;

public sealed class CreatePolygonRequest
{
    [Required]
    public required string Name { get; init; }

    [Required]
    public required GeoJsonGeometry Geometry { get; init; }
}