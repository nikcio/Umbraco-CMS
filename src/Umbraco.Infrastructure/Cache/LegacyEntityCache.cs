using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Entities;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Umbraco.Cms.Infrastructure.Cache;

/// <summary>
/// Represents a cache for entities.
/// </summary>
/// <remarks>
/// This uses the legacy cache implementations <see cref="DefaultRepositoryCachePolicy{TEntity, TId}"/>, <see cref="FullDataSetRepositoryCachePolicy{TEntity, TId}"/>.
/// </remarks>
/// <typeparam name="TEntity"></typeparam>
internal class LegacyEntityCache<TEntity> : IEntityCache<TEntity>
    where TEntity : class, IEntity
{
    private static readonly List<Type> _fullDataSetRepositoryCacheTypes = new()
    {
        typeof(IContent),
        typeof(ILanguage),
        typeof(IContentType),
        typeof(IDomain),
        typeof(ILogViewerQuery),
        typeof(IMediaType),
        typeof(IMemberType),
        typeof(PublicAccessEntry),
        typeof(IRelationType),
        typeof(ITemplate),
        typeof(AuditItem),
    };

    private static readonly List<Type> _singleItemsOnlyRepositoryCacheTypes = new()
    {
        typeof(IDictionaryItem),
    };

    private readonly IRepositoryCachePolicy<TEntity, int> _cachePolicy;

    /// <summary>
    /// Initializes a new instance of the <see cref="LegacyEntityCache{TEntity}"/> class.
    /// </summary>
    /// <param name="appPolicyCache"></param>
    /// <param name="scopeProvider"></param>
    public LegacyEntityCache(IAppPolicyCache appPolicyCache, IScopeProvider scopeProvider)
    {
        var scopeAccessor = new BridgeScopeAccessor(scopeProvider);

        if (_fullDataSetRepositoryCacheTypes.Any(type => type == typeof(TEntity) || type.IsAssignableFrom(typeof(TEntity))))
        {
            _cachePolicy = new FullDataSetRepositoryCachePolicy<TEntity, int>(appPolicyCache, scopeAccessor, item => item.Id, false);
        }
        else if (_singleItemsOnlyRepositoryCacheTypes.Any(type => type == typeof(TEntity) || type.IsAssignableFrom(typeof(TEntity))))
        {
            _cachePolicy = new SingleItemsOnlyRepositoryCachePolicy<TEntity, int>(appPolicyCache, scopeAccessor, new RepositoryCachePolicyOptions());
        }

        _cachePolicy = new DefaultRepositoryCachePolicy<TEntity, int>(appPolicyCache, scopeAccessor, new RepositoryCachePolicyOptions());
    }

    /// <inheritdoc/>
    public TEntity[] GetEntities(int[] ids, Func<int[], IEnumerable<TEntity>>? performGetEntities = null)
    {
        if (performGetEntities == null)
        {
            return _cachePolicy.GetAll(ids, _ => Enumerable.Empty<TEntity>());
        }

        return _cachePolicy.GetAll(ids, ids => performGetEntities(ids ?? Array.Empty<int>()));
    }

    /// <inheritdoc/>
    public TEntity? GetEntity(int id, Func<int, TEntity?>? performGet = null)
    {
        if (performGet == null)
        {
            return _cachePolicy.Get(id, _ => null, _ => null);
        }

        return _cachePolicy.Get(id, performGet, _ => null);
    }

    /// <inheritdoc/>
    public void RefreshEntity(int id) => throw new NotImplementedException("This implementation doesn't support refreshing entities.");

    /// <inheritdoc/>
    public void Remove(TEntity entity)
    {
        _cachePolicy.Delete(entity, entity =>
        {
            return; // Delete should only affect the cache, not the database.
        });
    }

    /// <inheritdoc/>
    public void Set(TEntity entity, EntityCacheOptions? options = null)
    {
        if (GetEntity(entity.Id) != null)
        {
            _cachePolicy.Update(entity, entity =>
            {
                return; // Update should only affect the cache, not the database.
            });
            return;
        }

        _cachePolicy.Create(entity, entity =>
        {
            return; // Create should only affect the cache, not the database.
        });
    }

    internal class BridgeScopeAccessor : IScopeAccessor
    {
        public BridgeScopeAccessor(IScopeProvider scopeProvider)
        {
            AmbientScope = scopeProvider.CreateScope();
        }

        public IScope? AmbientScope { get; }
    }
}
