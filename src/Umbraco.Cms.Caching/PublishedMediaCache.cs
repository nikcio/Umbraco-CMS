using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Umbraco.Cms.Caching;

internal class PublishedMediaCache : IPublishedMediaCache
{
    public IEnumerable<IPublishedContent> GetAtRoot(bool preview, string? culture = null) => throw new NotImplementedException();

    public IEnumerable<IPublishedContent> GetAtRoot(string? culture = null) => throw new NotImplementedException();

    public IEnumerable<IPublishedContent> GetByContentType(IPublishedContentType contentType) => throw new NotImplementedException();

    public IPublishedContent? GetById(bool preview, int contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(bool preview, Guid contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(bool preview, Udi contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(int contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(Guid contentId) => throw new NotImplementedException();

    public IPublishedContent? GetById(Udi contentId) => throw new NotImplementedException();

    public IPublishedContentType? GetContentType(int id) => throw new NotImplementedException();

    public IPublishedContentType? GetContentType(string alias) => throw new NotImplementedException();

    public IPublishedContentType? GetContentType(Guid key) => throw new NotImplementedException();

    public bool HasById(bool preview, int contentId) => throw new NotImplementedException();

    public bool HasById(int contentId) => throw new NotImplementedException();

    public bool HasContent(bool preview) => throw new NotImplementedException();

    public bool HasContent() => throw new NotImplementedException();
}
