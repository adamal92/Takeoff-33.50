using System.ComponentModel.DataAnnotations;
using MapApp.Server.Models;

namespace MapApp.Server.DTOs;

public sealed class CreateMapObjectRequest
{
    [Required]
    public required string Name { get; init; }

    [Required]
    public required string SymbolType { get; init; }

    [Required]
    public required GeoJsonGeometry Geometry { get; init; }
}