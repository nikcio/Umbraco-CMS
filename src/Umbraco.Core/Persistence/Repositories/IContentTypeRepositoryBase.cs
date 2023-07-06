// Copyright (c) Umbraco.
// See LICENSE for more details.

using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IContentTypeRepositoryBase<TItem> : IReadWriteQueryRepository<int, TItem>, IReadRepository<Guid, TItem>
    where TItem : IContentTypeComposition
{
    TItem? Get(string alias);

    Task<TItem?> GetAsync(string alias, CancellationToken? cancellationToken = null);

    IEnumerable<MoveEventInfo<TItem>> Move(TItem moving, EntityContainer container);

    Task<IEnumerable<MoveEventInfo<TItem>>> MoveAsync(TItem moving, EntityContainer container, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Derives a unique alias from an existing alias.
    /// </summary>
    /// <param name="alias">The original alias.</param>
    /// <returns>The original alias with a number appended to it, so that it is unique.</returns>
    /// <remarks>Unique across all content, media and member types.</remarks>
    string GetUniqueAlias(string alias);

    /// <summary>
    ///     Derives a unique alias from an existing alias.
    /// </summary>
    /// <param name="alias">The original alias.</param>
    /// <returns>The original alias with a number appended to it, so that it is unique.</returns>
    /// <remarks>Unique across all content, media and member types.</remarks>
    Task<string> GetUniqueAliasAsync(string alias, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets a value indicating whether there is a list view content item in the path.
    /// </summary>
    /// <param name="contentPath"></param>
    /// <returns></returns>
    bool HasContainerInPath(string contentPath);

    /// <summary>
    ///     Gets a value indicating whether there is a list view content item in the path.
    /// </summary>
    /// <param name="contentPath"></param>
    /// <returns></returns>
    Task<bool> HasContainerInPathAsync(string contentPath, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Gets a value indicating whether there is a list view content item in the path.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    bool HasContainerInPath(params int[] ids);

    /// <summary>
    ///     Gets a value indicating whether there is a list view content item in the path.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<bool> HasContainerInPathAsync(CancellationToken? cancellationToken = null, params int[] ids);

    /// <summary>
    ///     Returns true or false depending on whether content nodes have been created based on the provided content type id.
    /// </summary>
    bool HasContentNodes(int id);

    /// <summary>
    ///     Returns true or false depending on whether content nodes have been created based on the provided content type id.
    /// </summary>
    Task<bool> HasContentNodesAsync(int id, CancellationToken? cancellationToken = null);
}
