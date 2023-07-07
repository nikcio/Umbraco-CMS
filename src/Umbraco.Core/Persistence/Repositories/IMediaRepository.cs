using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IMediaRepository : IContentRepository<int, IMedia>, IReadRepository<Guid, IMedia>
{
    IMedia? GetMediaByPath(string mediaPath);

    Task<IMedia?> GetMediaByPathAsync(string mediaPath, CancellationToken? cancellationToken = null);

    bool RecycleBinSmells();

    Task<bool> RecycleBinSmellsAsync(CancellationToken? cancellationToken = null);
}
