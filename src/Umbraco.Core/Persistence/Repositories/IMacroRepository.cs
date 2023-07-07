using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IMacroRepository : IReadWriteQueryRepository<int, IMacro>, IReadRepository<Guid, IMacro>
{
    IMacro? GetByAlias(string alias);

    Task<IMacro?> GetByAliasAsync(string alias, CancellationToken? cancellationToken = null);

    IEnumerable<IMacro> GetAllByAlias(string[] aliases);

    Task<IEnumerable<IMacro>> GetAllByAliasAsync(string[] aliases, CancellationToken? cancellationToken = null);

}
