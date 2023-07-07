using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

/// <summary>
///     Represents a repository for <see cref="ICacheInstruction" /> entities.
/// </summary>
public interface ICacheInstructionRepository : IRepository
{
    /// <summary>
    ///     Gets the count of pending cache instruction records.
    /// </summary>
    int CountAll();

    /// <summary>
    ///     Gets the count of pending cache instruction records.
    /// </summary>
    Task<int> CountAllAsync(CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets the count of pending cache instructions.
    /// </summary>
    int CountPendingInstructions(int lastId);

    /// <summary>
    ///     Gets the count of pending cache instructions.
    /// </summary>
    Task<int> CountPendingInstructionsAsync(int lastId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets the most recent cache instruction record Id.
    /// </summary>
    /// <returns></returns>
    int GetMaxId();

    /// <summary>
    ///     Gets the most recent cache instruction record Id.
    /// </summary>
    /// <returns></returns>
    Task<int> GetMaxIdAsync(CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Checks to see if a single cache instruction by Id exists.
    /// </summary>
    bool Exists(int id);

    /// <summary>
    ///     Checks to see if a single cache instruction by Id exists.
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Adds a new cache instruction record.
    /// </summary>
    void Add(CacheInstruction cacheInstruction);

    /// <summary>
    ///     Adds a new cache instruction record.
    /// </summary>
    Task AddAsync(CacheInstruction cacheInstruction, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets a collection of cache instructions created later than the provided Id.
    /// </summary>
    /// <param name="lastId">Last id processed.</param>
    /// <param name="maxNumberToRetrieve">The maximum number of instructions to retrieve.</param>
    IEnumerable<CacheInstruction> GetPendingInstructions(int lastId, int maxNumberToRetrieve);

    /// <summary>
    ///     Gets a collection of cache instructions created later than the provided Id.
    /// </summary>
    /// <param name="lastId">Last id processed.</param>
    /// <param name="maxNumberToRetrieve">The maximum number of instructions to retrieve.</param>
    Task<IEnumerable<CacheInstruction>> GetPendingInstructionsAsync(int lastId, int maxNumberToRetrieve, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Deletes cache instructions older than the provided date.
    /// </summary>
    void DeleteInstructionsOlderThan(DateTime pruneDate);

    /// <summary>
    ///     Deletes cache instructions older than the provided date.
    /// </summary>
    Task DeleteInstructionsOlderThanAsync(DateTime pruneDate, CancellationToken? cancellationToken = null);
}
