using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Cache;
using Umbraco.Extensions;

namespace Umbraco.Cms.Infrastructure.Composers;

public class InfrastructureComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddTransient(typeof(IEntityCache<>), typeof(LegacyEntityCache<>));

        builder.Services.AddUnique<IContentService, ContentService>(ServiceLifetime.Transient);
    }
}
