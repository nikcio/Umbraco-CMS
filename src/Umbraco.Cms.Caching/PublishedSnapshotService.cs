using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.PublishedCache;

namespace Umbraco.Cms.Caching;

internal class PublishedSnapshotService : IPublishedSnapshotService
{
    private readonly IPublishedContentCache _publishedContentCache;
    private readonly IPublishedMediaCache _publishedMediaCache;
    private readonly IPublishedMemberCache _publishedMemberCache;
    private readonly IDomainCache _domainCache;

    public PublishedSnapshotService(
        IPublishedContentCache publishedContentCache,
        IPublishedMediaCache publishedMediaCache,
        IPublishedMemberCache publishedMemberCache,
        IDomainCache domainCache)
    {
        _publishedContentCache = publishedContentCache;
        _publishedMediaCache = publishedMediaCache;
        _publishedMemberCache = publishedMemberCache;
        _domainCache = domainCache;
    }

    public Task CollectAsync() => Task.CompletedTask;

    public IPublishedSnapshot CreatePublishedSnapshot(string? previewToken)
    {
        bool defaultPreviewValue = !string.IsNullOrWhiteSpace(previewToken);

        return new PublishedSnapshot(
            defaultPreviewValue,
            _publishedContentCache,
            _publishedMediaCache,
            _publishedMemberCache,
            _domainCache);
    }

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
