using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Infrastructure.Persistence;

namespace Umbraco.Cms.Infrastructure.Scoping;

/// <summary>
/// Represents a database a scope.
/// </summary>
public class DatabaseScope : NotificationScope, IDatabaseScope
{
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseScope"/> class.
    /// </summary>
    /// <param name="parentScope"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="scopedNotificationPublisher"></param>
    /// <param name="umbracoDatabase"></param>
    public DatabaseScope(
        IBaseScope? parentScope,
        ILoggerFactory loggerFactory,
        IScopedNotificationPublisher scopedNotificationPublisher,
        IUmbracoDatabase umbracoDatabase)
        : base(parentScope, loggerFactory, scopedNotificationPublisher)
    {
        UmbracoDatabase = umbracoDatabase;
    }

    /// <inheritdoc/>
    public IUmbracoDatabase UmbracoDatabase { get; }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing && UmbracoDatabase.InTransaction)
        {
            CompleteDatabaseTransation();
        }

        base.Dispose(disposing);

        _disposedValue = true;
    }

    private void CompleteDatabaseTransation()
    {
        try
        {
            UmbracoDatabase.CompleteTransaction();
        }
        catch (Exception ex)
        {
            LoggerFactory.CreateLogger<DatabaseScope>().LogError(ex, "Error completing transaction");
            UmbracoDatabase.AbortTransaction();
        }
    }
}
