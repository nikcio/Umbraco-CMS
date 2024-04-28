using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.PublishedCache;

namespace Umbraco.Cms.Caching;

internal class PublishedSnapshot : IPublishedSnapshot
{
    public IPublishedContentCache? Content { get; } = new PublishedContentCache();

    public IPublishedMediaCache? Media => throw new NotImplementedException();

    public IPublishedMemberCache? Members => throw new NotImplementedException();

    public IDomainCache? Domains => throw new NotImplementedException();

    public IAppCache? SnapshotCache => throw new NotImplementedException();

    public IAppCache? ElementsCache => throw new NotImplementedException();

    public void Dispose() => throw new NotImplementedException();

    public IDisposable ForcedPreview(bool preview, Action<bool>? callback = null) => throw new NotImplementedException();
}
