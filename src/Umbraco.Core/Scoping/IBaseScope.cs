namespace Umbraco.Cms.Core.Scoping;

/// <summary>
/// Represents a base scope.
/// </summary>
public interface IBaseScope : IDisposable
{
    /// <summary>
    /// Gets the instance unique identifier.
    /// </summary>
    Guid InstanceId { get; }

    /// <summary>
    /// Gets the distance from the root scope.
    /// </summary>
    /// <remarks>
    /// A zero represents a root scope, any value greater than zero represents a child scope.
    /// </remarks>
    public int Depth { get; }

    /// <summary>
    /// Marks the scope as completed.
    /// </summary>
    /// <param name="completed"></param>
    /// <remarks>Completing a scope disposes the stack of scopes</remarks>
    void Completed(bool completed);
}
