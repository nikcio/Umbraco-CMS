using System.Data;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Infrastructure.Persistence;

namespace Umbraco.Cms.Infrastructure.Scoping;

/// <summary>
/// A database scope provider that creates <see cref="DatabaseScope"/> instances.
/// </summary>
internal class DatabaseScopeProvider : IDatabaseScopeProvider
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IScopedNotificationPublisher _scopedNotificationPublisher;
    private readonly ILogger<DatabaseScopeProvider> _logger;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseScopeProvider"/> class.
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="scopedNotificationPublisher"></param>
    /// <param name="umbracoDatabaseFactory"></param>
    public DatabaseScopeProvider(ILoggerFactory loggerFactory, IScopedNotificationPublisher scopedNotificationPublisher, IUmbracoDatabaseFactory umbracoDatabaseFactory)
    {
        _loggerFactory = loggerFactory;
        _scopedNotificationPublisher = scopedNotificationPublisher;
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _logger = loggerFactory.CreateLogger<DatabaseScopeProvider>();
    }

    /// <inheritdoc/>
    public IDatabaseScope CreateScope(IDatabaseScope? parentScope = null, IsolationLevel? isolationLevel = null)
    {
        IUmbracoDatabase database = parentScope?.UmbracoDatabase ?? _umbracoDatabaseFactory.CreateDatabase();

        if (isolationLevel.HasValue && !database.InTransaction)
        {
            database.BeginTransaction(isolationLevel.Value);
        }
        else if (database.InTransaction)
        {
            throw new InvalidOperationException($"Cannot start new transaction with isolation level {isolationLevel} when there is already an active transaction");
        }

        var scope = new DatabaseScope(parentScope, _loggerFactory, _scopedNotificationPublisher, database);
        _logger.LogDebug("Created scope {ScopeId} - {ScopeType}", scope.InstanceId, scope.GetType());
        return scope;
    }
}
