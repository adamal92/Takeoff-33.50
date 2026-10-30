using SparkySparky.Models;

namespace SparkySparky.Services;

public interface IMapObjectService
{
    Task<IReadOnlyList<MapObjectEntity>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<MapObjectEntity> CreateAsync(
        MapObjectEntity mapObject,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        MapObjectEntity mapObject,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        string id,
        CancellationToken cancellationToken);
}