using SparkySparky.DTOs;
using SparkySparky.Models;
using SparkySparky.Services;
using Microsoft.AspNetCore.Mvc;

namespace SparkySparky.Controllers;

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
        return GeoJsonGeometryParser.TryParsePolygon(
            geometry,
            out _);
    }
}