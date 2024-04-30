using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NeoSmart.Caching.Sqlite;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Umbraco.Cms.Caching.Composition;

internal class CacheComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.TryAddSingleton<IPublishedSnapshotStatus, PublishedSnapshotStatus>();
        builder.Services.TryAddTransient<IReservedFieldNamesService, ReservedFieldNamesService>();
        builder.SetPublishedSnapshotService<PublishedSnapshotService>();

        builder.Services.AddFusionCache()
            .WithOptions(options =>
            {
                options.CacheName = "Umbraco";
                options.EnableAutoRecovery = true;
            })
            .WithSerializer(new FusionCacheSystemTextJsonSerializer())
            .WithDistributedCache(new SqliteCache(new SqliteCacheOptions { CachePath = "fusion-cache.db" }));

        builder.Services.AddSingleton<IDomainCache, DomainCache>();
        builder.Services.AddSingleton<DomainCacheFactory>();
    }
}
