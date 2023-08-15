using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the ContentRelationRepository for doing CRUD operations for realtions on a<see cref="IContent"/>.
/// </summary>
public interface IDatabaseContentRelationRepository : IDatabaseRepository
{
    /// <summary>
    /// Saves the content relations for a content item.
    /// </summary>
    /// <param name="contentItem"></param>
    void SaveContentRelations(IContent contentItem);
}
