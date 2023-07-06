using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Entities;
using Umbraco.Cms.Core.Persistence.Querying;
using Umbraco.Cms.Core.Services;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IEntityRepository : IRepository
{
    IEntitySlim? Get(int id);

    Task<IEntitySlim?> GetAsync(int id, CancellationToken? cancellationToken = null);

    IEntitySlim? Get(Guid key);

    Task<IEntitySlim?> GetAsync(Guid key, CancellationToken? cancellationToken = null);

    IEntitySlim? Get(int id, Guid objectTypeId);

    Task<IEntitySlim?> GetAsync(int id, Guid objectTypeId, CancellationToken? cancellationToken = null);

    IEntitySlim? Get(Guid key, Guid objectTypeId);

    Task<IEntitySlim?> GetAsync(Guid key, Guid objectTypeId, CancellationToken? cancellationToken = null);

    IEnumerable<IEntitySlim> GetAll(Guid objectType, params int[] ids);

    Task<IEnumerable<IEntitySlim>> GetAllAsync(Guid objectType, CancellationToken? cancellationToken = null, params int[] ids);

    IEnumerable<IEntitySlim> GetAll(Guid objectType, params Guid[] keys);

    Task<IEnumerable<IEntitySlim>> GetAllAsync(Guid objectType, CancellationToken? cancellationToken = null, params Guid[] keys);

    /// <summary>
    ///     Gets entities for a query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    IEnumerable<IEntitySlim> GetByQuery(IQuery<IUmbracoEntity> query);

    /// <summary>
    ///     Gets entities for a query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<IEntitySlim>> GetByQueryAsync(IQuery<IUmbracoEntity> query, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets entities for a query and a specific object type allowing the query to be slightly more optimized
    /// </summary>
    /// <param name="query"></param>
    /// <param name="objectType"></param>
    /// <returns></returns>
    IEnumerable<IEntitySlim> GetByQuery(IQuery<IUmbracoEntity> query, Guid objectType);

    /// <summary>
    ///     Gets entities for a query and a specific object type allowing the query to be slightly more optimized
    /// </summary>
    /// <param name="query"></param>
    /// <param name="objectType"></param>
    /// <returns></returns>
    Task<IEnumerable<IEntitySlim>> GetByQueryAsync(IQuery<IUmbracoEntity> query, Guid objectType, CancellationToken? cancellationToken = null);

    UmbracoObjectTypes GetObjectType(int id);

    Task<UmbracoObjectTypes> GetObjectTypeAsync(int id, CancellationToken? cancellationToken = null);

    UmbracoObjectTypes GetObjectType(Guid key);

    Task<UmbracoObjectTypes> GetObjectTypeAsync(Guid key, CancellationToken? cancellationToken = null);

    int ReserveId(Guid key);

    Task<int> ReserveIdAsync(Guid key, CancellationToken? cancellationToken = null);

    IEnumerable<TreeEntityPath> GetAllPaths(Guid objectType, params int[]? ids);

    Task<IEnumerable<TreeEntityPath>> GetAllPathsAsync(Guid objectType, CancellationToken? cancellationToken = null, params int[]? ids);

    IEnumerable<TreeEntityPath> GetAllPaths(Guid objectType, params Guid[] keys);

    Task<IEnumerable<TreeEntityPath>> GetAllPathsAsync(Guid objectType, CancellationToken? cancellationToken = null, params Guid[] keys);

    bool Exists(int id);

    Task<bool> ExistsAsync(int id, CancellationToken? cancellationToken = null);

    bool Exists(Guid key);

    Task<bool> ExistsAsync(Guid key, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets paged entities for a query and a specific object type
    /// </summary>
    /// <param name="query"></param>
    /// <param name="objectType"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="totalRecords"></param>
    /// <param name="filter"></param>
    /// <param name="ordering"></param>
    /// <returns></returns>
    IEnumerable<IEntitySlim> GetPagedResultsByQuery(
        IQuery<IUmbracoEntity> query,
        Guid objectType,
        long pageIndex,
        int pageSize,
        out long totalRecords,
        IQuery<IUmbracoEntity>? filter,
        Ordering? ordering);

    /// <summary>
    ///     Gets paged entities for a query and a specific object type
    /// </summary>
    /// <param name="query"></param>
    /// <param name="objectType"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="filter"></param>
    /// <param name="ordering"></param>
    /// <returns></returns>
    Task<(IEnumerable<IEntitySlim> Results, long TotalRecords)> GetPagedResultsByQueryAsync(
        IQuery<IUmbracoEntity> query,
        Guid objectType,
        long pageIndex,
        int pageSize,
        IQuery<IUmbracoEntity>? filter,
        Ordering? ordering,
        CancellationToken? cancellationToken = null);
}
