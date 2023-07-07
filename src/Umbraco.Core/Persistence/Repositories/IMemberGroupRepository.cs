using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IMemberGroupRepository : IReadWriteQueryRepository<int, IMemberGroup>
{
    /// <summary>
    ///     Gets a member group by it's uniqueId
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    IMemberGroup? Get(Guid uniqueId);

    /// <summary>
    ///     Gets a member group by it's uniqueId
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    Task<IMemberGroup?> GetAsync(Guid uniqueId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets a member group by it's name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IMemberGroup? GetByName(string? name);

    /// <summary>
    ///     Gets a member group by it's name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<IMemberGroup?> GetByNameAsync(string? name, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Creates the new member group if it doesn't already exist
    /// </summary>
    /// <param name="roleName"></param>
    IMemberGroup? CreateIfNotExists(string roleName);

    /// <summary>
    ///     Creates the new member group if it doesn't already exist
    /// </summary>
    /// <param name="roleName"></param>
    Task<IMemberGroup?> CreateIfNotExistsAsync(string roleName, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Returns the member groups for a given member
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    IEnumerable<IMemberGroup> GetMemberGroupsForMember(int memberId);

    /// <summary>
    ///     Returns the member groups for a given member
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    Task<IEnumerable<IMemberGroup>> GetMemberGroupsForMemberAsync(int memberId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Returns the member groups for a given member
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    IEnumerable<IMemberGroup> GetMemberGroupsForMember(string? username);

    /// <summary>
    ///     Returns the member groups for a given member
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<IEnumerable<IMemberGroup>> GetMemberGroupsForMemberAsync(string? username, CancellationToken? cancellationToken = null);

    void ReplaceRoles(int[] memberIds, string[] roleNames);

    Task ReplaceRolesAsync(int[] memberIds, string[] roleNames, CancellationToken? cancellationToken = null);

    void AssignRoles(int[] memberIds, string[] roleNames);

    Task AssignRolesAsync(int[] memberIds, string[] roleNames, CancellationToken? cancellationToken = null);

    void DissociateRoles(int[] memberIds, string[] roleNames);

    Task DissociateRolesAsync(int[] memberIds, string[] roleNames, CancellationToken? cancellationToken = null);
}
