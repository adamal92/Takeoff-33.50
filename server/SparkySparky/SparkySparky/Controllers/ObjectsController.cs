using SparkySparky.DTOs;
using SparkySparky.Models;
using SparkySparky.Services;
using Microsoft.AspNetCore.Mvc;

namespace SparkySparky.Controllers;

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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id,
        CreateMapObjectRequest request,
        CancellationToken cancellationToken)
    {
        if (!IsValidPoint(request.Geometry))
        {
            return BadRequest("An object must have valid Point coordinates.");
        }

        var updated = await mapObjectService.UpdateAsync(
            new MapObjectEntity
            {
                Id = id,
                Name = request.Name,
                SymbolType = request.SymbolType,
                Geometry = request.Geometry,
                CreatedAt = DateTime.UtcNow
            },
            cancellationToken);

        return updated ? NoContent() : NotFound();
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
        return GeoJsonGeometryParser.TryParsePoint(
            geometry,
            out _);
    }
}