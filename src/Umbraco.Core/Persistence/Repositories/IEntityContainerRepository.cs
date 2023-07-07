using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IEntityContainerRepository : IReadRepository<int, EntityContainer>, IWriteRepository<EntityContainer>
{
    EntityContainer? Get(Guid id);

    Task<EntityContainer?> GetAsync(Guid id, CancellationToken? cancellationToken = null);

    IEnumerable<EntityContainer> Get(string name, int level);

    Task<IEnumerable<EntityContainer>> GetAsync(string name, int level, CancellationToken? cancellationToken = null);
}
