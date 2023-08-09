using Umbraco.Cms.Infrastructure.Persistence;

namespace Umbraco.Cms.Core.Scoping;

/// <summary>
/// Represents a database scope.
/// </summary>
public interface IDatabaseScope : INotificationScope
{
    /// <summary>
    /// Gets the <see cref="IUmbracoDatabase"/>. for the scope
    /// </summary>
    IUmbracoDatabase UmbracoDatabase { get; }
}
