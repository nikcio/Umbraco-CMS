using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the ContentTagRepository for doing CRUD operations for tags on a <see cref="IContent"/>.
/// </summary>
public interface IDatabaseContentTagRepository : IDatabaseRepository
{
    /// <summary>
    /// Sets the tags on a content item.
    /// </summary>
    /// <param name="contentItem"></param>
    void SetContentTags(IContent contentItem);
}
