using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IDictionaryRepository : IReadWriteQueryRepository<int, IDictionaryItem>
{
    IDictionaryItem? Get(Guid uniqueId);

    Task<IDictionaryItem?> GetAsync(Guid uniqueId, CancellationToken? cancellationToken = null);

    IEnumerable<IDictionaryItem> GetMany(params Guid[] uniqueIds) => Array.Empty<IDictionaryItem>();

    Task<IEnumerable<IDictionaryItem>> GetManyAsync(CancellationToken? cancellationToken = null, params Guid[] uniqueIds) => Task.FromResult(Array.Empty<IDictionaryItem>().AsEnumerable());

    IEnumerable<IDictionaryItem> GetManyByKeys(params string[] keys) => Array.Empty<IDictionaryItem>();

    Task<IEnumerable<IDictionaryItem>> GetManyByKeysAsync(CancellationToken? cancellationToken = null, params string[] keys) => Task.FromResult(Array.Empty<IDictionaryItem>().AsEnumerable());

    IDictionaryItem? Get(string key);

    Task<IDictionaryItem?> GetAsync(string key, CancellationToken? cancellationToken = null);

    IEnumerable<IDictionaryItem> GetDictionaryItemDescendants(Guid? parentId);

    Task<IEnumerable<IDictionaryItem>> GetDictionaryItemDescendantsAsync(Guid? parentId, CancellationToken? cancellationToken = null);

    Dictionary<string, Guid> GetDictionaryItemKeyMap();

    Task<Dictionary<string, Guid>> GetDictionaryItemKeyMapAsync(CancellationToken? cancellationToken = null);
}
