using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Umbraco.Cms.Caching;

internal class PublishedMemberCache : IPublishedMemberCache
{
    public IPublishedContent? Get(IMember member) => throw new NotImplementedException();

    public IPublishedContentType GetContentType(int id) => throw new NotImplementedException();

    public IPublishedContentType GetContentType(string alias) => throw new NotImplementedException();
}
