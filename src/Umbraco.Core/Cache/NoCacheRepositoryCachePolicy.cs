using Umbraco.Cms.Core.Models.Entities;

namespace Umbraco.Cms.Core.Cache;

public class NoCacheRepositoryCachePolicy<TEntity, TId> : IRepositoryCachePolicy<TEntity, TId>
    where TEntity : class, IEntity
{
    private NoCacheRepositoryCachePolicy()
    {
    }

    public static NoCacheRepositoryCachePolicy<TEntity, TId> Instance { get; } = new();

    public TEntity? Get(TId? id, Func<TId?, TEntity?> performGet, Func<TId[]?, IEnumerable<TEntity>?> performGetAll) =>
        performGet(id);

    public async Task<TEntity?> GetAsync(TId? id, Func<TId?, CancellationToken?, Task<TEntity?>> performGetAsync, Func<CancellationToken?, TId[]?, Task<IEnumerable<TEntity>>> performGetAllAsync, CancellationToken? cancellationToken = null) => await performGetAsync(id, cancellationToken);

    public TEntity? GetCached(TId id) => null;

    public bool Exists(TId id, Func<TId, bool> performExists, Func<TId[], IEnumerable<TEntity>?> performGetAll) =>
        performExists(id);

    public async Task<bool> ExistsAsync(TId id, Func<TId, CancellationToken?, Task<bool>> performExistsAsync, Func<CancellationToken?, TId[], Task<IEnumerable<TEntity>>> performGetAllAsync, CancellationToken? cancellationToken = null) => await performExistsAsync(id, cancellationToken);

    public void Create(TEntity entity, Action<TEntity> persistNew) => persistNew(entity);

    public async Task CreateAsync(TEntity entity, Func<TEntity, CancellationToken?, Task> persistNewAsync, CancellationToken? cancellationToken = null) => await persistNewAsync(entity, cancellationToken);

    public void Update(TEntity entity, Action<TEntity> persistUpdated) => persistUpdated(entity);

    public async Task UpdateAsync(TEntity entity, Func<TEntity, CancellationToken?, Task> persistUpdatedAsync, CancellationToken? cancellationToken = null) => await persistUpdatedAsync(entity, cancellationToken);

    public void Delete(TEntity entity, Action<TEntity> persistDeleted) => persistDeleted(entity);

    public async Task DeleteAsync(TEntity entity, Func<TEntity, CancellationToken?, Task> persistDeletedAsync, CancellationToken? cancellationToken = null) =>  await persistDeletedAsync(entity, cancellationToken);

    public TEntity[] GetAll(TId[]? ids, Func<TId[]?, IEnumerable<TEntity>?> performGetAll) =>
        performGetAll(ids)?.ToArray() ?? Array.Empty<TEntity>();

    public async Task<TEntity[]> GetAllAsync(TId[]? ids, Func<CancellationToken?, TId[]?, Task<IEnumerable<TEntity>>> performGetAllAsync, CancellationToken? cancellationToken = null) => (await performGetAllAsync(cancellationToken, ids))?.ToArray() ?? Array.Empty<TEntity>();

    public void ClearAll()
    {
    }
}
