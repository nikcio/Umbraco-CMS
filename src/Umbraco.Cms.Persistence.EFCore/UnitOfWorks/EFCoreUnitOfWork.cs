using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.DistributedLocking;
using Umbraco.Cms.Infrastructure.Persistence.Repositories;
using Umbraco.Cms.Infrastructure.Persistence.UnitOfWorks;
using Umbraco.Cms.Persistence.EFCore.DbContexts;

namespace Umbraco.Cms.Persistence.EFCore.UnitOfWorks;

/// <summary>
/// Represents a unit of work for a specific <see cref="IDatabaseRepository"/>.
/// </summary>
internal class EFCoreUnitOfWork : UnitOfWorkBase
{
    private readonly UmbracoDbContext _dbContext;
    private readonly ILogger<EFCoreUnitOfWork> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFCoreUnitOfWork"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="lockingMechanismFactory"></param>
    public EFCoreUnitOfWork(
        ILoggerFactory loggerFactory,
        IDistributedLockingMechanismFactory lockingMechanismFactory,
        UmbracoDbContext dbContext)
        : base(loggerFactory, lockingMechanismFactory)
    {
        _dbContext = dbContext;
        _logger = loggerFactory.CreateLogger<EFCoreUnitOfWork>();
    }

    /// <inheritdoc/>
    public override void Complete()
    {
        try
        {
            var modifiedEntries = _dbContext.SaveChanges();
            _logger.LogDebug("Saved {ModifiedEntries} entries to the database", modifiedEntries);
        }
        catch (DbUpdateConcurrencyException updateConcurrencyException)
        {
            _logger.LogError(updateConcurrencyException, "Encountered concurrency error while saving changes to the database");
            throw;
        }
        catch (DbUpdateException updateException)
        {
            _logger.LogError(updateException, "Encountered update exception while saving changes to the database");
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error saving changes to the database");
            throw;
        }
        finally
        {
            LockingMechanism.ClearLocks(InstanceId);
        }
    }

    /// <inheritdoc/>
    public override object GetDatabaseConnection() => _dbContext;

    /// <inheritdoc/>
    public override void Start(IsolationLevel? isolationLevel = null)
    {
        if (isolationLevel.HasValue)
        {
            _dbContext.Database.BeginTransaction(isolationLevel.Value);
        }

        _dbContext.Database.BeginTransaction();
    }
}
