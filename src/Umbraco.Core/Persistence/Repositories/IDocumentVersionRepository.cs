using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IDocumentVersionRepository : IRepository
{
    /// <summary>
    ///     Gets a list of all historic content versions.
    /// </summary>
    public IReadOnlyCollection<ContentVersionMeta>? GetDocumentVersionsEligibleForCleanup();

    /// <summary>
    ///     Gets a list of all historic content versions.
    /// </summary>
    public Task<IReadOnlyCollection<ContentVersionMeta>?> GetDocumentVersionsEligibleForCleanupAsync(CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets cleanup policy override settings per content type.
    /// </summary>
    public IReadOnlyCollection<ContentVersionCleanupPolicySettings>? GetCleanupPolicies();

    /// <summary>
    ///     Gets cleanup policy override settings per content type.
    /// </summary>
    public Task<IReadOnlyCollection<ContentVersionCleanupPolicySettings>?> GetCleanupPoliciesAsync(CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets paginated content versions for given content id paginated.
    /// </summary>
    public IEnumerable<ContentVersionMeta>? GetPagedItemsByContentId(int contentId, long pageIndex, int pageSize, out long totalRecords, int? languageId = null);

    /// <summary>
    ///     Gets paginated content versions for given content id paginated.
    /// </summary>
    public Task<(IEnumerable<ContentVersionMeta>? Results, long TotalRecords)> GetPagedItemsByContentIdAsync(int contentId, long pageIndex, int pageSize, int? languageId = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Deletes multiple content versions by ID.
    /// </summary>
    void DeleteVersions(IEnumerable<int> versionIds);

    /// <summary>
    ///     Deletes multiple content versions by ID.
    /// </summary>
    Task DeleteVersionsAsync(IEnumerable<int> versionIds, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Updates the prevent cleanup flag on a content version.
    /// </summary>
    void SetPreventCleanup(int versionId, bool preventCleanup);

    /// <summary>
    ///     Updates the prevent cleanup flag on a content version.
    /// </summary>
    Task SetPreventCleanupAsync(int versionId, bool preventCleanup, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets the content version metadata for a specific version.
    /// </summary>
    ContentVersionMeta? Get(int versionId);

    /// <summary>
    ///     Gets the content version metadata for a specific version.
    /// </summary>
    Task<ContentVersionMeta?> GetAsync(int versionId, CancellationToken? cancellationToken = null);
}
