using Microsoft.Extensions.Logging;
using NPoco;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Persistence.Querying;
using Umbraco.Cms.Core.Persistence.Repositories;
using Umbraco.Cms.Infrastructure.Persistence.Dtos;
using Umbraco.Cms.Infrastructure.Persistence.Factories;
using Umbraco.Cms.Infrastructure.Persistence.Querying;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace Umbraco.Cms.Infrastructure.Persistence.Repositories.Implement;

/// <summary>
///     Represents the NPoco implementation of <see cref="IAuditEntryRepository" />.
/// </summary>
internal class AuditEntryRepository : EntityRepositoryBase<int, IAuditEntry>, IAuditEntryRepository
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AuditEntryRepository" /> class.
    /// </summary>
    public AuditEntryRepository(IScopeAccessor scopeAccessor, AppCaches cache, ILogger<AuditEntryRepository> logger)
        : base(scopeAccessor, cache, logger)
    {
    }

    /// <inheritdoc />
    public IEnumerable<IAuditEntry> GetPage(long pageIndex, int pageCount, out long records)
    {
        Sql<ISqlContext> sql = Sql()
            .Select<AuditEntryDto>()
            .From<AuditEntryDto>()
            .OrderByDescending<AuditEntryDto>(x => x.EventDateUtc);

        Page<AuditEntryDto> page = Database.Page<AuditEntryDto>(pageIndex + 1, pageCount, sql);
        records = page.TotalItems;
        return page.Items.Select(AuditEntryFactory.BuildEntity);
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<IAuditEntry> Results, long Records)> GetPageAsync(long pageIndex, int pageCount, CancellationToken? cancellationToken = null)
    {
        Sql<ISqlContext> sql = Sql()
            .Select<AuditEntryDto>()
            .From<AuditEntryDto>()
            .OrderByDescending<AuditEntryDto>(x => x.EventDateUtc);

        Page<AuditEntryDto> page = await Database.PageAsync<AuditEntryDto>(pageIndex + 1, pageCount, sql);
        var records = page.TotalItems;
        return new (page.Items.Select(AuditEntryFactory.BuildEntity), records);
    }

    /// <inheritdoc />
    public bool IsAvailable()
    {
        var tables = SqlSyntax.GetTablesInSchema(Database).ToArray();
        return tables.InvariantContains(Constants.DatabaseSchema.Tables.AuditEntry);
    }

    /// <inheritdoc />
    public async Task<bool> IsAvailableAsync(CancellationToken? cancellationToken = null)
    {
        var tables = (await SqlSyntax.GetTablesInSchemaAsync(Database, cancellationToken)).ToArray();
        return tables.InvariantContains(Constants.DatabaseSchema.Tables.AuditEntry);
    }

    /// <inheritdoc />
    protected override IAuditEntry? PerformGet(int id)
    {
        Sql<ISqlContext> sql = Sql()
            .Select<AuditEntryDto>()
            .From<AuditEntryDto>()
            .Where<AuditEntryDto>(x => x.Id == id);

        AuditEntryDto dto = Database.FirstOrDefault<AuditEntryDto>(sql);
        return dto == null ? null : AuditEntryFactory.BuildEntity(dto);
    }

    /// <inheritdoc />
    protected override async Task<IAuditEntry?> PerformGetAsync(int id, CancellationToken? cancellationToken = null)
    {
        Sql<ISqlContext> sql = Sql()
            .Select<AuditEntryDto>()
            .From<AuditEntryDto>()
            .Where<AuditEntryDto>(x => x.Id == id);

        AuditEntryDto dto = await Database.FirstOrDefaultAsync<AuditEntryDto>(sql);
        return dto == null ? null : AuditEntryFactory.BuildEntity(dto);
    }

    /// <inheritdoc />
    protected override IEnumerable<IAuditEntry> PerformGetAll(params int[]? ids)
    {
        if (ids?.Length == 0)
        {
            Sql<ISqlContext> sql = Sql()
                .Select<AuditEntryDto>()
                .From<AuditEntryDto>();

            return Database.Fetch<AuditEntryDto>(sql).Select(AuditEntryFactory.BuildEntity);
        }

        var entries = new List<IAuditEntry>();

        foreach (IEnumerable<int> group in ids.InGroupsOf(Constants.Sql.MaxParameterCount))
        {
            Sql<ISqlContext> sql = Sql()
                .Select<AuditEntryDto>()
                .From<AuditEntryDto>()
                .WhereIn<AuditEntryDto>(x => x.Id, group);

            entries.AddRange(Database.Fetch<AuditEntryDto>(sql).Select(AuditEntryFactory.BuildEntity));
        }

        return entries;
    }

    /// <inheritdoc />
    protected override async Task<IEnumerable<IAuditEntry>> PerformGetAllAsync(CancellationToken? cancellationToken = null, params int[]? ids)
    {
        if (ids?.Length == 0)
        {
            Sql<ISqlContext> sql = Sql()
                .Select<AuditEntryDto>()
                .From<AuditEntryDto>();

            return (await Database.FetchAsync<AuditEntryDto>(sql)).Select(AuditEntryFactory.BuildEntity);
        }

        var entries = new List<IAuditEntry>();

        foreach (IEnumerable<int> group in ids.InGroupsOf(Constants.Sql.MaxParameterCount))
        {
            Sql<ISqlContext> sql = Sql()
                .Select<AuditEntryDto>()
                .From<AuditEntryDto>()
                .WhereIn<AuditEntryDto>(x => x.Id, group);

            entries.AddRange((await Database.FetchAsync<AuditEntryDto>(sql)).Select(AuditEntryFactory.BuildEntity));
        }

        return entries;
    }

    /// <inheritdoc />
    protected override IEnumerable<IAuditEntry> PerformGetByQuery(IQuery<IAuditEntry> query)
    {
        Sql<ISqlContext> sqlClause = GetBaseQuery(false);
        var translator = new SqlTranslator<IAuditEntry>(sqlClause, query);
        Sql<ISqlContext> sql = translator.Translate();
        return Database.Fetch<AuditEntryDto>(sql).Select(AuditEntryFactory.BuildEntity);
    }

    /// <inheritdoc />
    protected override async Task<IEnumerable<IAuditEntry>> PerformGetByQueryAsync(IQuery<IAuditEntry> query, CancellationToken? cancellationToken = null)
    {
        Sql<ISqlContext> sqlClause = GetBaseQuery(false);
        var translator = new SqlTranslator<IAuditEntry>(sqlClause, query);
        Sql<ISqlContext> sql = translator.Translate();
        return (await Database.FetchAsync<AuditEntryDto>(sql)).Select(AuditEntryFactory.BuildEntity);
    }

    /// <inheritdoc />
    protected override Sql<ISqlContext> GetBaseQuery(bool isCount)
    {
        Sql<ISqlContext> sql = Sql();
        sql = isCount ? sql.SelectCount() : sql.Select<AuditEntryDto>();
        sql = sql.From<AuditEntryDto>();
        return sql;
    }

    /// <inheritdoc />
    protected override string GetBaseWhereClause() => $"{Constants.DatabaseSchema.Tables.AuditEntry}.id = @id";

    /// <inheritdoc />
    protected override IEnumerable<string> GetDeleteClauses() =>
        throw new NotSupportedException("Audit entries cannot be deleted.");

    /// <inheritdoc />
    protected override void PersistNewItem(IAuditEntry entity)
    {
        entity.AddingEntity();

        AuditEntryDto dto = AuditEntryFactory.BuildDto(entity);
        Database.Insert(dto);
        entity.Id = dto.Id;
        entity.ResetDirtyProperties();
    }

    /// <inheritdoc />
    protected override async Task PersistNewItemAsync(IAuditEntry item, CancellationToken? cancellationToken = null)
    {
        item.AddingEntity();

        AuditEntryDto dto = AuditEntryFactory.BuildDto(item);
        await Database.InsertAsync(dto);
        item.Id = dto.Id;
        item.ResetDirtyProperties();
    }

    /// <inheritdoc />
    protected override void PersistUpdatedItem(IAuditEntry entity) =>
        throw new NotSupportedException("Audit entries cannot be updated.");

    /// <inheritdoc />
    protected override Task PersistUpdatedItemAsync(IAuditEntry item, CancellationToken? cancellationToken = null) => throw new NotSupportedException("Audit entries cannot be updated.");
}
