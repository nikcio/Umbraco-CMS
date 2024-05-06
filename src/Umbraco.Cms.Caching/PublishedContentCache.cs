using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;
using ZiggyCreatures.Caching.Fusion;

namespace Umbraco.Cms.Caching;

internal class PublishedContentCache : IPublishedContentCache
{
    private readonly IFusionCache _cache;
    private readonly PublishedContentCacheFactory _publishedContentCacheFactory;
    private bool _defaultPreviewValue;

    public PublishedContentCache(IFusionCacheProvider fusionCacheProvider, PublishedContentCacheFactory publishedContentCacheFactory)
    {
        _cache = fusionCacheProvider.GetCache(Caches.PublishedContentCache);
        _publishedContentCacheFactory = publishedContentCacheFactory;
    }

    internal void SetDefaultPreviewValue(bool value)
    {
        _defaultPreviewValue = value;
    }

    public IEnumerable<IPublishedContent> GetAtRoot(bool preview, string? culture = null)
    {
        FusionCacheEntryOptions cacheOptions = new()
        {
            Duration = TimeSpan.FromHours(1),
        };

        string cacheKey = CacheKeys.GetKeysAtRoot(preview, culture);

        List<Guid> allContentKeys = _cache.GetOrSet(cacheKey, (_) => _publishedContentCacheFactory.GetKeysAtRootFactory(preview, culture), cacheOptions);

        return allContentKeys.Select(GetById).OfType<IPublishedContent>();
    }

    public IEnumerable<IPublishedContent> GetAtRoot(string? culture = null) => GetAtRoot(_defaultPreviewValue, culture);

    public IEnumerable<IPublishedContent> GetByContentType(IPublishedContentType contentType)
    {
        FusionCacheEntryOptions cacheOptions = new()
        {
            Duration = TimeSpan.FromHours(1),
        };

        string cacheKey = CacheKeys.GetKeysByContentType(contentType.Alias);

        List<Guid> allContentKeys = _cache.GetOrSet(cacheKey, (_) => _publishedContentCacheFactory.GetKeysByContentTypeFactory(contentType), cacheOptions);

        return allContentKeys.Select(GetById).OfType<IPublishedContent>();
    }

    public IPublishedContent? GetById(bool preview, int contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(bool preview, Guid contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(bool preview, Udi contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(int contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(Guid contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(Udi contentId) => throw new NotImplementedException();

    public IPublishedContent? GetByRoute(bool preview, string route, bool? hideTopLevelNode = null, string? culture = null) => throw new NotImplementedException();

    public IPublishedContent? GetByRoute(string route, bool? hideTopLevelNode = null, string? culture = null) => throw new NotImplementedException();

    public IPublishedContentType? GetContentType(int id) => throw new NotImplementedException();

    public IPublishedContentType? GetContentType(string alias) => throw new NotImplementedException();

    public IPublishedContentType? GetContentType(Guid key) => throw new NotImplementedException();

    public string? GetRouteById(bool preview, int contentId, string? culture = null) => throw new NotImplementedException();

    public string? GetRouteById(int contentId, string? culture = null) => throw new NotImplementedException();

    public bool HasById(bool preview, int contentId) => throw new NotImplementedException();

    public bool HasById(int contentId) => throw new NotImplementedException();

    public bool HasContent(bool preview) => throw new NotImplementedException();

    public bool HasContent() => throw new NotImplementedException();

    public static class CacheKeys
    {
        public static string GetKeysAtRoot(bool includePreview, string? culture) => includePreview ? $"v0:GetKeysAtRoot:{culture ?? "__NULL__"}" : $"v0:GetKeysAtRoot:{culture ?? "__NULL__"}:IncludePreivew";

        public static string GetKeysByContentType(string contentTypeAlias) => $"v0:GetKeysByContentType:{contentTypeAlias}";

        public static string GetKeyById(int id) => $"v0:GetKeyById:{id}";

        public static string GetKeyByUdi(Udi udi) => $"v0:GetKeyByUdi:{udi}";

        public static string GetByKey(Guid guid) => $"v0:GetByKey:{guid}";
    }
}

internal class PublishedContentCacheFactory
{
    private readonly IContentService _contentService;
    private readonly ILogger<PublishedContentCache> _logger;

    public PublishedContentCacheFactory(IContentService contentService, ILogger<PublishedContentCache> logger)
    {
        _contentService = contentService;
        _logger = logger;
    }

    public List<Guid> GetKeysAtRootFactory(bool includePreview, string? culture)
    {
        IEnumerable<IContent> contentAtRoot = _contentService.GetRootContent();

        return contentAtRoot
            .Where(content => ShouldIncludeContentInCache(content, includePreview, culture))
            .Select(content => content.Key).ToList();
    }

    public List<Guid> GetKeysByContentTypeFactory(IPublishedContentType contentType)
    {
        IEnumerable<IContent> contentOfContentType = _contentService.GetRootContent()
            .SelectMany(x => _contentService.GetPagedDescendants(x.Id, 0, int.MaxValue, out _))
            .Where(x => x.ContentType.Id == contentType.Id);

        return contentOfContentType.Select(x => x.Key).ToList();
    }

    public Guid? GetKeyById(bool includePreview, int id)
    {
        IContent? contentItem = _contentService.GetById(id);

        if (contentItem == null)
        {
            return null;
        }

        if (!ShouldIncludeContentInCache(contentItem, includePreview, culture: null))
        {
            return null;
        }

        return contentItem.Key;
    }

    public IPublishedContent? GetByKey(bool includePreview, Guid key)
    {
        IContent? contentItem = _contentService.GetById(key);

        if (contentItem == null)
        {
            return null;
        }


        if (!ShouldIncludeContentInCache(contentItem, includePreview, culture: null))
        {
            return null;
        }
    }

    // TODO: Make unit tests for this method
    internal static bool ShouldIncludeContentInCache(IContent content, bool includePreview, string? culture)
    {
        if (culture != null && content.ContentType.VariesByCulture())
        {
            if (!includePreview)
            {
                return content.IsCultureAvailable(culture);
            }

            return content.IsCultureAvailable(culture) && content.IsCulturePublished(culture);
        }

        if (!includePreview)
        {
            return culture == null && !content.ContentType.VariesByCulture() && content.Published;
        }

        return culture == null && !content.ContentType.VariesByCulture();
    }
}
