using Umbraco.Cms.Core.Models.Membership;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IUserGroupRepository : IReadWriteQueryRepository<int, IUserGroup>
{
    /// <summary>
    ///     Gets a group by it's alias
    /// </summary>
    /// <param name="alias"></param>
    /// <returns></returns>
    IUserGroup? Get(string alias);

    /// <summary>
    ///     Gets a group by it's alias
    /// </summary>
    /// <param name="alias"></param>
    /// <returns></returns>
    Task<IUserGroup?> GetAsync(string alias, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     This is useful when an entire section is removed from config
    /// </summary>
    /// <param name="sectionAlias"></param>
    IEnumerable<IUserGroup> GetGroupsAssignedToSection(string sectionAlias);

    /// <summary>
    ///     This is useful when an entire section is removed from config
    /// </summary>
    /// <param name="sectionAlias"></param>
    Task<IEnumerable<IUserGroup>> GetGroupsAssignedToSectionAsync(string sectionAlias, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Used to add or update a user group and assign users to it
    /// </summary>
    /// <param name="userGroup"></param>
    /// <param name="userIds"></param>
    void AddOrUpdateGroupWithUsers(IUserGroup userGroup, int[]? userIds);

    /// <summary>
    ///     Used to add or update a user group and assign users to it
    /// </summary>
    /// <param name="userGroup"></param>
    /// <param name="userIds"></param>
    Task AddOrUpdateGroupWithUsersAsync(IUserGroup userGroup, int[]? userIds, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets explicitly defined permissions for the group for specified entities
    /// </summary>
    /// <param name="groupIds"></param>
    /// <param name="entityIds">Array of entity Ids, if empty will return permissions for the group for all entities</param>
    EntityPermissionCollection GetPermissions(int[] groupIds, params int[] entityIds);

    /// <summary>
    ///     Gets explicitly defined permissions for the group for specified entities
    /// </summary>
    /// <param name="groupIds"></param>
    /// <param name="entityIds">Array of entity Ids, if empty will return permissions for the group for all entities</param>
    Task<EntityPermissionCollection> GetPermissionsAsync(int[] groupIds, CancellationToken? cancellationToken = null, params int[] entityIds);

    /// <summary>
    ///     Gets explicit and default permissions (if requested) permissions for the group for specified entities
    /// </summary>
    /// <param name="groups"></param>
    /// <param name="fallbackToDefaultPermissions">
    ///     If true will include the group's default permissions if no permissions are
    ///     explicitly assigned
    /// </param>
    /// <param name="nodeIds">Array of entity Ids, if empty will return permissions for the group for all entities</param>
    EntityPermissionCollection GetPermissions(IReadOnlyUserGroup[]? groups, bool fallbackToDefaultPermissions, params int[] nodeIds);

    /// <summary>
    ///     Gets explicit and default permissions (if requested) permissions for the group for specified entities
    /// </summary>
    /// <param name="groups"></param>
    /// <param name="fallbackToDefaultPermissions">
    ///     If true will include the group's default permissions if no permissions are
    ///     explicitly assigned
    /// </param>
    /// <param name="nodeIds">Array of entity Ids, if empty will return permissions for the group for all entities</param>
    Task<EntityPermissionCollection> GetPermissionsAsync(IReadOnlyUserGroup[]? groups, bool fallbackToDefaultPermissions, CancellationToken? cancellationToken = null, params int[] nodeIds);

    /// <summary>
    ///     Replaces the same permission set for a single group to any number of entities
    /// </summary>
    /// <param name="groupId">Id of group</param>
    /// <param name="permissions">Permissions as enumerable list of <see cref="char" /></param>
    /// <param name="entityIds">
    ///     Specify the nodes to replace permissions for. If nothing is specified all permissions are
    ///     removed.
    /// </param>
    void ReplaceGroupPermissions(int groupId, IEnumerable<char>? permissions, params int[] entityIds);

    /// <summary>
    ///     Replaces the same permission set for a single group to any number of entities
    /// </summary>
    /// <param name="groupId">Id of group</param>
    /// <param name="permissions">Permissions as enumerable list of <see cref="char" /></param>
    /// <param name="entityIds">
    ///     Specify the nodes to replace permissions for. If nothing is specified all permissions are
    ///     removed.
    /// </param>
    Task ReplaceGroupPermissionsAsync(int groupId, IEnumerable<char>? permissions, CancellationToken? cancellationToken = null, params int[] entityIds);

    /// <summary>
    ///     Assigns the same permission set for a single group to any number of entities
    /// </summary>
    /// <param name="groupId">Id of group</param>
    /// <param name="permission">Permissions as enumerable list of <see cref="char" /></param>
    /// <param name="entityIds">Specify the nodes to replace permissions for</param>
    void AssignGroupPermission(int groupId, char permission, params int[] entityIds);

    /// <summary>
    ///     Assigns the same permission set for a single group to any number of entities
    /// </summary>
    /// <param name="groupId">Id of group</param>
    /// <param name="permission">Permissions as enumerable list of <see cref="char" /></param>
    /// <param name="entityIds">Specify the nodes to replace permissions for</param>
    Task AssignGroupPermissionAsync(int groupId, char permission, CancellationToken? cancellationToken = null, params int[] entityIds);
}
