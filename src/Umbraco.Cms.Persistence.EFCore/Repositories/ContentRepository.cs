using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Persistence.Repositories;
using Umbraco.Cms.Core.Persistence.Repositories.Factories;
using Umbraco.Cms.Infrastructure.Persistence.Factories;
using Umbraco.Cms.Infrastructure.Persistence.Models;
using Umbraco.Cms.Infrastructure.Persistence.Repositories.Implement;
using Umbraco.Cms.Persistence.EFCore.DbContexts;
using Umbraco.Extensions;

namespace Umbraco.Cms.Persistence.EFCore.Repositories;

/// <summary>
/// Represents the ContentRepository for doing CRUD operations for <see cref="IContent"/>.
/// </summary>
internal class ContentRepository : EFCoreDatabaseRepositoryBase, IDatabaseContentRepository
{
    private readonly IDatabaseLanguageRepository _languageRepository;
    private readonly IDatabaseContentTagRepository _contentTagRepository;
    private readonly IDatabaseContentRelationRepository _contentRelationRepository;
    private readonly IDatabaseAuditRepository _auditRepository;
    private readonly ILogger<ContentRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentRepository"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="databaseRepositoryFactory"></param>
    /// <param name="logger"></param>
    internal ContentRepository(
        UmbracoDbContext dbContext,
        IDatabaseRepositoryFactory databaseRepositoryFactory,
        ILogger<ContentRepository> logger)
        : base(dbContext)
    {
        _languageRepository = databaseRepositoryFactory.CreateRepository<IDatabaseLanguageRepository>(this);
        _contentTagRepository = databaseRepositoryFactory.CreateRepository<IDatabaseContentTagRepository>(this);
        _contentRelationRepository = databaseRepositoryFactory.CreateRepository<IDatabaseContentRelationRepository>(this);
        _auditRepository = databaseRepositoryFactory.CreateRepository<IDatabaseAuditRepository>(this);
        _logger = logger;
    }

    /// <inheritdoc/>
    public IContent SaveContent(IContent contentItem)
    {
        if (contentItem.HasIdentity)
        {
            return SaveUpdatedContent(contentItem);
        }
        else
        {
            return SaveNewContent(contentItem);
        }
    }

    private IContent SaveUpdatedContent(IContent contentItem)
    {
        return contentItem;
    }

