using System.Data;
using Umbraco.Cms.Core.Models.Entities;
using Umbraco.Cms.Core.Persistence.Repositories;

namespace Umbraco.Cms.Core.Persistence;

/// <summary>
/// Represents a unit of work for a database.
/// </summary>
public interface IDatabaseUnitOfWork
{
    /// <summary>
    /// Starts the unit of work.
    /// </summary>
    /// <param name="isolationLevel"></param>
    void Start(IsolationLevel? isolationLevel = null);

    /// <summary>
    /// Starts the unit of work asynchronously.
    /// </summary>
    /// <param name="isolationLevel"></param>
    /// <returns></returns>
    Task StartAsync(IsolationLevel? isolationLevel = null) => throw new NotImplementedException(); //TODO: Implement

    /// <summary>
    /// Completes the unit of work.
    /// </summary>
    void Complete();

    /// <summary>
    /// Completes the unit of work asynchronously.
    /// </summary>
    /// <returns></returns>
    Task CompleteAsync() => throw new NotImplementedException(); //TODO: Implement

    /// <summary>
    /// Read-locks some lock objects.
    /// </summary>
    /// <param name="lockIds">Array of lock object identifiers.</param>
    void ReadLock(params int[] lockIds);

    /// <summary>
    /// Read-locks some lock objects.
    /// </summary>
    /// <param name="timeout">The database timeout in milliseconds</param>
    /// <param name="lockIds">Array of object identifiers.</param>
    void ReadLock(TimeSpan timeout, params int[] lockIds);

    /// <summary>
    /// Write-locks some lock objects.
    /// </summary>
    /// <param name="lockIds">Array of object identifiers.</param>
    void WriteLock(params int[] lockIds);

    /// <summary>
    /// Write-locks some lock objects.
    /// </summary>
    /// <param name="timeout">The database timeout in milliseconds</param>
    /// <param name="lockIds">Array of object identifiers.</param>
    void WriteLock(TimeSpan timeout, params int[] lockIds);
}
