// Copyright (c) Umbraco.
// See LICENSE for more details.

using Umbraco.Cms.Core.Notifications;

namespace Umbraco.Cms.Core.Events;

public class ScopedNotificationPublisher : ScopedNotificationPublisher<INotificationHandler>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScopedNotificationPublisher"/> class.
    /// </summary>
    /// <param name="eventAggregator"></param>
    public ScopedNotificationPublisher(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }
}

public class ScopedNotificationPublisher<TNotificationHandler> : IScopedNotificationPublisher
    where TNotificationHandler : INotificationHandler
{
    private readonly IEventAggregator _eventAggregator;
    private readonly List<INotification> _notificationOnScopeCompleted = new();
    private readonly bool _publishCancelableNotificationOnScopeExit;
    private readonly object _locker = new();
    private bool _isSuppressed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScopedNotificationPublisher{TNotificationHandler}"/> class.
    /// </summary>
    /// <param name="eventAggregator"></param>
    /// <param name="publishCancelableNotificationOnScopeExit"></param>
    public ScopedNotificationPublisher(IEventAggregator eventAggregator, bool publishCancelableNotificationOnScopeExit = false)
    {
        _eventAggregator = eventAggregator;
        _publishCancelableNotificationOnScopeExit = publishCancelableNotificationOnScopeExit;
    }

    /// <inheritdoc/>
    public bool PublishCancelable(ICancelableNotification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);

        if (_isSuppressed)
        {
            return false;
        }

        if (_publishCancelableNotificationOnScopeExit)
        {
            _notificationOnScopeCompleted.Add(notification);
        }
        else
        {
            _eventAggregator.Publish(notification);
        }

        return notification.Cancel;
    }

    /// <inheritdoc/>
    public async Task<bool> PublishCancelableAsync(ICancelableNotification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);

        if (_isSuppressed)
        {
            return false;
        }

        if (_publishCancelableNotificationOnScopeExit)
        {
            _notificationOnScopeCompleted.Add(notification);
        }
        else
        {
            Task task = _eventAggregator.PublishAsync(notification);
            if (task is not null)
            {
                await task;
            }
        }

        return notification.Cancel;
    }

    /// <inheritdoc/>
    public void Publish(INotification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);

        if (_isSuppressed)
        {
            return;
        }

        _notificationOnScopeCompleted.Add(notification);
    }

    /// <inheritdoc/>
    public void ScopeExit(bool completed)
    {
        try
        {
            if (completed)
            {
                PublishScopedNotifications(_notificationOnScopeCompleted);
            }
        }
        finally
        {
            _notificationOnScopeCompleted.Clear();
        }
    }

    /// <inheritdoc/>
    public IDisposable Suppress()
    {
        lock (_locker)
        {
            if (_isSuppressed)
            {
                throw new InvalidOperationException("Notifications are already suppressed.");
            }

            return new Suppressor(this);
        }
    }

    /// <summary>
    /// Publishes scoped notifications through the event aggregator.
    /// </summary>
    /// <param name="notifications">The notification to publish</param>
    protected virtual void PublishScopedNotifications(IList<INotification> notifications)
        => _eventAggregator.Publish<INotification, TNotificationHandler>(notifications);

    private sealed class Suppressor : IDisposable
    {
        private readonly ScopedNotificationPublisher<TNotificationHandler> _scopedNotificationPublisher;
        private bool _disposedValue;

        public Suppressor(ScopedNotificationPublisher<TNotificationHandler> scopedNotificationPublisher)
        {
            _scopedNotificationPublisher = scopedNotificationPublisher;
            _scopedNotificationPublisher._isSuppressed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    lock (_scopedNotificationPublisher._locker)
                    {
                        _scopedNotificationPublisher._isSuppressed = false;
                    }
                }

                _disposedValue = true;
            }
        }
    }
}
