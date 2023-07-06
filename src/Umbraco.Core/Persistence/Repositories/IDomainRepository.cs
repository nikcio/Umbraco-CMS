using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IDomainRepository : IReadWriteQueryRepository<int, IDomain>
{
    IDomain? GetByName(string domainName);

    Task<IDomain?> GetByNameAsync(string domainName, CancellationToken? cancellationToken = null);

    bool Exists(string domainName);

    Task<bool> ExistsAsync(string domainName, CancellationToken? cancellationToken = null);

    IEnumerable<IDomain> GetAll(bool includeWildcards);

    Task<IEnumerable<IDomain>> GetAllAsync(bool includeWildcards, CancellationToken? cancellationToken = null);

    IEnumerable<IDomain> GetAssignedDomains(int contentId, bool includeWildcards);

    Task<IEnumerable<IDomain>> GetAssignedDomainsAsync(int contentId, bool includeWildcards, CancellationToken? cancellationToken = null);
}
