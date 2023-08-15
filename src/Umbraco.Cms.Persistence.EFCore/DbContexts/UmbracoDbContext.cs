using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Infrastructure.Persistence.Models;
using Umbraco.Cms.Persistence.EFCore.Migrations;
using Umbraco.Cms.Persistence.EFCore.Models;

namespace Umbraco.Cms.Persistence.EFCore.DbContexts;

/// <remarks>
/// To autogenerate migrations use the following commands
/// and insure the 'src/Umbraco.Web.UI/appsettings.json' have a connection string set with the right provider.
///
/// Create a migration for each provider
/// <code>dotnet ef migrations add %Name% -s src/Umbraco.Web.UI -p src/Umbraco.Cms.Persistence.EFCore.SqlServer -c _umbracoDbContext  -- --provider SqlServer</code>
///
/// <code>dotnet ef migrations add %Name% -s src/Umbraco.Web.UI -p src/Umbraco.Cms.Persistence.EFCore.Sqlite -c _umbracoDbContext  -- --provider Sqlite</code>
///
/// Remove the last migration for each provider
/// <code>dotnet ef migrations remove -s src/Umbraco.Web.UI -p src/Umbraco.Cms.Persistence.EFCore.SqlServer -c _umbracoDbContext -- --provider SqlServer</code>
///
/// <code>dotnet ef migrations remove -s src/Umbraco.Web.UI -p src/Umbraco.Cms.Persistence.EFCore.Sqlite -c _umbracoDbContext -- --provider Sqlite</code>
///
/// To find documentation about this way of working with the context see
/// https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers?tabs=dotnet-core-cli#using-one-context-type
/// </remarks>
public class UmbracoDbContext : DbContext
{
    public UmbracoDbContext(DbContextOptions<UmbracoDbContext> options)
        : base(ConfigureOptions(options, out IOptionsMonitor<ConnectionStrings>? connectionStringsOptionsMonitor))
    {
        connectionStringsOptionsMonitor.OnChange(_ =>
        {
            ILogger<UmbracoDbContext> logger = StaticServiceProvider.Instance.GetRequiredService<ILogger<UmbracoDbContext>>();
            logger.LogWarning("Connection string changed, disposing context");
            Dispose();
        });
    }

    private static DbContextOptions<UmbracoDbContext> ConfigureOptions(DbContextOptions<UmbracoDbContext> options, out IOptionsMonitor<ConnectionStrings> connectionStringsOptionsMonitor)
    {
        connectionStringsOptionsMonitor = StaticServiceProvider.Instance.GetRequiredService<IOptionsMonitor<ConnectionStrings>>();

        ConnectionStrings connectionStrings = connectionStringsOptionsMonitor.CurrentValue;

        if (string.IsNullOrWhiteSpace(connectionStrings.ConnectionString))
        {
            ILogger<UmbracoDbContext> logger = StaticServiceProvider.Instance.GetRequiredService<ILogger<UmbracoDbContext>>();
            logger.LogCritical("No connection string was found, cannot setup Umbraco EF Core context");
        }

        IEnumerable<IMigrationProviderSetup> migrationProviders = StaticServiceProvider.Instance.GetServices<IMigrationProviderSetup>();
        IMigrationProviderSetup? migrationProvider = migrationProviders.FirstOrDefault(x => x.ProviderName == connectionStrings.ProviderName);

        if (migrationProvider == null && connectionStrings.ProviderName != null)
        {
            throw new InvalidOperationException($"No migration provider found for provider name {connectionStrings.ProviderName}");
        }

        var optionsBuilder = new DbContextOptionsBuilder<UmbracoDbContext>(options);
        migrationProvider?.Setup(optionsBuilder, connectionStrings.ConnectionString);
        return optionsBuilder.Options;
    }

    /// <inheritdoc/>
    public DbSet<CmsContentNu> CmsContentNus => Set<CmsContentNu>();

    /// <inheritdoc/>
    public DbSet<CmsContentType> CmsContentTypes => Set<CmsContentType>();

    /// <inheritdoc/>
    public DbSet<CmsContentTypeAllowedContentType> CmsContentTypeAllowedContentTypes => Set<CmsContentTypeAllowedContentType>();

    /// <inheritdoc/>
    public DbSet<CmsDictionary> CmsDictionaries => Set<CmsDictionary>();

