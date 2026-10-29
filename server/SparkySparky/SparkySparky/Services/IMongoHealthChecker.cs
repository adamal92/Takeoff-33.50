namespace SparkySparky.Services;

public interface IMongoHealthChecker
{
    Task<bool> IsHealthyAsync(CancellationToken cancellationToken);
}
