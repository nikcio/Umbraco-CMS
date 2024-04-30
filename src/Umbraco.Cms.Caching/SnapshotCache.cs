using Umbraco.Cms.Core.Cache;

namespace Umbraco.Cms.Caching;

/// <remarks>
/// This is based on a ConcurrencyDictionary and does therefore not act like other caches.
/// TODO: Review if this is the intended implementation.
/// </remarks>
internal class SnapshotCache : DictionaryAppCache
{
}
