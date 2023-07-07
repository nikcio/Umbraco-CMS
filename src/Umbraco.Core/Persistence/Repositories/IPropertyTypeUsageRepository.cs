namespace Umbraco.Cms.Core.Persistence.Repositories;

public interface IPropertyTypeUsageRepository
{
    bool HasSavedPropertyValues(string propertyTypeAlias);

    Task<bool> HasSavedPropertyValuesAsync(string propertyTypeAlias, CancellationToken? cancellationToken = null);
}
