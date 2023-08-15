using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the ContentRepository for doing CRUD operations for <see cref="IContent"/>.
/// </summary>
public interface IDatabaseContentRepository : IDatabaseRepository
{
    /// <summary>
    /// Saves a content item.
    /// </summary>
    /// <param name="contentItem"></param>
    IContent SaveContent(IContent contentItem);

    /// <summary>
    /// Saves a content item asynchronously.
    /// </summary>
    /// <param name="contentItem"></param>
    /// <returns></returns>
    Task<IContent> SaveContentAsync(IContent contentItem) => throw new NotImplementedException(); //TODO: Implement
}
