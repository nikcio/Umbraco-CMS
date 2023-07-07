using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Membership;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IDocumentRepository : IContentRepository<int, IContent>, IReadRepository<Guid, IContent>
{
    /// <summary>
    ///     Gets publish/unpublish schedule for a content node.
    /// </summary>
    /// <param name="contentId"></param>
    /// <returns>
    ///     <see cref="ContentScheduleCollection" />
    /// </returns>
    ContentScheduleCollection GetContentSchedule(int contentId);

    /// <summary>
    ///     Gets publish/unpublish schedule for a content node.
    /// </summary>
    /// <param name="contentId"></param>
    /// <returns>
    ///     <see cref="ContentScheduleCollection" />
    /// </returns>
    Task<ContentScheduleCollection> GetContentScheduleAsync(int contentId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Persists publish/unpublish schedule for a content node.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="schedule"></param>
    void PersistContentSchedule(IContent content, ContentScheduleCollection schedule);

    /// <summary>
    ///     Persists publish/unpublish schedule for a content node.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="schedule"></param>
    Task PersistContentScheduleAsync(IContent content, ContentScheduleCollection schedule, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Clears the publishing schedule for all entries having an a date before (lower than, or equal to) a specified date.
    /// </summary>
    void ClearSchedule(DateTime date);

    /// <summary>
    ///     Clears the publishing schedule for all entries having an a date before (lower than, or equal to) a specified date.
    /// </summary>
    Task ClearScheduleAsync(DateTime date, CancellationToken? cancellationToken = null);

    void ClearSchedule(DateTime date, ContentScheduleAction action);

    Task ClearScheduleAsync(DateTime date, ContentScheduleAction action, CancellationToken? cancellationToken = null);

    bool HasContentForExpiration(DateTime date);

    Task<bool> HasContentForExpirationAsync(DateTime date, CancellationToken? cancellationToken = null);

    bool HasContentForRelease(DateTime date);

    Task<bool> HasContentForReleaseAsync(DateTime date, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets <see cref="IContent" /> objects having an expiration date before (lower than, or equal to) a specified date.
    /// </summary>
    /// <remarks>
    ///     The content returned from this method may be culture variant, in which case the resulting
    ///     <see cref="IContent.ContentSchedule" /> should be queried
    ///     for which culture(s) have been scheduled.
    /// </remarks>
    IEnumerable<IContent> GetContentForExpiration(DateTime date);

    /// <summary>
    ///     Gets <see cref="IContent" /> objects having an expiration date before (lower than, or equal to) a specified date.
    /// </summary>
    /// <remarks>
    ///     The content returned from this method may be culture variant, in which case the resulting
    ///     <see cref="IContent.ContentSchedule" /> should be queried
    ///     for which culture(s) have been scheduled.
    /// </remarks>
    Task<IEnumerable<IContent>> GetContentForExpirationAsync(DateTime date, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets <see cref="IContent" /> objects having a release date before (lower than, or equal to) a specified date.
    /// </summary>
    /// <remarks>
    ///     The content returned from this method may be culture variant, in which case the resulting
    ///     <see cref="IContent.ContentSchedule" /> should be queried
    ///     for which culture(s) have been scheduled.
    /// </remarks>
    IEnumerable<IContent> GetContentForRelease(DateTime date);

    /// <summary>
    ///     Gets <see cref="IContent" /> objects having a release date before (lower than, or equal to) a specified date.
    /// </summary>
    /// <remarks>
    ///     The content returned from this method may be culture variant, in which case the resulting
    ///     <see cref="IContent.ContentSchedule" /> should be queried
    ///     for which culture(s) have been scheduled.
    /// </remarks>
    Task<IEnumerable<IContent>> GetContentForReleaseAsync(DateTime date, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Get the count of published items
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    ///     We require this on the repo because the IQuery{IContent} cannot supply the 'newest' parameter
    /// </remarks>
    int CountPublished(string? contentTypeAlias = null);

    /// <summary>
    ///     Get the count of published items
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    ///     We require this on the repo because the IQuery{IContent} cannot supply the 'newest' parameter
    /// </remarks>
    Task<int> CountPublishedAsync(string? contentTypeAlias = null, CancellationToken? cancellationToken = null);

    bool IsPathPublished(IContent? content);

    Task<bool> IsPathPublishedAsync(IContent? content, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Used to bulk update the permissions set for a content item. This will replace all permissions
    ///     assigned to an entity with a list of user id &amp; permission pairs.
    /// </summary>
    /// <param name="permissionSet"></param>
    void ReplaceContentPermissions(EntityPermissionSet permissionSet);

    /// <summary>
    ///     Used to bulk update the permissions set for a content item. This will replace all permissions
    ///     assigned to an entity with a list of user id &amp; permission pairs.
    /// </summary>
    /// <param name="permissionSet"></param>
    Task ReplaceContentPermissionsAsync(EntityPermissionSet permissionSet, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Assigns a single permission to the current content item for the specified user group ids
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="permission"></param>
    /// <param name="groupIds"></param>
    void AssignEntityPermission(IContent entity, char permission, IEnumerable<int> groupIds);

    /// <summary>
    ///     Assigns a single permission to the current content item for the specified user group ids
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="permission"></param>
    /// <param name="groupIds"></param>
    Task AssignEntityPermissionAsync(IContent entity, char permission, IEnumerable<int> groupIds, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets the explicit list of permissions for the content item
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    EntityPermissionCollection GetPermissionsForEntity(int entityId);

    /// <summary>
    ///     Gets the explicit list of permissions for the content item
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    Task<EntityPermissionCollection> GetPermissionsForEntityAsync(int entityId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Used to add/update a permission for a content item
    /// </summary>
    /// <param name="permission"></param>
    void AddOrUpdatePermissions(ContentPermissionSet permission);

    /// <summary>
    ///     Used to add/update a permission for a content item
    /// </summary>
    /// <param name="permission"></param>
    Task AddOrUpdatePermissionsAsync(ContentPermissionSet permission, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Returns true if there is any content in the recycle bin
    /// </summary>
    bool RecycleBinSmells();

    /// <summary>
    ///     Returns true if there is any content in the recycle bin
    /// </summary>
    Task<bool> RecycleBinSmellsAsync(CancellationToken? cancellationToken = null);
}
