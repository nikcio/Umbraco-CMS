using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.PublishedCache;

namespace Umbraco.Cms.Caching;

internal class PublishedSnapshotService : IPublishedSnapshotService
{
    public Task CollectAsync() => Task.CompletedTask;

    public IPublishedSnapshot CreatePublishedSnapshot(string? previewToken) => new PublishedSnapshot();

    public void Dispose()
    {
    }

    public void Notify(ContentCacheRefresher.JsonPayload[] payloads, out bool draftChanged, out bool publishedChanged) => throw new NotImplementedException();

    public void Notify(MediaCacheRefresher.JsonPayload[] payloads, out bool anythingChanged) => throw new NotImplementedException();

    public void Notify(ContentTypeCacheRefresher.JsonPayload[] payloads) => throw new NotImplementedException();

    public void Notify(DataTypeCacheRefresher.JsonPayload[] payloads) => throw new NotImplementedException();

    public void Notify(DomainCacheRefresher.JsonPayload[] payloads) => throw new NotImplementedException();

    public void Rebuild(IReadOnlyCollection<int>? contentTypeIds = null, IReadOnlyCollection<int>? mediaTypeIds = null, IReadOnlyCollection<int>? memberTypeIds = null) => throw new NotImplementedException();
}
