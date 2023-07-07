using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Entities;
using Umbraco.Cms.Core.Persistence.Querying;
using Umbraco.Cms.Core.Services;

namespace Umbraco.Cms.Core.Persistence.Repositories;

/// <summary>
///     Defines the base implementation of a repository for content items.
/// </summary>
public interface IContentRepository<in TId, TEntity> : IReadWriteQueryRepository<TId, TEntity>
    where TEntity : IUmbracoEntity
{
    /// <summary>
    ///     Gets the recycle bin identifier.
    /// </summary>
    int RecycleBinId { get; }

    /// <summary>
    ///     Gets versions.
    /// </summary>
    /// <remarks>Current version is first, and then versions are ordered with most recent first.</remarks>
    IEnumerable<TEntity> GetAllVersions(int nodeId);

    /// <summary>
    ///     Gets versions.
    /// </summary>
    /// <remarks>Current version is first, and then versions are ordered with most recent first.</remarks>
    Task<IEnumerable<TEntity>> GetAllVersionsAsync(int nodeId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets versions.
    /// </summary>
    /// <remarks>Current version is first, and then versions are ordered with most recent first.</remarks>
    IEnumerable<TEntity> GetAllVersionsSlim(int nodeId, int skip, int take);

    /// <summary>
    ///     Gets versions.
    /// </summary>
    /// <remarks>Current version is first, and then versions are ordered with most recent first.</remarks>
    Task<IEnumerable<TEntity>> GetAllVersionsSlimAsync(int nodeId, int skip, int take, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets version identifiers.
    /// </summary>
    /// <remarks>Current version is first, and then versions are ordered with most recent first.</remarks>
    IEnumerable<int> GetVersionIds(int id, int topRows);

    /// <summary>
    ///     Gets version identifiers.
    /// </summary>
    /// <remarks>Current version is first, and then versions are ordered with most recent first.</remarks>
    Task<IEnumerable<int>> GetVersionIdsAsync(int id, int topRows, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets a version.
    /// </summary>
    TEntity? GetVersion(int versionId);

    /// <summary>
    ///     Gets a version.
    /// </summary>
    Task<TEntity?> GetVersionAsync(int versionId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Deletes a version.
    /// </summary>
    void DeleteVersion(int versionId);

    /// <summary>
    ///     Deletes a version.
    /// </summary>
    Task DeleteVersionAsync(int versionId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Deletes all versions older than a date.
    /// </summary>
    void DeleteVersions(int nodeId, DateTime versionDate);

    /// <summary>
    ///     Deletes all versions older than a date.
    /// </summary>
    Task DeleteVersionsAsync(int nodeId, DateTime versionDate, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets the recycle bin content.
    /// </summary>
    IEnumerable<TEntity>? GetRecycleBin();

    /// <summary>
    ///     Gets the recycle bin content.
    /// </summary>
    Task<IEnumerable<TEntity>?> GetRecycleBinAsync(CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets the count of content items of a given content type.
    /// </summary>
    int Count(string? contentTypeAlias = null);

    /// <summary>
    ///     Gets the count of content items of a given content type.
    /// </summary>
    Task<int> CountAsync(string? contentTypeAlias = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets the count of child content items of a given parent content, of a given content type.
    /// </summary>
    int CountChildren(int parentId, string? contentTypeAlias = null);

    /// <summary>
    ///     Gets the count of child content items of a given parent content, of a given content type.
    /// </summary>
    Task<int> CountChildrenAsync(int parentId, string? contentTypeAlias = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets the count of descendant content items of a given parent content, of a given content type.
    /// </summary>
    int CountDescendants(int parentId, string? contentTypeAlias = null);

    /// <summary>
    ///     Gets the count of descendant content items of a given parent content, of a given content type.
    /// </summary>
    Task<int> CountDescendantsAsync(int parentId, string? contentTypeAlias = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets paged content items.
    /// </summary>
    /// <remarks>Here, <paramref name="filter" /> can be null but <paramref name="ordering" /> cannot.</remarks>
    IEnumerable<TEntity> GetPage(
        IQuery<TEntity>? query,
        long pageIndex,
        int pageSize,
        out long totalRecords,
        IQuery<TEntity>? filter,
        Ordering? ordering);

    /// <summary>
    ///     Gets paged content items.
    /// </summary>
    /// <remarks>Here, <paramref name="filter" /> can be null but <paramref name="ordering" /> cannot.</remarks>
    Task<(IEnumerable<TEntity> Results, long TotalRecords)> GetPageAsync(
        IQuery<TEntity>? query,
        long pageIndex,
        int pageSize,
        IQuery<TEntity>? filter,
        Ordering? ordering,
        CancellationToken? cancellationToken = null);

    ContentDataIntegrityReport CheckDataIntegrity(ContentDataIntegrityReportOptions options);

    Task<ContentDataIntegrityReport> CheckDataIntegrityAsync(ContentDataIntegrityReportOptions options, CancellationToken? cancellationToken = null);
}
