using NPoco;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Persistence.Repositories;
using Umbraco.Cms.Infrastructure.Persistence.Dtos;
using Umbraco.Cms.Infrastructure.Persistence.Factories;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace Umbraco.Cms.Infrastructure.Persistence.Repositories.Implement;

/// <summary>
///     Represents the NPoco implementation of <see cref="ICacheInstructionRepository" />.
/// </summary>
internal class CacheInstructionRepository : ICacheInstructionRepository
{
    private readonly IScopeAccessor _scopeAccessor;

    public CacheInstructionRepository(IScopeAccessor scopeAccessor) => _scopeAccessor = scopeAccessor;

    private IScope? AmbientScope => _scopeAccessor.AmbientScope;

    /// <inheritdoc />
    public int CountAll()
    {
        Sql<ISqlContext>? sql = AmbientScope?.SqlContext.Sql().Select("COUNT(*)")
            .From<CacheInstructionDto>();

        return AmbientScope?.Database.ExecuteScalar<int>(sql) ?? 0;
    }

    public async Task<int> CountAllAsync(CancellationToken? cancellationToken = null)
    {
        if (AmbientScope == null)
        {
            return 0;
        }

        Sql<ISqlContext>? sql = AmbientScope.SqlContext.Sql().Select("COUNT(*)")
            .From<CacheInstructionDto>();

        return await AmbientScope.Database.ExecuteScalarAsync<int>(sql);
    }

    /// <inheritdoc />
    public int CountPendingInstructions(int lastId) =>
        AmbientScope?.Database.ExecuteScalar<int>(
            "SELECT SUM(instructionCount) FROM umbracoCacheInstruction WHERE id > @lastId", new { lastId }) ?? 0;

    public async Task<int> CountPendingInstructionsAsync(int lastId, CancellationToken? cancellationToken = null)
    {
        if (AmbientScope == null)
        {
            return 0;
        }

        return await AmbientScope.Database.ExecuteScalarAsync<int>("SELECT SUM(instructionCount) FROM umbracoCacheInstruction WHERE id > @lastId", new { lastId });
    }

    /// <inheritdoc />
    public int GetMaxId() =>
        AmbientScope?.Database.ExecuteScalar<int>("SELECT MAX(id) FROM umbracoCacheInstruction") ?? 0;

    public async Task<int> GetMaxIdAsync(CancellationToken? cancellationToken = null)
    {
        if (AmbientScope == null)
        {
            return 0;
        }

        return await AmbientScope.Database.ExecuteScalarAsync<int>("SELECT MAX(id) FROM umbracoCacheInstruction");
    }

    /// <inheritdoc />
    public bool Exists(int id) => AmbientScope?.Database.Exists<CacheInstructionDto>(id) ?? false;

    public Task<bool> ExistsAsync(int id, CancellationToken? cancellationToken = null){
        if (AmbientScope == null)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(AmbientScope.Database.Exists<CacheInstructionDto>(id));
    }

    /// <inheritdoc />
    public void Add(CacheInstruction cacheInstruction)
    {
        CacheInstructionDto dto = CacheInstructionFactory.BuildDto(cacheInstruction);
        AmbientScope?.Database.Insert(dto);
    }

    public async Task AddAsync(CacheInstruction cacheInstruction, CancellationToken? cancellationToken = null)
    {
        if(AmbientScope == null)
        {
            return;
        }

        CacheInstructionDto dto = CacheInstructionFactory.BuildDto(cacheInstruction);
        await AmbientScope.Database.InsertAsync(dto);
    }

    /// <inheritdoc />
    public IEnumerable<CacheInstruction> GetPendingInstructions(int lastId, int maxNumberToRetrieve)
    {
        Sql<ISqlContext>? sql = AmbientScope?.SqlContext.Sql().SelectAll()
            .From<CacheInstructionDto>()
            .Where<CacheInstructionDto>(dto => dto.Id > lastId)
            .OrderBy<CacheInstructionDto>(dto => dto.Id);
        Sql<ISqlContext>? topSql = sql?.SelectTop(maxNumberToRetrieve);
        return AmbientScope?.Database.Fetch<CacheInstructionDto>(topSql).Select(CacheInstructionFactory.BuildEntity) ??
               Array.Empty<CacheInstruction>();
    }

    public async Task<IEnumerable<CacheInstruction>> GetPendingInstructionsAsync(int lastId, int maxNumberToRetrieve, CancellationToken? cancellationToken = null)
    {
        if (AmbientScope == null)
        {
            return Enumerable.Empty<CacheInstruction>();
        }

        Sql<ISqlContext>? sql = AmbientScope.SqlContext.Sql().SelectAll()
            .From<CacheInstructionDto>()
            .Where<CacheInstructionDto>(dto => dto.Id > lastId)
            .OrderBy<CacheInstructionDto>(dto => dto.Id);
        Sql<ISqlContext>? topSql = sql?.SelectTop(maxNumberToRetrieve);
        return (await AmbientScope.Database.FetchAsync<CacheInstructionDto>(topSql)).Select(CacheInstructionFactory.BuildEntity);
    }

    /// <inheritdoc />
    public void DeleteInstructionsOlderThan(DateTime pruneDate)
    {
        // Using 2 queries is faster than convoluted joins.
        var maxId = AmbientScope?.Database.ExecuteScalar<int>("SELECT MAX(id) FROM umbracoCacheInstruction;");
        Sql deleteSql =
            new Sql().Append(
                @"DELETE FROM umbracoCacheInstruction WHERE utcStamp < @pruneDate AND id < @maxId",
                new { pruneDate, maxId });
        AmbientScope?.Database.Execute(deleteSql);
    }

    public async Task DeleteInstructionsOlderThanAsync(DateTime pruneDate, CancellationToken? cancellationToken = null)
    {
        if(AmbientScope == null)
        {
            return;
        }

        // Using 2 queries is faster than convoluted joins.
        var maxId = await AmbientScope.Database.ExecuteScalarAsync<int>("SELECT MAX(id) FROM umbracoCacheInstruction;");
        Sql deleteSql =
            new Sql().Append(
                @"DELETE FROM umbracoCacheInstruction WHERE utcStamp < @pruneDate AND id < @maxId",
                new { pruneDate, maxId });
        await AmbientScope.Database.ExecuteAsync(deleteSql);
    }
}
