﻿namespace Umbraco.Cms.Infrastructure.QueryBuilders
{
    public interface IIQueryableQueryBuilder
    {
        /// <summary>
        ///     Generates a string representation of the query used. This string may not be suitable for direct execution and is intended only
        ///     for use in debugging.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method is only typically supported by queries generated by Entity Framework Core.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-query-string">Viewing SQL generated by EF Core</see> for more information and examples.
        ///     </para>
        /// </remarks>
        /// <param name="source">The query source.</param>
        /// <returns>The query string for debugging.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
        string ToQueryString(IQueryable source);
    }
}
