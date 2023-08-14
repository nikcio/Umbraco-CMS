using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models.Entities;
using Umbraco.Cms.Infrastructure.Persistence.Repositories;
using Umbraco.Cms.Persistence.EFCore.DbContexts;

namespace Umbraco.Cms.Persistence.EFCore.Repositories;

/// <summary>
/// Represents the base class for all EFCore database repositories.
/// </summary>
internal abstract class EFCoreDatabaseRepositoryBase : DatabaseRepositoryBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EFCoreDatabaseRepositoryBase"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    protected EFCoreDatabaseRepositoryBase(UmbracoDbContext dbContext)
    {
        Context = dbContext;
    }

    /// <summary>
    /// Gets the database context.
    /// </summary>
    protected UmbracoDbContext Context { get; }

    /// <inheritdoc/>
    public override object GetDatabaseConnection() => Context;
}
