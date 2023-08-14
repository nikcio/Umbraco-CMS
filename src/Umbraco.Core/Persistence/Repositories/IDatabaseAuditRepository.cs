using Umbraco.Cms.Core.Models;

namespace Umbraco.Cms.Core.Persistence.Repositories;

/// <summary>
/// Represents the AuditRepository for doing CRUD operations for <see cref="IAuditItem"/>.
/// </summary>
public interface IDatabaseAuditRepository : IDatabaseRepository
{
    /// <summary>
    /// Saves an audit item.
    /// </summary>
    /// <param name="auditItem"></param>
    /// <returns></returns>
    IAuditItem SaveAudit(IAuditItem auditItem);
}
