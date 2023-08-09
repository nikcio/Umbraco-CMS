using Umbraco.Cms.Core.Events;

namespace Umbraco.Cms.Core.Scoping;

/// <summary>
/// Represents a notification scope.
/// </summary>
public interface INotificationScope : IBaseScope
{
    /// <summary>
    /// Gets the notification publisher.
    /// </summary>
    IScopedNotificationPublisher NotificationPublisher { get; }
}
