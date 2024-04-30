using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.PublishedCache;

namespace Umbraco.Cms.Caching;

internal class PublishedSnapshot : IPublishedSnapshot
{
    private bool _defaultPreviewValue;
    private bool _disposedValue;

    public PublishedSnapshot(
        bool defaultPreviewValue,
        IPublishedContentCache publishedContentCache,
        IPublishedMediaCache publishedMediaCache,
        IPublishedMemberCache publishedMemberCache,
        IDomainCache domainCache)
    {
        _defaultPreviewValue = defaultPreviewValue;

        Content = publishedContentCache;
        Media = publishedMediaCache;
        Members = publishedMemberCache;
        Domains = domainCache;
        SnapshotCache = new SnapshotCache();
        ElementsCache = new ElementsCache();
    }

    public IPublishedContentCache? Content { get; }

    public IPublishedMediaCache? Media { get; }

    public IPublishedMemberCache? Members { get; }

    public IDomainCache? Domains { get; }

    public IAppCache? SnapshotCache { get; }

    public IAppCache? ElementsCache { get; }

    public IDisposable ForcedPreview(bool preview, Action<bool>? callback = null) => new ForcedPreviewWrapper(this, preview, callback);

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private sealed class ForcedPreviewWrapper : IDisposable
    {
        private readonly Action<bool>? _callback;
        private readonly bool _originalPreviewValue;
        private readonly PublishedSnapshot _publishedSnapshot;
        private bool _disposedValue;

        public ForcedPreviewWrapper(PublishedSnapshot publishedShapshot, bool defaultPreviewValue, Action<bool>? callback)
        {
            _publishedSnapshot = publishedShapshot;
            _callback = callback;
            _originalPreviewValue = publishedShapshot._defaultPreviewValue;

            publishedShapshot._defaultPreviewValue = defaultPreviewValue;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _publishedSnapshot._defaultPreviewValue = _originalPreviewValue;
                    _callback?.Invoke(_originalPreviewValue);
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
