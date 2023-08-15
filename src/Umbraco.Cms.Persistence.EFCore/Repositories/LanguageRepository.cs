using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Infrastructure.Cache;
using Umbraco.Cms.Infrastructure.Persistence.Repositories;
using Umbraco.Cms.Persistence.EFCore.DbContexts;
using Umbraco.Cms.Core;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Persistence.Models;

namespace Umbraco.Cms.Persistence.EFCore.Repositories;

internal class LanguageRepository : EFCoreeRepositoryBase, IDatabaseLanguageRepository
{
    private readonly IEntityCache<ILanguage> _cache;
    private readonly ILogger<LanguageRepository> _logger;

    public LanguageRepository(UmbracoDbContext dbContext, IEntityCache<ILanguage> cache, ILogger<LanguageRepository> logger)
        : base(dbContext)
    {
        _cache = cache;
        _logger = logger;
    }

    public string GetDefaultIsoCode()
    {
        _cache.GetEntity(Constants.Cache.Languages.DefaultLangaugeKey, () => GetDefaultLanguage());
    }

    private ILanguage GetDefaultLanguage()
    {
        UmbracoLanguage? umbracoLangauge = Context.UmbracoLanguages.FirstOrDefault(x => x.IsDefaultVariantLang.HasValue && x.IsDefaultVariantLang.Value);

        if (umbracoLangauge == null)
        {
            _logger.LogError("No default language found");
            throw new InvalidOperationException("No default language found");
        }

        if (umbracoLangauge.IsDefaultVariantLang == null)
        {
            _logger.LogError("The returned default language does not have a value for IsDefaultVariantLang");
            throw new InvalidOperationException("The returned default language does not have a value for IsDefaultVariantLang");
        }

        var cultureName = umbracoLangauge.LanguageCultureName;

        if (string.IsNullOrEmpty(cultureName))
        {
            _logger.LogError("The returned default language {languageId} does not have a value for LanguageCultureName", umbracoLangauge.Id);
            cultureName = string.Empty;
        }

        var isoCode = umbracoLangauge.LanguageIsocode;

        if (string.IsNullOrEmpty(isoCode))
        {
            _logger.LogError("The returned default language {languageId} does not have a value for LanguageIsocode", umbracoLangauge.Id);
            isoCode = string.Empty;
        }

        return new Language(cultureName, umbracoLangauge.FallbackLanguage?.LanguageIsocode, umbracoLangauge.IsDefaultVariantLang.Value, isoCode);
    }

    public int? GetIdByIsoCode(string isoCode) => throw new NotImplementedException();
    public List<string> GetLanguageNames(params string[] isoCodes) => throw new NotImplementedException();
    public List<ILanguage> GetLanguages(params string[] isoCodes) => throw new NotImplementedException();
}
