using Umbraco.Cms.Persistence.EFCore.Migrations;

namespace Umbraco.Cms.Infrastructure.Migrations.Upgrade.V_12_1_0;

public class AddOpenIddict : UnscopedMigrationBase
{
    private readonly IEFCoreMigrationExecutor _efCoreMigrationExecutor;

    public AddOpenIddict(IMigrationContext context, IEFCoreMigrationExecutor efCoreMigrationExecutor)
        : base(context)
    {
        _efCoreMigrationExecutor = efCoreMigrationExecutor;
    }

    protected override void Migrate()
    {
        _efCoreMigrationExecutor.ExecuteSingleMigrationAsync(EFCoreMigration.AddOpenIddict).GetAwaiter().GetResult();
    }
}
