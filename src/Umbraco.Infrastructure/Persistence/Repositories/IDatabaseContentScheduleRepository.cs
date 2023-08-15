using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the ContentScheduleRepository for doing CRUD operations for <see cref="ContentSchedule"/>.
/// </summary>
public interface IDatabaseContentScheduleRepository : IDatabaseRepository
{
    /// <summary>
    /// Saves a content schedule for an <see cref="IContent"/>.
    /// </summary>
    /// <param name="contentItem"></param>
    /// <param name="contentScheduleCollection"></param>
    void SaveContentSchedule(IContent contentItem, ContentScheduleCollection contentScheduleCollection);
}
