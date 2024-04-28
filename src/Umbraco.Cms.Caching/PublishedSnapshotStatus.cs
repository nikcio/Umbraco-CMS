using Umbraco.Cms.Core.PublishedCache;
using ZiggyCreatures.Caching.Fusion;
namespace Umbraco.Cms.Caching;

internal class PublishedSnapshotStatus : IPublishedSnapshotStatus
{
    private readonly IFusionCache _cache;

    public PublishedSnapshotStatus(IFusionCacheProvider fusionCacheProvider)
    {
        _cache = fusionCacheProvider.GetCache("Umbraco");
    }

    public string GetStatus()
    {
        return _cache.GetOrDefault("PublishedSnapshotStatus", "NOT ok (rebuild?)") ?? "NOT ok (rebuild?)";
    }
}