    /// <inheritdoc/>
    public DbSet<CmsDocumentType> CmsDocumentTypes => Set<CmsDocumentType>();

    /// <inheritdoc/>
    public DbSet<CmsLanguageText> CmsLanguageTexts => Set<CmsLanguageText>();

    /// <inheritdoc/>
    public DbSet<CmsMacro> CmsMacros => Set<CmsMacro>();

    /// <inheritdoc/>
    public DbSet<CmsMacroProperty> CmsMacroProperties => Set<CmsMacroProperty>();

    /// <inheritdoc/>
    public DbSet<CmsMember> CmsMembers => Set<CmsMember>();

    /// <inheritdoc/>
    public DbSet<CmsMemberType> CmsMemberTypes => Set<CmsMemberType>();

    /// <inheritdoc/>
    public DbSet<CmsPropertyType> CmsPropertyTypes => Set<CmsPropertyType>();

    /// <inheritdoc/>
    public DbSet<CmsPropertyTypeGroup> CmsPropertyTypeGroups => Set<CmsPropertyTypeGroup>();

    /// <inheritdoc/>
    public DbSet<CmsTag> CmsTags => Set<CmsTag>();

    /// <inheritdoc/>
    public DbSet<CmsTagRelationship> CmsTagRelationships => Set<CmsTagRelationship>();

    /// <inheritdoc/>
    public DbSet<CmsTemplate> CmsTemplates => Set<CmsTemplate>();

    /// <inheritdoc/>
    public DbSet<UmbracoAccess> UmbracoAccesses => Set<UmbracoAccess>();

    /// <inheritdoc/>
    public DbSet<UmbracoAccessRule> UmbracoAccessRules => Set<UmbracoAccessRule>();

    /// <inheritdoc/>
    public DbSet<UmbracoAudit> UmbracoAudits => Set<UmbracoAudit>();

    /// <inheritdoc/>
    public DbSet<UmbracoCacheInstruction> UmbracoCacheInstructions => Set<UmbracoCacheInstruction>();

    /// <inheritdoc/>
    public DbSet<UmbracoConsent> UmbracoConsents => Set<UmbracoConsent>();

    /// <inheritdoc/>
    public DbSet<UmbracoContent> UmbracoContents => Set<UmbracoContent>();

    /// <inheritdoc/>
    public DbSet<UmbracoContentSchedule> UmbracoContentSchedules => Set<UmbracoContentSchedule>();

    /// <inheritdoc/>
    public DbSet<UmbracoContentVersion> UmbracoContentVersions => Set<UmbracoContentVersion>();

    /// <inheritdoc/>
    public DbSet<UmbracoContentVersionCleanupPolicy> UmbracoContentVersionCleanupPolicies => Set<UmbracoContentVersionCleanupPolicy>();

    /// <inheritdoc/>
    public DbSet<UmbracoContentVersionCultureVariation> UmbracoContentVersionCultureVariations => Set<UmbracoContentVersionCultureVariation>();

    /// <inheritdoc/>
    public DbSet<UmbracoCreatedPackageSchema> UmbracoCreatedPackageSchemas => Set<UmbracoCreatedPackageSchema>();

    /// <inheritdoc/>
    public DbSet<UmbracoDataType> UmbracoDataTypes => Set<UmbracoDataType>();

    /// <inheritdoc/>
    public DbSet<UmbracoDocument> UmbracoDocuments => Set<UmbracoDocument>();

    /// <inheritdoc/>
    public DbSet<UmbracoDocumentCultureVariation> UmbracoDocumentCultureVariations => Set<UmbracoDocumentCultureVariation>();

    /// <inheritdoc/>
    public DbSet<UmbracoDocumentVersion> UmbracoDocumentVersions => Set<UmbracoDocumentVersion>();

    /// <inheritdoc/>
    public DbSet<UmbracoDomain> UmbracoDomains => Set<UmbracoDomain>();

    /// <inheritdoc/>
    public DbSet<UmbracoExternalLogin> UmbracoExternalLogins => Set<UmbracoExternalLogin>();

    /// <inheritdoc/>
    public DbSet<UmbracoExternalLoginToken> UmbracoExternalLoginTokens => Set<UmbracoExternalLoginToken>();

    /// <inheritdoc/>
    public DbSet<UmbracoKeyValue> UmbracoKeyValues => Set<UmbracoKeyValue>();

