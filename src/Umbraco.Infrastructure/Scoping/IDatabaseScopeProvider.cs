using System.Data;

namespace Umbraco.Cms.Core.Scoping;

/// <summary>
/// Provides a way to create <see cref="IDatabaseScope"/> instances.
/// </summary>
public interface IDatabaseScopeProvider
{
    /// <summary>
    /// Creates a new <see cref="IDatabaseScope"/> instance.
    /// </summary>
    /// <param name="parentScope">The current scope. This will be set as the parent scope.</param>
    /// <param name="isolationLevel">The isolation level of the transaction. If null is passed no transation will be started. You can't begin a transation if one is already active.</param>
    /// <returns>The new instance of <see cref="IDatabaseScope"/>.</returns>
    IDatabaseScope CreateScope(IDatabaseScope? parentScope = null, IsolationLevel? isolationLevel = null);
}
