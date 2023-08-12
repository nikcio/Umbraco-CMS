using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DistributedLocking;
using Umbraco.Cms.Persistence.EFCore.Locking;
using Umbraco.Cms.Persistence.EFCore.Migrations;
using Umbraco.Cms.Persistence.EFCore.Scoping;

namespace Umbraco.Extensions;

public static class UmbracoEFCoreServiceCollectionExtensions
{
    public static IServiceCollection AddUmbracoEFCoreContext<T>(this IServiceCollection services)
        where T : DbContext
    {
        services.AddPooledDbContextFactory<T>(SetupDbContext);

        services.AddUnique<IAmbientEFCoreScopeStack<T>, AmbientEFCoreScopeStack<T>>();
        services.AddUnique<IEFCoreScopeAccessor<T>, EFCoreScopeAccessor<T>>();
        services.AddUnique<IEFCoreScopeProvider<T>, EFCoreScopeProvider<T>>();
        services.AddSingleton<IDistributedLockingMechanism, SqliteEFCoreDistributedLockingMechanism<T>>();
        services.AddSingleton<IDistributedLockingMechanism, SqlServerEFCoreDistributedLockingMechanism<T>>();

        return services;
    }

    private static void SetupDbContext(IServiceProvider provider, DbContextOptionsBuilder builder)
    {
        ConnectionStrings connectionStrings = GetConnectionStringAndProviderName(provider);
        IEnumerable<IMigrationProviderSetup> migrationProviders = provider.GetServices<IMigrationProviderSetup>();

        IMigrationProviderSetup? migrationProvider = migrationProviders.FirstOrDefault(x => x.ProviderName == connectionStrings.ProviderName);
        migrationProvider?.Setup(builder, connectionStrings.ConnectionString);
    }

    private static ConnectionStrings GetConnectionStringAndProviderName(IServiceProvider serviceProvider)
    {
        ConnectionStrings connectionStrings = serviceProvider.GetRequiredService<IOptionsMonitor<ConnectionStrings>>().CurrentValue;

        // Replace data directory
        string? dataDirectory = AppDomain.CurrentDomain.GetData(Constants.System.DataDirectoryName)?.ToString();
        if (string.IsNullOrEmpty(dataDirectory) is false)
        {
            connectionStrings.ConnectionString = connectionStrings.ConnectionString?.Replace(Constants.System.DataDirectoryPlaceholder, dataDirectory);
        }

        return connectionStrings;
    }
}