    /// <inheritdoc/>
    public DbSet<UmbracoLanguage> UmbracoLanguages => Set<UmbracoLanguage>();

    /// <inheritdoc/>
    public DbSet<UmbracoLock> UmbracoLocks => Set<UmbracoLock>();

    /// <inheritdoc/>
    public DbSet<UmbracoLog> UmbracoLogs => Set<UmbracoLog>();

    /// <inheritdoc/>
    public DbSet<UmbracoLogViewerQuery> UmbracoLogViewerQueries => Set<UmbracoLogViewerQuery>();

    /// <inheritdoc/>
    public DbSet<UmbracoMediaVersion> UmbracoMediaVersions => Set<UmbracoMediaVersion>();

    /// <inheritdoc/>
    public DbSet<UmbracoNode> UmbracoNodes => Set<UmbracoNode>();

    /// <remarks>
    /// Not included in <see cref="IUmbracoDatabaseContract"/> because the model depends on <see cref="OpenIddict.EntityFrameworkCore"/>
    /// </remarks>
    public virtual DbSet<UmbracoOpenIddictApplication> UmbracoOpenIddictApplications => Set<UmbracoOpenIddictApplication>();

    /// <remarks>
    /// Not included in <see cref="IUmbracoDatabaseContract"/> because the model depends on <see cref="OpenIddict.EntityFrameworkCore"/>
    /// </remarks>
    public virtual DbSet<UmbracoOpenIddictAuthorization> UmbracoOpenIddictAuthorizations => Set<UmbracoOpenIddictAuthorization>();

    /// <remarks>
    /// Not included in <see cref="IUmbracoDatabaseContract"/> because the model depends on <see cref="OpenIddict.EntityFrameworkCore"/>
    /// </remarks>
    public virtual DbSet<UmbracoOpenIddictScope> UmbracoOpenIddictScopes => Set<UmbracoOpenIddictScope>();

    /// <remarks>
    /// Not included in <see cref="IUmbracoDatabaseContract"/> because the model depends on <see cref="OpenIddict.EntityFrameworkCore"/>
    /// </remarks>
    public virtual DbSet<UmbracoOpenIddictToken> UmbracoOpenIddictTokens => Set<UmbracoOpenIddictToken>();

    /// <inheritdoc/>
    public DbSet<UmbracoPropertyDatum> UmbracoPropertyData => Set<UmbracoPropertyDatum>();

    /// <inheritdoc/>
    public DbSet<UmbracoRedirectUrl> UmbracoRedirectUrls => Set<UmbracoRedirectUrl>();

    /// <inheritdoc/>
    public DbSet<UmbracoRelation> UmbracoRelations => Set<UmbracoRelation>();

    /// <inheritdoc/>
    public DbSet<UmbracoRelationType> UmbracoRelationTypes => Set<UmbracoRelationType>();

    /// <inheritdoc/>
    public DbSet<UmbracoServer> UmbracoServers => Set<UmbracoServer>();

    /// <inheritdoc/>
    public DbSet<UmbracoTwoFactorLogin> UmbracoTwoFactorLogins => Set<UmbracoTwoFactorLogin>();

    /// <inheritdoc/>
    public DbSet<UmbracoUser> UmbracoUsers => Set<UmbracoUser>();

    /// <inheritdoc/>
    public DbSet<UmbracoUser2NodeNotify> UmbracoUser2NodeNotifies => Set<UmbracoUser2NodeNotify>();

    /// <inheritdoc/>
    public DbSet<UmbracoUserGroup> UmbracoUserGroups => Set<UmbracoUserGroup>();

    /// <inheritdoc/>
    public DbSet<UmbracoUserGroup2App> UmbracoUserGroup2Apps => Set<UmbracoUserGroup2App>();

    /// <inheritdoc/>
    public DbSet<UmbracoUserGroup2NodePermission> UmbracoUserGroup2NodePermissions => Set<UmbracoUserGroup2NodePermission>();

    /// <inheritdoc/>
    public DbSet<UmbracoUserLogin> UmbracoUserLogins => Set<UmbracoUserLogin>();

    /// <inheritdoc/>
    public DbSet<UmbracoUserStartNode> UmbracoUserStartNodes => Set<UmbracoUserStartNode>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(UmbracoDbContext))!);
    }
}
