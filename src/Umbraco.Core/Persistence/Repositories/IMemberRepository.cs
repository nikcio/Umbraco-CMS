using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IMemberRepository : IContentRepository<int, IMember>
{
    int[] GetMemberIds(string[] names);

    Task<int[]> GetMemberIdsAsync(string[] names, CancellationToken? cancellationToken = null);

    IMember? GetByUsername(string? username);

    Task<IMember?> GetByUsernameAsync(string? username, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Finds members in a given role
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="usernameToMatch"></param>
    /// <param name="matchType"></param>
    /// <returns></returns>
    IEnumerable<IMember> FindMembersInRole(string roleName, string usernameToMatch, StringPropertyMatchType matchType = StringPropertyMatchType.StartsWith);

    /// <summary>
    ///     Finds members in a given role
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="usernameToMatch"></param>
    /// <param name="matchType"></param>
    /// <returns></returns>
    Task<IEnumerable<IMember>> FindMembersInRoleAsync(string roleName, string usernameToMatch, StringPropertyMatchType matchType = StringPropertyMatchType.StartsWith, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Get all members in a specific group
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    IEnumerable<IMember> GetByMemberGroup(string groupName);

    /// <summary>
    ///     Get all members in a specific group
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    Task<IEnumerable<IMember>> GetByMemberGroupAsync(string groupName, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Checks if a member with the username exists
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    bool Exists(string username);

    /// <summary>
    ///     Checks if a member with the username exists
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(string username, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets the count of items based on a complex query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    int GetCountByQuery(IQuery<IMember>? query);

    /// <summary>
    ///     Gets the count of items based on a complex query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<int> GetCountByQueryAsync(IQuery<IMember>? query, CancellationToken? cancellationToken = null);
}
