using MapApp.Server.DTOs;
using MapApp.Server.Models;
using MapApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapApp.Server.Controllers;

[ApiController]
[Route("api/polygons")]
public sealed class PolygonsController : ControllerBase
{
    private readonly IPolygonService polygonService;

    public PolygonsController(IPolygonService polygonService)
    {
        this.polygonService = polygonService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PolygonEntity>>> GetAll(
        CancellationToken cancellationToken)
    {
        var polygons = await polygonService.GetAllAsync(cancellationToken);
        return Ok(polygons);
    }

    [HttpPost]
    public async Task<ActionResult<PolygonEntity>> Create(
        CreatePolygonRequest request,
        CancellationToken cancellationToken)
    {
        if (!IsValidPolygon(request.Geometry))
        {
            return BadRequest(
                "A polygon must contain at least four coordinates and be closed.");
        }

        var polygon = new PolygonEntity
        {
            Name = request.Name,
            Geometry = request.Geometry,
            CreatedAt = DateTime.UtcNow
        };

        var created = await polygonService.CreateAsync(
            polygon,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetAll),
            new { id = created.Id },
            created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        string id,
        CancellationToken cancellationToken)
    {
        var deleted = await polygonService.DeleteAsync(
            id,
            cancellationToken);

        return deleted ? NoContent() : NotFound();
    }

    private static bool IsValidPolygon(GeoJsonGeometry geometry)
    {
        if (!string.Equals(geometry.Type, "Polygon", StringComparison.OrdinalIgnoreCase)
            || geometry.Coordinates.Length == 0)
        {
            return false;
        }

        var ring = geometry.Coordinates[0];

        return ring.Length >= 4
            && ring[0].Length == 2
            && ring[^1].Length == 2
            && ring[0][0] == ring[^1][0]
            && ring[0][1] == ring[^1][1];
    }
}