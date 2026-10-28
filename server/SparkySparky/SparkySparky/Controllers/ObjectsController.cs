using MapApp.Server.DTOs;
using MapApp.Server.Models;
using MapApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapApp.Server.Controllers;

[ApiController]
[Route("api/objects")]
public sealed class ObjectsController : ControllerBase
{
    private readonly IMapObjectService mapObjectService;

    public ObjectsController(IMapObjectService mapObjectService)
    {
        this.mapObjectService = mapObjectService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MapObjectEntity>>> GetAll(
        CancellationToken cancellationToken)
    {
        var objects = await mapObjectService.GetAllAsync(cancellationToken);
        return Ok(objects);
    }

    [HttpPost]
    public async Task<ActionResult<MapObjectEntity>> Create(
        CreateMapObjectRequest request,
        CancellationToken cancellationToken)
    {
        if (!IsValidPoint(request.Geometry))
        {
            return BadRequest(
                "An object geometry must be a GeoJSON Point with [longitude, latitude].");
        }

        var mapObject = new MapObjectEntity
        {
            Name = request.Name,
            SymbolType = request.SymbolType,
            Geometry = request.Geometry,
            CreatedAt = DateTime.UtcNow
        };

        var created = await mapObjectService.CreateAsync(
            mapObject,
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
        var deleted = await mapObjectService.DeleteAsync(
            id,
            cancellationToken);

        return deleted ? NoContent() : NotFound();
    }

    private static bool IsValidPoint(GeoJsonGeometry geometry)
    {
        return string.Equals(
                   geometry.Type,
                   "Point",
                   StringComparison.OrdinalIgnoreCase)
               && geometry.Coordinates.Length == 1
               && geometry.Coordinates[0].Length == 2;
    }
}