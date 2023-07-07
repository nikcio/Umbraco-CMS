using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Entities;
using Umbraco.Cms.Core.Models.Membership;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface INotificationsRepository : IRepository
{
    Notification CreateNotification(IUser user, IEntity entity, string action);

    Task<Notification> CreateNotificationAsync(IUser user, IEntity entity, string action, CancellationToken? cancellationToken = null);

    int DeleteNotifications(IUser user);

    Task<int> DeleteNotificationsAsync(IUser user, CancellationToken? cancellationToken = null);

    int DeleteNotifications(IEntity entity);

    Task<int> DeleteNotificationsAsync(IEntity entity, CancellationToken? cancellationToken = null);

    int DeleteNotifications(IUser user, IEntity entity);

    Task<int> DeleteNotificationsAsync(IUser user, IEntity entity, CancellationToken? cancellationToken = null);

    IEnumerable<Notification>? GetEntityNotifications(IEntity entity);

    Task<IEnumerable<Notification>?> GetEntityNotificationsAsync(IEntity entity, CancellationToken? cancellationToken = null);

    IEnumerable<Notification>? GetUserNotifications(IUser user);

    Task<IEnumerable<Notification>?> GetUserNotificationsAsync(IUser user, CancellationToken? cancellationToken = null);

    IEnumerable<Notification>? GetUsersNotifications(IEnumerable<int> userIds, string? action, IEnumerable<int> nodeIds, Guid objectType);

    Task<IEnumerable<Notification>?> GetUsersNotificationsAsync(IEnumerable<int> userIds, string? action, IEnumerable<int> nodeIds, Guid objectType, CancellationToken? cancellationToken = null);

    IEnumerable<Notification> SetNotifications(IUser user, IEntity entity, string[] actions);

    Task<IEnumerable<Notification>> SetNotificationsAsync(IUser user, IEntity entity, string[] actions, CancellationToken? cancellationToken = null);
}
