using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using ZiggyCreatures.Caching.Fusion;

namespace Umbraco.Cms.Caching;

internal class DomainCache : IDomainCache
{
    private readonly IFusionCache _cache;
    private readonly IDefaultCultureAccessor _defaultCultureAccessor;
    private readonly DomainCacheFactory _domainCacheFactory;

    public DomainCache(
        IFusionCacheProvider fusionCacheProvider,
        IDefaultCultureAccessor defaultCultureAccessor,
        DomainCacheFactory domainCacheFactory)
    {
        _cache = fusionCacheProvider.GetCache(Caches.DomainCache);
        _defaultCultureAccessor = defaultCultureAccessor;
        _domainCacheFactory = domainCacheFactory;
    }

    public string DefaultCulture => _defaultCultureAccessor.DefaultCulture;

    public IEnumerable<Domain> GetAll(bool includeWildcards)
    {
        FusionCacheEntryOptions cacheOptions = new()
        {
            Duration = TimeSpan.FromHours(1),
        };

        string cacheKey = CacheKeys.GetAllDomainIds(includeWildcards);

        List<int> allDomainIds = _cache.GetOrSet(cacheKey, (_) => _domainCacheFactory.GetAllKeysFactory(includeWildcards), cacheOptions);

        return allDomainIds.Select(GetDomain).OfType<Domain>();
    }

    private Domain? GetDomain(int domainId)
    {
        FusionCacheEntryOptions cacheOptions = new()
        {
            Duration = TimeSpan.FromHours(1),
        };

        string cacheKey = CacheKeys.Domain(domainId);

        return _cache.GetOrSet(cacheKey, (_) => _domainCacheFactory.GetDomainFactory(domainId), cacheOptions);
    }

    public IEnumerable<Domain> GetAssigned(int documentId, bool includeWildcards = false)
    {
        List<int> assignedDomainIds = GetAssignedDomainIds(documentId, includeWildcards);

        return assignedDomainIds.Select(GetDomain).OfType<Domain>();
    }

    public bool HasAssigned(int documentId, bool includeWildcards = false) => GetAssignedDomainIds(documentId, includeWildcards).Count != 0;

    private List<int> GetAssignedDomainIds(int documentId, bool includeWildcards)
    {
        FusionCacheEntryOptions cacheOptions = new()
        {
            Duration = TimeSpan.FromHours(1),
        };

        string cacheKey = CacheKeys.AssignedToIds(documentId, includeWildcards);

        return _cache.GetOrSet(cacheKey, (_) => _domainCacheFactory.GetAssignedIdsFactory(documentId, includeWildcards), cacheOptions);
    }

    public static class CacheKeys
    {
        public static string GetAllDomainIds(bool includeWildcards) => includeWildcards ? "v0:GetAllDomainIds" : "v0:GetAllDomainIds:IncludeWildcards";

        public static string Domain(int domainId) => $"v0:Domain:{domainId}";

        public static string AssignedToIds(int documentId, bool includeWildcards) => includeWildcards ? $"v0:AssignedToIds:{documentId}" : $"v0:AssignedToIds:{documentId}:IncludeWildcards";
    }
}

internal class DomainCacheFactory
{
    private readonly IDomainService _domainService;
    private readonly ILogger<DomainCache> _logger;

    public DomainCacheFactory(IDomainService domainService, ILogger<DomainCache> logger)
    {
        _domainService = domainService;
        _logger = logger;
    }

    public List<int> GetAllKeysFactory(bool includeWildcards)
    {
        IEnumerable<IDomain> allDomains = _domainService.GetAll(includeWildcards);

        return allDomains.Select(domain => domain.Id).ToList();
    }

    public Domain? GetDomainFactory(int domainId)
    {
        IDomain? domain = _domainService.GetById(domainId);

        if (domain == null)
        {
            _logger.LogWarning("Domain with id: {Id} could not be found in domain service.", domainId);
            return null;
        }

        if (domain.RootContentId == null || !domain.RootContentId.HasValue)
        {
            _logger.LogWarning("Domain with id: {Id} doesn't have a root content id. Domain has been skipped for caching.", domainId);
            return null;
        }

        if (string.IsNullOrWhiteSpace(domain.LanguageIsoCode))
        {
            _logger.LogWarning("Domain with id: {Id} doesn't have a valid language iso code. Domain has been skipped for caching. Provided languageIsoCode: {LanguageIsoCode}", domainId, domain.LanguageIsoCode);
            return null;
        }

        return new Domain(
            domain.Id,
            domain.DomainName,
            domain.RootContentId.Value,
            domain.LanguageIsoCode,
            domain.IsWildcard,
            domain.SortOrder);
    }

    public List<int> GetAssignedIdsFactory(int documentId, bool includeWildcards)
    {
        IEnumerable<IDomain> assignedDomains = _domainService.GetAssignedDomains(documentId, includeWildcards);

        return assignedDomains.Select(domain => domain.Id).ToList();
    }
}
