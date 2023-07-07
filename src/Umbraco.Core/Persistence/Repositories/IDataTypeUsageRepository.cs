namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IDataTypeUsageRepository
{
    bool HasSavedValues(int dataTypeId);

    Task<bool> HasSavedValuesAsync(int dataTypeId, CancellationToken? cancellationToken = null);
}
