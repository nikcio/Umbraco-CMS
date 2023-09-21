using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Serilog;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Persistence.EFCore;

namespace Umbraco.Cms.Web.UI
{
    public class MyStartupHandler : INotificationAsyncHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IOpenIddictApplicationManager _a;
        private readonly UmbracoDbContext _dbContext;

        public MyStartupHandler(IOpenIddictApplicationManager a, UmbracoDbContext dbContext)
        {
            Log.Error("async notification");
            _a = a;
            _dbContext = dbContext;
            var conn = _dbContext.Database.GetConnectionString();
            Log.Error(conn ?? string.Empty);
        }

        public Task HandleAsync(UmbracoApplicationStartingNotification notification, CancellationToken cancellationToken)
        {


            return Task.CompletedTask;
        }
    }

    public class MyStartupHandlerSync : INotificationHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IOpenIddictApplicationManager _a;
        private readonly UmbracoDbContext _dbContext;

        public MyStartupHandlerSync(IOpenIddictApplicationManager a, UmbracoDbContext dbContext)
        {
            Log.Error("sync notification");
            _a = a;
            _dbContext = dbContext;
            var conn = _dbContext.Database.GetConnectionString();
            Log.Error(conn ?? string.Empty);
        }

        public void Handle(UmbracoApplicationStartingNotification notification)
        {
        }
    }


    public class MyStartupHandlerSyncStarted : INotificationHandler<UmbracoApplicationStartedNotification>
    {
        private readonly IOpenIddictApplicationManager _a;
        private readonly UmbracoDbContext _dbContext;

        public MyStartupHandlerSyncStarted(IOpenIddictApplicationManager a, UmbracoDbContext dbContext)
        {
            Log.Error("started sync notification");
            _a = a;
            _dbContext = dbContext;
            var conn = _dbContext.Database.GetConnectionString();
            Log.Error(conn ?? string.Empty);
        }

        public void Handle(UmbracoApplicationStartedNotification notification) { }
    }
}
