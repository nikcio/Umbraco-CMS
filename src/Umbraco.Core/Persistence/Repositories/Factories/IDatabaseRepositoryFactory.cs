namespace Umbraco.Cms.Core.Persistence.Repositories.Factories;

/// <summary>
/// A factory used to create database repositories.
/// </summary>
public interface IDatabaseRepositoryFactory
{
    /// <summary>
    /// Creates a repository with its own database connection.
    /// </summary>
    /// <remarks>
    /// Use <see cref="CreateRepository(IDatabaseUnitOfWork)"/> if you want to use the same database connection as the unit of work.
    /// </remarks>
    /// <typeparam name="TRepository">The repository to create.</typeparam>
    /// <returns>The created <typeparamref name="TRepository"/>.</returns>
    TRepository CreateRepository<TRepository>()
        where TRepository : class, IDatabaseRepository;

    /// <summary>
    /// Creates a repository for the specified <paramref name="unitOfWork"/>.
    /// </summary>
    /// <remarks>
    /// Sharing the database connection between the <paramref name="unitOfWork"/> and the repository is useful when you want to share the same transaction and state in the database connection.
    /// </remarks>
    /// <param name="unitOfWork">The <see cref="IDatabaseUnitOfWork"/> to share a database connection with.</param>
    /// <typeparam name="TRepository">The repository to create.</typeparam>
    /// <returns>The created <typeparamref name="TRepository"/>.</returns>
    TRepository CreateRepository<TRepository>(IDatabaseUnitOfWork unitOfWork)
        where TRepository : class, IDatabaseRepository;

    /// <summary>
    /// Creates a repository for the specified other <paramref name="repository"/>.
    /// </summary>
    /// <remarks>
    /// Sharing the database connection between the unit of work and the repository is useful when you want to share the same transaction and state in the database connection.
    /// </remarks>
    /// <param name="repository">The <see cref="IDatabaseUnitOfWork"/> to share a database connection with.</param>
    /// <typeparam name="TRepository">The repository to create.</typeparam>
    /// <returns>The created <typeparamref name="TRepository"/>.</returns>
    TRepository CreateRepository<TRepository>(IDatabaseRepository repository)
        where TRepository : class, IDatabaseRepository;
}
