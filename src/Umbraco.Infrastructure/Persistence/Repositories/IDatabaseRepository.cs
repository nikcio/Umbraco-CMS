namespace Umbraco.Cms.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents a repository that works with the database.
/// </summary>
public interface IDatabaseRepository
{
    /// <summary>
    /// Gets the database connection.
    /// </summary>
    /// <returns>The database connection.</returns>
    object GetDatabaseConnection();
}
