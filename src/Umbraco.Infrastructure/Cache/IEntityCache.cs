using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models.Entities;

namespace Umbraco.Cms.Infrastructure.Cache;

/// <summary>
/// Represents a cache for an entity.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEntityCache<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Gets an entity from the cache or by <paramref name="performGet"/>.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="performGet">How to get an entity if none exists in the cache.</param>
    /// <returns>The entity with the specified identifier, if it exits, else null.</returns>
    TEntity? GetEntity(string id, Func<string, TEntity?>? performGet = null);

    /// <summary>
    /// Gets an entity from the cache or by <paramref name="performGet"/>.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="performGet">How to get an entity if none exists in the cache.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The entity with the specified identifier, if it exits, else null.</returns>
    Task<TEntity?> GetEntityAsync(string id, Func<string, TEntity?>? performGet = null, CancellationToken cancellationToken = default) => throw new NotImplementedException(); //TODO: Implement

    /// <summary>
    /// Gets entities from the cache or by <paramref name="performGetEntities"/>.
    /// </summary>
    /// <param name="ids">The identifiers.</param>
    /// <param name="performGetEntities">How to get entities if they are not in the cache.</param>
    /// <returns>The entities with the specified identifiers.</returns>
    TEntity[] GetEntities(string[] ids, Func<string[], IEnumerable<TEntity>>? performGetEntities = null);

    /// <summary>
    /// Gets entities from the cache or by <paramref name="performEntities"/>.
    /// </summary>
    /// <param name="ids">The identifiers.</param>
    /// <param name="performEntities">How to get entities if they are not in the cache.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The entities with the specified identifiers.</returns>
    Task<TEntity[]> GetEntitiesAsync(string[] ids, Func<string[], IEnumerable<TEntity>>? performEntities = null, CancellationToken cancellationToken = default) => throw new NotImplementedException(); //TODO: Implement

    /// <summary>
    /// Sets an entity in the cache.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="options">The cache options.</param>
    void Set(TEntity entity, EntityCacheOptions? options = null);

    /// <summary>
    /// Sets an entity in the cache.
    /// </summary>
    /// <param name="key">The key for the item.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="options">The cache options.</param>
    void Set(string key, TEntity entity, EntityCacheOptions? options = null);

    /// <summary>
    /// Sets an entity in the cache.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="options">The cache options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task.</returns>
    Task SetAsync(TEntity entity, EntityCacheOptions options, CancellationToken cancellationToken = default) => throw new NotImplementedException(); //TODO: Implement

    /// <summary>
    /// Sets an entity in the cache.
    /// </summary>
    /// <param name="key">The key for the item.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="options">The cache options.</param>
    Task SetAsync(string key, TEntity entity, EntityCacheOptions? options = null) => throw new NotImplementedException(); //TODO: Implement

    /// <summary>
    /// Removes an entity from the cache.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Remove(TEntity entity);

    /// <summary>
    /// Removes an entity from the cache.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task.</returns>
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default) => throw new NotImplementedException(); //TODO: Implement

    /// <summary>
    /// Refreshes the entity in the cache based on its identifier, resetting its sliding expiration timeout (if any).
    /// </summary>
    /// <param name="id">The identifier.</param>
    void RefreshEntity(string id);

    /// <summary>
    /// Refreshes the entity in the cache based on its identifier, resetting its sliding expiration timeout (if any).
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellation">A cancellation token.</param>
    Task RefreshEntityAsync(string id, CancellationToken cancellation = default) => throw new NotImplementedException(); //TODO: Implement
}
