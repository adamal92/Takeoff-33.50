using MapApp.Server.Models;

namespace MapApp.Server.Services;

public interface IPolygonService
{
    Task<IReadOnlyList<PolygonEntity>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<PolygonEntity> CreateAsync(
        PolygonEntity polygon,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        string id,
        CancellationToken cancellationToken);
}