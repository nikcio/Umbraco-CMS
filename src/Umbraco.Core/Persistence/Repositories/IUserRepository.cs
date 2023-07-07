using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Persistence.Querying;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IUserRepository : IReadWriteQueryRepository<int, IUser>
{
    /// <summary>
    ///     Gets the count of items based on a complex query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    int GetCountByQuery(IQuery<IUser>? query);

    /// <summary>
    ///     Gets the count of items based on a complex query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<int> GetCountByQueryAsync(IQuery<IUser>? query, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Checks if a user with the username exists
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    bool ExistsByUserName(string username);

    /// <summary>
    ///     Checks if a user with the username exists
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<bool> ExistsByUserNameAsync(string username, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Checks if a user with the login exists
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    bool ExistsByLogin(string login);

    /// <summary>
    ///     Checks if a user with the login exists
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    Task<bool> ExistsByLoginAsync(string login, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets a list of <see cref="IUser" /> objects associated with a given group
    /// </summary>
    /// <param name="groupId">Id of group</param>
    IEnumerable<IUser> GetAllInGroup(int groupId);

    /// <summary>
    ///     Gets a list of <see cref="IUser" /> objects associated with a given group
    /// </summary>
    /// <param name="groupId">Id of group</param>
    Task<IEnumerable<IUser>> GetAllInGroupAsync(int groupId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets a list of <see cref="IUser" /> objects not associated with a given group
    /// </summary>
    /// <param name="groupId">Id of group</param>
    IEnumerable<IUser> GetAllNotInGroup(int groupId);

    /// <summary>
    ///     Gets a list of <see cref="IUser" /> objects not associated with a given group
    /// </summary>
    /// <param name="groupId">Id of group</param>
    Task<IEnumerable<IUser>> GetAllNotInGroupAsync(int groupId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets paged user results
    /// </summary>
    /// <param name="query"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="totalRecords"></param>
    /// <param name="orderBy"></param>
    /// <param name="orderDirection"></param>
    /// <param name="includeUserGroups">
    ///     A filter to only include user that belong to these user groups
    /// </param>
    /// <param name="excludeUserGroups">
    ///     A filter to only include users that do not belong to these user groups
    /// </param>
    /// <param name="userState">Optional parameter to filter by specified user state</param>
    /// <param name="filter"></param>
    /// <returns></returns>
    IEnumerable<IUser> GetPagedResultsByQuery(
        IQuery<IUser>? query,
        long pageIndex,
        int pageSize,
        out long totalRecords,
        Expression<Func<IUser, object?>> orderBy,
        Direction orderDirection = Direction.Ascending,
        string[]? includeUserGroups = null,
        string[]? excludeUserGroups = null,
        UserState[]? userState = null,
        IQuery<IUser>? filter = null);

    /// <summary>
    ///     Gets paged user results
    /// </summary>
    /// <param name="query"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="totalRecords"></param>
    /// <param name="orderBy"></param>
    /// <param name="orderDirection"></param>
    /// <param name="includeUserGroups">
    ///     A filter to only include user that belong to these user groups
    /// </param>
    /// <param name="excludeUserGroups">
    ///     A filter to only include users that do not belong to these user groups
    /// </param>
    /// <param name="userState">Optional parameter to filter by specified user state</param>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<(IEnumerable<IUser> Results, long TotalRecords)> GetPagedResultsByQueryAsync(
        IQuery<IUser>? query,
        long pageIndex,
        int pageSize,
        Expression<Func<IUser, object?>> orderBy,
        Direction orderDirection = Direction.Ascending,
        string[]? includeUserGroups = null,
        string[]? excludeUserGroups = null,
        UserState[]? userState = null,
        IQuery<IUser>? filter = null,
        CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Returns a user by username
    /// </summary>
    /// <param name="username"></param>
    /// <param name="includeSecurityData">
    ///     This is only used for a shim in order to upgrade to 7.7
    /// </param>
    /// <returns>
    ///     A non cached <see cref="IUser" /> instance
    /// </returns>
    IUser? GetByUsername(string username, bool includeSecurityData);

    /// <summary>
    ///     Returns a user by username
    /// </summary>
    /// <param name="username"></param>
    /// <param name="includeSecurityData">
    ///     This is only used for a shim in order to upgrade to 7.7
    /// </param>
    /// <returns>
    ///     A non cached <see cref="IUser" /> instance
    /// </returns>
    Task<IUser?> GetByUsernameAsync(string username, bool includeSecurityData, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Returns a user by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeSecurityData">
    ///     This is only used for a shim in order to upgrade to 7.7
    /// </param>
    /// <returns>
    ///     A non cached <see cref="IUser" /> instance
    /// </returns>
    IUser? Get(int? id, bool includeSecurityData);

    /// <summary>
    ///     Returns a user by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeSecurityData">
    ///     This is only used for a shim in order to upgrade to 7.7
    /// </param>
    /// <returns>
    ///     A non cached <see cref="IUser" /> instance
    /// </returns>
    Task<IUser?> GetAsync(int? id, bool includeSecurityData, CancellationToken? cancellationToken = null);

    IProfile? GetProfile(string username);

    Task<IProfile?> GetProfileAsync(string username, CancellationToken? cancellationToken = null);

    IProfile? GetProfile(int id);

    Task<IProfile?> GetProfileAsync(int id, CancellationToken? cancellationToken = null);

    IDictionary<UserState, int> GetUserStates();

    Task<IDictionary<UserState, int>> GetUserStatesAsync(CancellationToken? cancellationToken = null);

    Guid CreateLoginSession(int? userId, string requestingIpAddress, bool cleanStaleSessions = true);

    Task<Guid> CreateLoginSessionAsync(int? userId, string requestingIpAddress, bool cleanStaleSessions = true, CancellationToken? cancellationToken = null);

    bool ValidateLoginSession(int userId, Guid sessionId);

    Task<bool> ValidateLoginSessionAsync(int userId, Guid sessionId, CancellationToken? cancellationToken = null);

    int ClearLoginSessions(int userId);

    Task<int> ClearLoginSessionsAsync(int userId, CancellationToken? cancellationToken = null);

    int ClearLoginSessions(TimeSpan timespan);

    Task<int> ClearLoginSessionsAsync(TimeSpan timespan, CancellationToken? cancellationToken = null);

    void ClearLoginSession(Guid sessionId);

    Task ClearLoginSessionAsync(Guid sessionId, CancellationToken? cancellationToken = null);

    IEnumerable<IUser> GetNextUsers(int id, int count);

    Task<IEnumerable<IUser>> GetNextUsersAsync(int id, int count, CancellationToken? cancellationToken = null);
}
