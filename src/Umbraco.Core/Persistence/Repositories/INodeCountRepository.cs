namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface INodeCountRepository
{
    int GetNodeCount(Guid nodeType);

    Task<int> GetNodeCountAsync(Guid nodeType, CancellationToken? cancellationToken = null);

    int GetMediaCount();

    Task<int> GetMediaCountAsync(CancellationToken? cancellationToken = null);
}
