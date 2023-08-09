using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;

namespace Umbraco.Cms.Core.Scoping;

/// <summary>
/// Represents a scope for notifications.
/// </summary>
public class NotificationScope : BaseScope, INotificationScope
{
    private bool _disposedValue;
    private bool _completed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationScope"/> class.
    /// </summary>
    /// <param name="parentScope"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="scopedNotificationPublisher"></param>
    public NotificationScope(IBaseScope? parentScope, ILoggerFactory loggerFactory, IScopedNotificationPublisher scopedNotificationPublisher)
        : base(parentScope, loggerFactory)
    {
        NotificationPublisher = scopedNotificationPublisher;
    }

    /// <inheritdoc/>
    public IScopedNotificationPublisher NotificationPublisher { get; }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            NotificationPublisher.ScopeExit(_completed);
        }

        base.Dispose(disposing);

        _completed = true;
        _disposedValue = true;
    }
}
