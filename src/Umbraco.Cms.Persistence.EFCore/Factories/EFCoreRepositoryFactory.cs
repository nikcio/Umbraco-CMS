using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Persistence;
using Umbraco.Cms.Core.Persistence.Repositories;
using Umbraco.Cms.Core.Persistence.Repositories.Factories;
using Umbraco.Cms.Persistence.EFCore.DbContexts;
using Umbraco.Cms.Persistence.EFCore.Repositories;

namespace Umbraco.Cms.Persistence.EFCore.Factories;

internal class EFCoreRepositoryFactory : IDatabaseRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDbContextFactory<UmbracoDbContext> _dbContextFactory;

    private readonly Dictionary<Type, Func<IServiceProvider, UmbracoDbContext, IDatabaseRepository>> _registeredRepositories = new();

    public EFCoreRepositoryFactory(IServiceProvider serviceProvider, IDbContextFactory<UmbracoDbContext> dbContextFactory)
    {
        _serviceProvider = serviceProvider;
        _dbContextFactory = dbContextFactory;

        RegisterRepository<IDatabaseContentRepository>((provider, dbContext) => new ContentRepository(dbContext, provider.GetRequiredService<IDatabaseRepositoryFactory>(), provider.GetRequiredService<ILogger<ContentRepository>>()));
    }

    public TRepository CreateRepository<TRepository>()
        where TRepository : class, IDatabaseRepository
    {
        if (_registeredRepositories.TryGetValue(typeof(TRepository), out Func<IServiceProvider, UmbracoDbContext, IDatabaseRepository>? factory))
        {
            return (TRepository)factory(_serviceProvider, _dbContextFactory.CreateDbContext());
        }
        else
        {
            throw new InvalidOperationException($"No repository registered for {typeof(TRepository)}");
        }
    }

    public TRepository CreateRepository<TRepository>(IDatabaseUnitOfWork unitOfWork)
        where TRepository : class, IDatabaseRepository
    {
        var databaseConnection = unitOfWork.GetDatabaseConnection();

        if (databaseConnection is not UmbracoDbContext dbContext)
        {
            throw new InvalidOperationException($"The unit of work isn't using the same database connection as required for {typeof(TRepository)} ({typeof(UmbracoDbContext)})");
        }

        if (_registeredRepositories.TryGetValue(typeof(TRepository), out Func<IServiceProvider, UmbracoDbContext, IDatabaseRepository>? factory))
        {
            return (TRepository)factory(_serviceProvider, dbContext);
        }
        else
        {
            throw new InvalidOperationException($"No repository registered for {typeof(TRepository)}");
        }
    }

    public TRepository CreateRepository<TRepository>(IDatabaseRepository repository)
        where TRepository : class, IDatabaseRepository
    {
        var databaseConnection = repository.GetDatabaseConnection();

        if (databaseConnection is not UmbracoDbContext dbContext)
        {
            throw new InvalidOperationException($"The unit of work isn't using the same database connection as required for {typeof(TRepository)} ({typeof(UmbracoDbContext)})");
        }

        if (_registeredRepositories.TryGetValue(typeof(TRepository), out Func<IServiceProvider, UmbracoDbContext, IDatabaseRepository>? factory))
        {
            return (TRepository)factory(_serviceProvider, dbContext);
        }
        else
        {
            throw new InvalidOperationException($"No repository registered for {typeof(TRepository)}");
        }
    }

    private void RegisterRepository<TRepository>(Func<IServiceProvider, UmbracoDbContext, IDatabaseRepository> factory)
        where TRepository : IDatabaseRepository
    {
        _registeredRepositories[typeof(TRepository)] = factory;
    }
}
