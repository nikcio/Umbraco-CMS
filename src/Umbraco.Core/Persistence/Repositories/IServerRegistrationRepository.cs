using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IServerRegistrationRepository : IReadWriteQueryRepository<int, IServerRegistration>
{
    void DeactiveStaleServers(TimeSpan staleTimeout);

    Task DeactiveStaleServersAsync(TimeSpan staleTimeout, CancellationToken? cancellationToken = null);

    void ClearCache();

    Task ClearCacheAsync(CancellationToken? cancellationToken = null);
}
