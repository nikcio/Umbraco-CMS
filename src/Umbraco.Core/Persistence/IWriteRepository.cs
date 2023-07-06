namespace Umbraco.Cms.Core.Persistence;

/// <summary>
///     Defines the base implementation of a writing repository.
/// </summary>
public interface IWriteRepository<in TEntity> : IRepository
{
    /// <summary>
    ///     Saves an entity.
    /// </summary>
    void Save(TEntity entity);

    /// <summary>
    ///     Saves an entity.
    /// </summary>
    Task SaveAsync(TEntity entity, CancellationToken? cancellation = null);

    /// <summary>
    ///     Deletes an entity.
    /// </summary>
    /// <param name="entity"></param>
    void Delete(TEntity entity);

    /// <summary>
    ///     Deletes an entity.
    /// </summary>
    /// <param name="entity"></param>
    Task DeleteAsync(TEntity entity, CancellationToken? cancellation = null);
}