    private IContent SaveNewContent(IContent contentItem)
    {
        EnsureInvariantNameIsUnique(contentItem);
        EnsureVariantNamesAreUnique(contentItem, contentItem.PublishedState == PublishedState.Publishing);

        UmbracoNode? parent = Context.UmbracoNodes.FirstOrDefault(node => node.Id == contentItem.ParentId);

        if (parent == null)
        {
            _logger.LogError("Unable to find parent with id {ParentId}", contentItem.ParentId);
            throw new InvalidOperationException($"Unable to find parent with id {contentItem.ParentId}");
        }

        contentItem.Level = parent.Level + 1;

        bool sortOrderExists = Context.UmbracoNodes
            .Select(node => new
            {
                node.SortOrder,
                node.ParentId,
                node.NodeObjectType,
            })
            .Where(node =>
                node.NodeObjectType == Constants.ObjectTypes.Document &&
                node.ParentId == contentItem.ParentId &&
                node.SortOrder >= contentItem.SortOrder)
            .Any();

        if (sortOrderExists)
        {
            int newSortOrder = Context.UmbracoNodes
                .Select(node => new
                {
                    node.SortOrder,
                    node.ParentId,
                    node.NodeObjectType,
                })
                .Where(node =>
                    node.NodeObjectType == Constants.ObjectTypes.Document &&
                    node.ParentId == contentItem.ParentId)
                .Max(node => node.SortOrder) + 1;
            contentItem.SortOrder = newSortOrder;
        }

        int? reservedId = Context.UmbracoNodes
            .Select(node => new
            {
                node.Id,
                node.UniqueId,
                node.NodeObjectType,
            })
            .Where(node =>
                node.NodeObjectType == Constants.ObjectTypes.IdReservation &&
                node.UniqueId == contentItem.Key)
            .Select(node => node.Id)
            .FirstOrDefault();

        if (reservedId != null)
        {
            contentItem.Id = reservedId.Value;
            contentItem.Path = string.Concat(parent.Path, ",", reservedId);
        }
        else
        {
            contentItem.Path = parent.Path;
        }

        UmbracoDocument umbracoDocument = ContentFactory.BuildUmbracoDocument(contentItem);

        if (reservedId == null)
        {
            Context.UmbracoNodes.Add(umbracoDocument.Node.Node);
            Context.SaveChanges();
            umbracoDocument.Node.Node.Path = string.Concat(parent.Path, ",", umbracoDocument.Node.Node.Id);
            umbracoDocument.NodeId = umbracoDocument.Node.Node.Id;

            contentItem.Id = umbracoDocument.Node.Node.Id;
            contentItem.Path = umbracoDocument.Node.Node.Path;
        }

        umbracoDocument.Node.Node.Path.ValidatePathWithException(parent.Id);
        umbracoDocument.Node.NodeId = umbracoDocument.Node.Node.Id;

        umbracoDocument.Node.UmbracoContentVersions.Add(ContentFactory.BuildUmbracoContentVersion(contentItem));

        IEnumerable<UmbracoPropertyDatum> umbracoPropertyData = PropertyFactory.BuildUmbracoPropertyDatums(contentItem, _languageRepository, out var edited, out HashSet<string>? editedCultures);

        if (contentItem.PublishedState != PublishedState.Publishing && contentItem.PublishName != contentItem.Name)
        {
            edited = true;
        }

        umbracoDocument.Edited = edited;
        contentItem.Edited = !umbracoDocument.Published || edited;

        if (contentItem.ContentType.VariesByCulture())
        {
            if (contentItem.CultureInfos != null)
            {
                editedCultures ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (ContentCultureInfos cultureInfo in contentItem.CultureInfos)
                {
                    if (cultureInfo.Name != contentItem.GetPublishName(cultureInfo.Culture))
                    {
                        editedCultures.Add(cultureInfo.Culture);
                    }
                }
            }

            contentItem.SetCultureEdited(editedCultures);

            contentItem.AdjustDates(umbracoDocument.Node.UmbracoContentVersions.First().VersionDate, contentItem.PublishedState == PublishedState.Publishing);

            Context.UmbracoContentVersionCultureVariations.AddRange(ContentFactory.BuildUmbracoContentVersionCultureVariations(contentItem, _languageRepository));

            Context.UmbracoDocumentCultureVariations.AddRange(ContentFactory.BuildUmbracoDocumentCultureVariations(contentItem, editedCultures, _languageRepository));
        }

        Context.UmbracoPropertyData.AddRange(umbracoPropertyData);

        Context.UmbracoDocuments.Add(umbracoDocument);

        Context.SaveChanges();

        contentItem.VersionId = umbracoDocument.Node.UmbracoContentVersions.First().Id;

        if (contentItem.PublishedState == PublishedState.Publishing)
        {
            contentItem.PublishedVersionId = contentItem.VersionId;
            contentItem.Published = true;
            contentItem.PublishTemplateId = contentItem.TemplateId;
            contentItem.PublisherId = contentItem.WriterId;
            contentItem.PublishName = contentItem.Name;
            contentItem.PublishDate = contentItem.UpdateDate;

            _contentTagRepository.SetContentTags(contentItem);
        }

        _contentRelationRepository.SaveContentRelations(contentItem);

        contentItem.ResetDirtyProperties();

        if (contentItem.ContentType.VariesByCulture() && editedCultures != null)
        {
            var languages = string.Join(", ", _languageRepository.GetLanguageNames(editedCultures.ToArray()));
            _auditRepository.SaveAudit(new AuditItem(contentItem.Id, AuditType.SaveVariant, contentItem.WriterId, UmbracoObjectTypes.Document.GetName(), $"Saved languages: {languages}", languages));
        }
        else
        {
            _auditRepository.SaveAudit(new AuditItem(contentItem.Id, AuditType.Save, contentItem.WriterId, UmbracoObjectTypes.Document.GetName(), $"Saved '{contentItem.Name}'", contentItem.Name));
        }

        return contentItem;
    }

