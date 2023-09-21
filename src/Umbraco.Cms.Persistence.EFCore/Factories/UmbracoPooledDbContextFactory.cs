using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Umbraco.Cms.Persistence.EFCore.Factories;

/// <inheritdoc/>
internal class UmbracoPooledDbContextFactory<TContext> : PooledDbContextFactory<TContext>
    where TContext : DbContext
{
    private bool _umbracoInstalled = false;
    private readonly DbContextOptions<TContext> _options;

    /// <inheritdoc/>
    public UmbracoPooledDbContextFactory(DbContextOptions<TContext> options, int poolSize = 1024) : base(options, poolSize)
    {
        _options = options;
    }

    /// <inheritdoc/>
    public override TContext CreateDbContext()
    {
        if (_umbracoInstalled)
        {
            return base.CreateDbContext();
        }
        else
        {
            return (TContext?)Activator.CreateInstance(typeof(TContext), _options) ?? throw new InvalidOperationException("Unable to create DbContext");
        }
    }

    /// <inheritdoc/>
    public override async Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        if (_umbracoInstalled)
        {
            return await base.CreateDbContextAsync(cancellationToken);
        }
        else
        {
            return (TContext?)Activator.CreateInstance(typeof(TContext), _options) ?? throw new InvalidOperationException("Unable to create DbContext");
        }
    }

    internal void SetUmbracoInstalled()
    {
        _umbracoInstalled = true;
    }
}

public class UmbracoStartingHandler : INotificationHandler<UmbracoApplicationStartingNotification>
{
    private readonly IOptionsSnapshot<ConnectionStrings> _connectionStringsOptions;
    private readonly IDbContextFactory<UmbracoDbContext> _contextFactory;

    public UmbracoStartingHandler(IOptionsSnapshot<ConnectionStrings> connectionStringsOptions, IDbContextFactory<UmbracoDbContext> contextFactory)
    {
        _connectionStringsOptions = connectionStringsOptions;
        _contextFactory = contextFactory;
    }

    public void Handle(UmbracoApplicationStartingNotification notification)
    {
        if (!string.IsNullOrEmpty(_connectionStringsOptions.Value.ProviderName) && !string.IsNullOrEmpty(_connectionStringsOptions.Value.ConnectionString))
        {
            if (_contextFactory is UmbracoPooledDbContextFactory<UmbracoDbContext> pooledDbContextFactory)
            {
                pooledDbContextFactory.SetUmbracoInstalled();
            }
        }
    }
}
