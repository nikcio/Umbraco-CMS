using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

/// <summary>
/// Represents the LangaugeRepository for doing CRUD operations for <see cref="ILanguage"/>.
/// </summary>
public interface IDatabaseLanguageRepository : IDatabaseRepository
{
    /// <summary>
    /// Gets a language identifier from its ISO code.
    /// </summary>
    /// <param name="isoCode">The ISO code for the language</param>
    /// <returns>The language identifier for the <paramref name="isoCode"/></returns>
    int? GetIdByIsoCode(string isoCode);

    /// <summary>
    /// Gets the default language ISO code.
    /// </summary>
    string GetDefaultIsoCode();

    /// <summary>
    /// Gets languages by their ISO codes.
    /// </summary>
    /// <param name="isoCodes"></param>
    /// <returns></returns>
    List<ILanguage> GetLanguages(params string[] isoCodes);

    /// <summary>
    /// Gets language names by their ISO codes.
    /// </summary>
    /// <param name="isoCodes"></param>
    /// <returns></returns>
    List<string> GetLanguageNames(params string[] isoCodes);
}
