using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

/// <summary>
///     Represents a repository for <see cref="IAuditEntry" /> entities.
/// </summary>
public interface IAuditEntryRepository : IReadWriteQueryRepository<int, IAuditEntry>
{
    /// <summary>
    ///     Gets a page of entries.
    /// </summary>
    IEnumerable<IAuditEntry> GetPage(long pageIndex, int pageCount, out long records);

    /// <summary>
    ///     Gets a page of entries.
    /// </summary>
    Task<(IEnumerable<IAuditEntry> Results, long Records)> GetPageAsync(long pageIndex, int pageCount, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Determines whether the repository is available.
    /// </summary>
    /// <remarks>During an upgrade, the repository may not be available, until the table has been created.</remarks>
    bool IsAvailable();

    /// <summary>
    ///     Determines whether the repository is available.
    /// </summary>
    /// <remarks>During an upgrade, the repository may not be available, until the table has been created.</remarks>
    Task<bool> IsAvailableAsync(CancellationToken? cancellationToken = null);
}
