using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Infrastructure.Persistence.Models;
using Umbraco.Cms.Infrastructure.Persistence.Repositories;

namespace Umbraco.Cms.Infrastructure.Persistence.Factories;

/// <summary>
/// Represents a factory for building objects from <see cref="IContent"/>.
/// </summary>
internal static class ContentFactory
{
    /// <summary>
    /// Builds a <see cref="UmbracoDocument"/> from a <see cref="IContent"/>.
    /// </summary>
    /// <param name="contentItem"></param>
    /// <returns><see cref="UmbracoDocument"/>.</returns>
    internal static UmbracoDocument BuildUmbracoDocument(IContent contentItem) => new()
    {
        NodeId = contentItem.Id,
        Published = contentItem.Published,
        Node = BuildUmbracoContent(contentItem),
    };

    /// <summary>
    /// Builds a <see cref="UmbracoContent"/> from a <see cref="IContent"/>.
    /// </summary>
    /// <param name="contentItem"></param>
    /// <returns><see cref="UmbracoContent"/>.</returns>
    internal static UmbracoContent BuildUmbracoContent(IContent contentItem) => new()
    {
        NodeId = contentItem.Id,
        ContentTypeId = contentItem.ContentTypeId,
        Node = BuildUmbracoNode(contentItem),
    };

    /// <summary>
    /// Builds a <see cref="UmbracoNode"/> from a <see cref="IContent"/>.
    /// </summary>
    /// <param name="contentItem"></param>
    /// <returns><see cref="UmbracoNode"/>.</returns>
    internal static UmbracoNode BuildUmbracoNode(IContent contentItem) => new()
    {
        Id = contentItem.Id,
        UniqueId = contentItem.Key,
        ParentId = contentItem.ParentId,
        Level = Convert.ToInt16(contentItem.Level),
        Path = contentItem.Path,
        SortOrder = contentItem.SortOrder,
        Trashed = contentItem.Trashed,
        NodeUser = contentItem.CreatorId,
        Text = contentItem.Name,
        NodeObjectType = Constants.ObjectTypes.DocumentType,
        CreateDate = contentItem.CreateDate,
    };

    /// <summary>
    /// Builds a <see cref="UmbracoDocumentVersion"/> from a <see cref="IContent"/>.
    /// </summary>
    /// <param name="contentItem"></param>
    /// <returns><see cref="UmbracoContentVersion"/>.</returns>
    internal static UmbracoDocumentVersion BuildUmbracoDocumentVersion(IContent contentItem) => new()
    {
        Id = contentItem.VersionId,
        TemplateId = contentItem.TemplateId,
        Published = contentItem.PublishedState == PublishedState.Publishing,
    };

    /// <summary>
    /// Builds a <see cref="UmbracoContentVersion"/> from a <see cref="IContent"/>.
    /// </summary>
    /// <param name="contentItem"></param>
    /// <returns><see cref="UmbracoContentVersion"/>.</returns>
    internal static UmbracoContentVersion BuildUmbracoContentVersion(IContent contentItem) => new()
    {
        NodeId = contentItem.Id,
        VersionDate = contentItem.UpdateDate,
        UserId = contentItem.WriterId,
        Current = contentItem.PublishedState == PublishedState.Publishing,
        Text = contentItem.Name,
        UmbracoDocumentVersion = BuildUmbracoDocumentVersion(contentItem),
    };

    /// <summary>
    /// Builds a <see cref="UmbracoContentVersionCultureVariation"/> from a <see cref="IContent"/>.
    /// </summary>
    /// <param name="contentItem"></param>
    /// <param name="languageRepository"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static IEnumerable<UmbracoContentVersionCultureVariation> BuildUmbracoContentVersionCultureVariations(IContent contentItem, IDatabaseLanguageRepository languageRepository)
    {
        if (contentItem.CultureInfos != null)
        {
            foreach (ContentCultureInfos cultureInfo in contentItem.CultureInfos)
            {
                yield return new UmbracoContentVersionCultureVariation
                {
                    VersionId = contentItem.VersionId,
                    LanguageId = languageRepository.GetIdByIsoCode(cultureInfo.Culture) ?? throw new InvalidOperationException($"A valid culture wasn't specified. Culture {cultureInfo.Culture}."),
                    Name = cultureInfo.Name ?? string.Empty,
                    Date = contentItem.GetUpdateDate(cultureInfo.Culture) ?? throw new InvalidOperationException($"Was unable to get an update date for content {contentItem.Id} and culture {cultureInfo.Culture}."),
                };
            }
        }

        if (contentItem.PublishedState != PublishedState.Publishing)
        {
            yield break;
        }

        if (contentItem.PublishCultureInfos != null)
        {
            foreach (ContentCultureInfos cultureInfo in contentItem.PublishCultureInfos)
            {
                yield return new UmbracoContentVersionCultureVariation
                {
                    VersionId = contentItem.PublishedVersionId,
                    LanguageId = languageRepository.GetIdByIsoCode(cultureInfo.Culture) ?? throw new InvalidOperationException($"A valid culture wasn't specified. Culture {cultureInfo.Culture}."),
                    Name = cultureInfo.Name ?? string.Empty,
                    Date = contentItem.GetPublishDate(cultureInfo.Culture) ?? throw new InvalidOperationException($"Was unable to get an update date for content {contentItem.Id} and culture {cultureInfo.Culture}."),
                };
            }
        }
    }

    /// <summary>
    /// Builds a <see cref="UmbracoDocumentCultureVariation"/> from a <see cref="IContent"/>.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="editedCultures"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static IEnumerable<UmbracoDocumentCultureVariation> BuildUmbracoDocumentCultureVariations(IContent content, HashSet<string>? editedCultures, IDatabaseLanguageRepository languageRepository)
    {
        IEnumerable<string> allCultures = content.AvailableCultures.Union(content.PublishedCultures);
        foreach (var culture in allCultures)
        {
            yield return new UmbracoDocumentCultureVariation
            {
                NodeId = content.Id,
                LanguageId = languageRepository.GetIdByIsoCode(culture) ?? throw new InvalidOperationException($"A valid culture wasn't specified. Culture {culture}."),
                Name = content.GetCultureName(culture) ?? content.GetPublishName(culture),
                Available = content.IsCultureAvailable(culture),
                Published = content.IsCulturePublished(culture),
                Edited = content.IsCultureAvailable(culture) && (!content.IsCulturePublished(culture) || (editedCultures != null && editedCultures.Contains(culture))),
            };
        }
    }
}
