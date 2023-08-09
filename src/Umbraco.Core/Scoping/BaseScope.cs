using Microsoft.Extensions.Logging;

namespace Umbraco.Cms.Core.Scoping;

/// <summary>
/// Provides a base class for scopes.
/// </summary>
public abstract class BaseScope : IBaseScope
{
    private bool _disposedValue;
    private bool _completed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseScope"/> class.
    /// </summary>
    /// <param name="parentScope"></param>
    /// <param name="loggerFactory"></param>
    protected BaseScope(IBaseScope? parentScope, ILoggerFactory loggerFactory)
    {
        InstanceId = Guid.NewGuid();
        ParentScope = parentScope;
        LoggerFactory = loggerFactory;
    }

    /// <inheritdoc/>
    public Guid InstanceId { get; }

    /// <inheritdoc/>
    public int Depth
    {
        get
        {
            if (ParentScope == null)
            {
                return 0;
            }

            return ParentScope.Depth + 1;
        }
    }

    /// <summary>
    /// Gets the parent scope.
    /// </summary>
    protected IBaseScope? ParentScope { get; }

    /// <summary>
    /// Gets the logger factory.
    /// </summary>
    protected ILoggerFactory LoggerFactory { get; }

    /// <inheritdoc/>
    public void Completed(bool completed)
    {
        _completed = completed;
        Dispose();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the scope.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            ParentScope?.Completed(_completed);
        }

        _completed = true;
        _disposedValue = true;
    }
}
