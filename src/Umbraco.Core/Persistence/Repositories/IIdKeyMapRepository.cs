using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IIdKeyMapRepository
{
    int? GetIdForKey(Guid key, UmbracoObjectTypes umbracoObjectType);

    Task<int?> GetIdForKeyAsync(Guid key, UmbracoObjectTypes umbracoObjectType, CancellationToken? cancellationToken = null);

    Guid? GetIdForKey(int id, UmbracoObjectTypes umbracoObjectType);

    Task<Guid?> GetIdForKeyAsync(int id, UmbracoObjectTypes umbracoObjectType, CancellationToken? cancellationToken = null);
}
