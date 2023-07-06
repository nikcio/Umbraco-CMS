using Umbraco.Cms.Core.Security;

namespace Umbraco.Cms.Core.Persistence.Repositories;

/// <summary>
///     Repository for external logins with Guid as key, so it can be shared for members and users
/// </summary>
public interface IExternalLoginWithKeyRepository : IReadWriteQueryRepository<int, IIdentityUserLogin>,
    IQueryRepository<IIdentityUserToken>
{
    /// <summary>
    ///     Replaces all external login providers for the user/member key
    /// </summary>
    void Save(Guid userOrMemberKey, IEnumerable<IExternalLogin> logins);

    /// <summary>
    ///     Replaces all external login providers for the user/member key
    /// </summary>
    Task SaveAsync(Guid userOrMemberKey, IEnumerable<IExternalLogin> logins, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Replaces all external login provider tokens for the providers specified for the user/member key
    /// </summary>
    void Save(Guid userOrMemberKey, IEnumerable<IExternalLoginToken> tokens);

    /// <summary>
    ///     Replaces all external login provider tokens for the providers specified for the user/member key
    /// </summary>
    Task SaveAsync(Guid userOrMemberKey, IEnumerable<IExternalLoginToken> tokens, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Deletes all external logins for the specified the user/member key
    /// </summary>
    void DeleteUserLogins(Guid userOrMemberKey);

    /// <summary>
    ///     Deletes all external logins for the specified the user/member key
    /// </summary>
    Task DeleteUserLoginsAsync(Guid userOrMemberKey, CancellationToken? cancellationToken = null);
}
