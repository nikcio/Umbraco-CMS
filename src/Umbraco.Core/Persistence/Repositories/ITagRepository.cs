using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface ITagRepository : IReadWriteQueryRepository<int, ITag>
{
    #region Assign and Remove Tags

    /// <summary>
    ///     Assign tags to a content property.
    /// </summary>
    /// <param name="contentId">The identifier of the content item.</param>
    /// <param name="propertyTypeId">The identifier of the property type.</param>
    /// <param name="tags">The tags to assign.</param>
    /// <param name="replaceTags">A value indicating whether to replace already assigned tags.</param>
    /// <remarks>
    ///     <para>
    ///         When <paramref name="replaceTags" /> is false, the tags specified in <paramref name="tags" /> are added to
    ///         those already assigned.
    ///     </para>
    ///     <para>
    ///         When <paramref name="tags" /> is empty and <paramref name="replaceTags" /> is true, all assigned tags are
    ///         removed.
    ///     </para>
    /// </remarks>
    // TODO: replaceTags is used as 'false' in tests exclusively - should get rid of it
    void Assign(int contentId, int propertyTypeId, IEnumerable<ITag> tags, bool replaceTags = true);

    /// <summary>
    ///     Assign tags to a content property.
    /// </summary>
    /// <param name="contentId">The identifier of the content item.</param>
    /// <param name="propertyTypeId">The identifier of the property type.</param>
    /// <param name="tags">The tags to assign.</param>
    /// <param name="replaceTags">A value indicating whether to replace already assigned tags.</param>
    /// <remarks>
    ///     <para>
    ///         When <paramref name="replaceTags" /> is false, the tags specified in <paramref name="tags" /> are added to
    ///         those already assigned.
    ///     </para>
    ///     <para>
    ///         When <paramref name="tags" /> is empty and <paramref name="replaceTags" /> is true, all assigned tags are
    ///         removed.
    ///     </para>
    /// </remarks>
    // TODO: replaceTags is used as 'false' in tests exclusively - should get rid of it
    Task AssignAsync(int contentId, int propertyTypeId, IEnumerable<ITag> tags, bool replaceTags = true, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Removes assigned tags from a content property.
    /// </summary>
    /// <param name="contentId">The identifier of the content item.</param>
    /// <param name="propertyTypeId">The identifier of the property type.</param>
    /// <param name="tags">The tags to remove.</param>
    void Remove(int contentId, int propertyTypeId, IEnumerable<ITag> tags);

    /// <summary>
    ///     Removes assigned tags from a content property.
    /// </summary>
    /// <param name="contentId">The identifier of the content item.</param>
    /// <param name="propertyTypeId">The identifier of the property type.</param>
    /// <param name="tags">The tags to remove.</param>
    Task RemoveAsync(int contentId, int propertyTypeId, IEnumerable<ITag> tags, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Removes all assigned tags from a content item.
    /// </summary>
    /// <param name="contentId">The identifier of the content item.</param>
    void RemoveAll(int contentId);

    /// <summary>
    ///     Removes all assigned tags from a content item.
    /// </summary>
    /// <param name="contentId">The identifier of the content item.</param>
    Task RemoveAllAsync(int contentId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Removes all assigned tags from a content property.
    /// </summary>
    /// <param name="contentId">The identifier of the content item.</param>
    /// <param name="propertyTypeId">The identifier of the property type.</param>
    void RemoveAll(int contentId, int propertyTypeId);

    /// <summary>
    ///     Removes all assigned tags from a content property.
    /// </summary>
    /// <param name="contentId">The identifier of the content item.</param>
    /// <param name="propertyTypeId">The identifier of the property type.</param>
    Task RemoveAllAsync(int contentId, int propertyTypeId, CancellationToken? cancellationToken = null);

    #endregion

    #region Queries

    /// <summary>
    ///     Gets a tagged entity.
    /// </summary>
    TaggedEntity? GetTaggedEntityByKey(Guid key);

    /// <summary>
    ///     Gets a tagged entity.
    /// </summary>
    Task<TaggedEntity?> GetTaggedEntityByKeyAsync(Guid key, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets a tagged entity.
    /// </summary>
    TaggedEntity? GetTaggedEntityById(int id);

    /// <summary>
    ///     Gets a tagged entity.
    /// </summary>
    Task<TaggedEntity?> GetTaggedEntityByIdAsync(int id, CancellationToken? cancellationToken = null);

    /// Gets all entities of a type, tagged with any tag in the specified group.
    IEnumerable<TaggedEntity> GetTaggedEntitiesByTagGroup(TaggableObjectTypes objectType, string group, string? culture = null);

    /// Gets all entities of a type, tagged with any tag in the specified group.
    Task<IEnumerable<TaggedEntity>> GetTaggedEntitiesByTagGroupAsync(TaggableObjectTypes objectType, string group, string? culture = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets all entities of a type, tagged with the specified tag.
    /// </summary>
    IEnumerable<TaggedEntity> GetTaggedEntitiesByTag(TaggableObjectTypes objectType, string tag, string? group = null, string? culture = null);

    /// <summary>
    ///     Gets all entities of a type, tagged with the specified tag.
    /// </summary>
    Task<IEnumerable<TaggedEntity>> GetTaggedEntitiesByTagAsync(TaggableObjectTypes objectType, string tag, string? group = null, string? culture = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets all tags for an entity type.
    /// </summary>
    IEnumerable<ITag> GetTagsForEntityType(TaggableObjectTypes objectType, string? group = null, string? culture = null);

    /// <summary>
    ///     Gets all tags for an entity type.
    /// </summary>
    Task<IEnumerable<ITag>> GetTagsForEntityTypeAsync(TaggableObjectTypes objectType, string? group = null, string? culture = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets all tags attached to an entity.
    /// </summary>
    IEnumerable<ITag> GetTagsForEntity(int contentId, string? group = null, string? culture = null);

    /// <summary>
    ///     Gets all tags attached to an entity.
    /// </summary>
    Task<IEnumerable<ITag>> GetTagsForEntityAsync(int contentId, string? group = null, string? culture = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets all tags attached to an entity.
    /// </summary>
    IEnumerable<ITag> GetTagsForEntity(Guid contentId, string? group = null, string? culture = null);

    /// <summary>
    ///     Gets all tags attached to an entity.
    /// </summary>
    Task<IEnumerable<ITag>> GetTagsForEntityAsync(Guid contentId, string? group = null, string? culture = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets all tags attached to an entity via a property.
    /// </summary>
    IEnumerable<ITag> GetTagsForProperty(int contentId, string propertyTypeAlias, string? group = null, string? culture = null);

    /// <summary>
    ///     Gets all tags attached to an entity via a property.
    /// </summary>
    Task<IEnumerable<ITag>> GetTagsForPropertyAsync(int contentId, string propertyTypeAlias, string? group = null, string? culture = null, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets all tags attached to an entity via a property.
    /// </summary>
    IEnumerable<ITag> GetTagsForProperty(Guid contentId, string propertyTypeAlias, string? group = null, string? culture = null);

    /// <summary>
    ///     Gets all tags attached to an entity via a property.
    /// </summary>
    Task<IEnumerable<ITag>> GetTagsForPropertyAsync(Guid contentId, string propertyTypeAlias, string? group = null, string? culture = null, CancellationToken? cancellationToken = null);

    #endregion
}
