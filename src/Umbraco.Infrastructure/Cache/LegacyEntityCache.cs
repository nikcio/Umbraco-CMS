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

        if (_fullDataSetRepositoryCacheTypes.Exists(type => type == typeof(TEntity) || type.IsAssignableFrom(typeof(TEntity))))
        {
            _cachePolicy = new FullDataSetRepositoryCachePolicy<TEntity, int>(appPolicyCache, scopeAccessor, item => item.Id, false);
        }
        else if (_singleItemsOnlyRepositoryCacheTypes.Exists(type => type == typeof(TEntity) || type.IsAssignableFrom(typeof(TEntity))))
        {
            _cachePolicy = new SingleItemsOnlyRepositoryCachePolicy<TEntity, int>(appPolicyCache, scopeAccessor, new RepositoryCachePolicyOptions());
        }

        _cachePolicy = new DefaultRepositoryCachePolicy<TEntity, int>(appPolicyCache, scopeAccessor, new RepositoryCachePolicyOptions());
    }

    /// <inheritdoc/>
    public TEntity[] GetEntities(string[] ids, Func<string[], IEnumerable<TEntity>>? performGetEntities = null)
    {
        var intIds = ids.Select(int.Parse).ToArray();
        if (performGetEntities == null)
        {
            return _cachePolicy.GetAll(intIds, _ => Enumerable.Empty<TEntity>());
        }

        TEntity[] returnValue = _cachePolicy.GetAll(intIds, _ => Enumerable.Empty<TEntity>());

        if (!returnValue.Any())
        {
            returnValue = performGetEntities(ids).ToArray();
        }

        return returnValue;
    }

    /// <inheritdoc/>
    public TEntity? GetEntity(string id, Func<string, TEntity?>? performGet = null)
    {
        var intId = int.Parse(id);
        if (performGet == null)
        {
            return _cachePolicy.Get(intId, _ => null, _ => null);
        }

        return _cachePolicy.Get(intId, _ => null, _ => null) ?? performGet(id);
    }

    /// <inheritdoc/>
    public void RefreshEntity(string id) => throw new NotImplementedException("This implementation doesn't support refreshing entities.");

    /// <inheritdoc/>
    public void Remove(TEntity entity)
    {
        _cachePolicy.Delete(entity, entity => { });
    }

    /// <inheritdoc/>
    public void Set(TEntity entity, EntityCacheOptions? options = null)
    {
        Set(entity.Id.ToString(), entity, options);
    }

    /// <inheritdoc/>
    public void Set(string key, TEntity entity, EntityCacheOptions? options = null)
    {
        if (GetEntity(key) != null)
        {
            _cachePolicy.Update(entity, entity => { });
            return;
        }

        _cachePolicy.Create(entity, entity => { });
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
