using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface ITwoFactorLoginRepository : IReadRepository<int, ITwoFactorLogin>, IWriteRepository<ITwoFactorLogin>
{
    Task<bool> DeleteUserLoginsAsync(Guid userOrMemberKey, CancellationToken? cancellationToken = null);

    Task<bool> DeleteUserLoginsAsync(Guid userOrMemberKey, string providerName, CancellationToken? cancellationToken = null);

    Task<IEnumerable<ITwoFactorLogin>> GetByUserOrMemberKeyAsync(Guid userOrMemberKey, CancellationToken? cancellationToken = null);
}
