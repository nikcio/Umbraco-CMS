using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface ITemplateRepository : IReadWriteQueryRepository<int, ITemplate>, IFileRepository
{
    ITemplate? Get(string? alias);

    Task<ITemplate?> GetAsync(string? alias, CancellationToken? cancellationToken = null);

    IEnumerable<ITemplate> GetAll(params string[] aliases);

    Task<IEnumerable<ITemplate>> GetAllAsync(CancellationToken? cancellationToken = null, params string[] aliases);

    IEnumerable<ITemplate> GetChildren(int masterTemplateId);

    Task<IEnumerable<ITemplate>> GetChildrenAsync(int masterTemplateId, CancellationToken? cancellationToken = null);

    IEnumerable<ITemplate> GetDescendants(int masterTemplateId);

    Task<IEnumerable<ITemplate>> GetDescendantsAsync(int masterTemplateId, CancellationToken? cancellationToken = null);
}