    private void EnsureInvariantNameIsUnique(IContent contentItem)
    {
        IEnumerable<SimilarNodeName> similarNames = Context.UmbracoNodes
            .Select(node => new
            {
                node.Id,
                node.Text,
                node.NodeObjectType,
                node.ParentId,
            })
            .Where(node => node.NodeObjectType == Constants.ObjectTypes.Document && node.ParentId == contentItem.ParentId)
            .AsEnumerable() // This is where the query is executed
            .Where(node => node.Text != null)
            .Select(node => new SimilarNodeName(node.Id, node.Text!));

        contentItem.Name = SimilarNodeName.GetUniqueName(similarNames, contentItem.Id, contentItem.Name);
    }

    private void EnsureVariantNamesAreUnique(IContent contentItem, bool publishing)
    {
        if (!contentItem.ContentType.VariesByCulture() || contentItem.CultureInfos?.Count == 0)
        {
            return;
        }

        // Using values like this is more performant than using the values directly in the where clause
        var currentValue = true;
        Guid nodeObjectType = Constants.ObjectTypes.Document;
        var names = Context.UmbracoContentVersionCultureVariations
            .Include(x => x.Version)
            .ThenInclude(x => x.Node)
            .ThenInclude(x => x.Node)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.LanguageId,
                Version = new
                {
                    x.Version.Current,
                    Node = new
                    {
                        Node = new
                        {
                            x.Version.Node.Node.NodeObjectType,
                            x.Version.Node.Node.ParentId,
                            x.Version.Node.Node.Id,
                        },
                    },
                },
            })
            .Where(x => x.Version.Current == currentValue &&
                        x.Version.Node.Node.NodeObjectType == nodeObjectType &&
                        x.Version.Node.Node.ParentId == contentItem.ParentId &&
                        x.Version.Node.Node.Id == contentItem.Id)
            .AsEnumerable() // This is where the query is executed
            .Select(x => new CultureNodeName(x.Id, x.Name, x.LanguageId))
            .GroupBy(x => x.LanguageId)
            .ToDictionary(x => x.Key, x => x);

        if (names.Count == 0)
        {
            return;
        }

        // Note: the code below means we are going to unique-ify every culture names, regardless
        // of whether the name has changed (ie the culture has been updated) - some saving culture
        // fr-FR could cause culture en-UK name to change - not sure that is clean
        if (contentItem.CultureInfos is null)
        {
            return;
        }

        foreach (ContentCultureInfos cultureInfo in contentItem.CultureInfos)
        {
            var langId = _languageRepository.GetIdByIsoCode(cultureInfo.Culture);
            if (!langId.HasValue)
            {
                continue;
            }

            if (!names.TryGetValue(langId.Value, out IGrouping<int, CultureNodeName>? cultureNames))
            {
                continue;
            }

            IEnumerable<SimilarNodeName> otherNames = cultureNames.Select(x => new SimilarNodeName(x.Id, x.Name));
            var uniqueName = SimilarNodeName.GetUniqueName(otherNames, 0, cultureInfo.Name);

            if (uniqueName == contentItem.GetCultureName(cultureInfo.Culture))
            {
                continue;
            }

            contentItem.SetCultureName(uniqueName, cultureInfo.Culture);
            if (publishing && (contentItem.PublishCultureInfos?.ContainsKey(cultureInfo.Culture) ?? false))
            {
                contentItem.SetPublishInfo(cultureInfo.Culture, uniqueName, DateTime.Now); //TODO: This is weird, this call will have already been made in the SetCultureName
            }
        }
    }

    private sealed class CultureNodeName
    {
        public CultureNodeName(int id, string name, int languageId)
        {
            Id = id;
            Name = name;
            LanguageId = languageId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int LanguageId { get; set; }
    }
}
