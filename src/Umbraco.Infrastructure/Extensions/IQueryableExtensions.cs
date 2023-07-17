using System.Linq.Expressions;
using Umbraco.Cms.Core.DependencyInjection;

namespace Umbraco.Cms.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <remarks>
    /// Used to decopule Umbraco from EF Core.
    /// </remarks>
    internal static class IQueryableExtensions
    {
        private static IIncludableQueryBuilder _builder = (IIncludableQueryBuilder)StaticServiceProvider.Instance.GetService(typeof(IIncludableQueryBuilder))!;

        /// <summary>
        ///     Specifies related entities to include in the query results. The navigation property to be included is specified starting with the
        ///     type of entity being queried (<typeparamref name="TEntity" />). If you wish to include additional types based on the navigation
        ///     properties of the type being included, then chain a call to
        ///     <see
        ///         cref="ThenInclude{TEntity, TPreviousProperty, TProperty}(IIncludableQueryable{TEntity, IEnumerable{TPreviousProperty}}, Expression{Func{TPreviousProperty, TProperty}})" />
        ///     after this call.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-load-related-data">Loading related entities</see> for more information
        ///     and examples.
        /// </remarks>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="navigationPropertyPath">
        ///     A lambda expression representing the navigation property to be included (<c>t => t.Property1</c>).
        /// </param>
        /// <returns>A new query with the related data included.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" /> or <paramref name="navigationPropertyPath" /> is <see langword="null" />.
        /// </exception>
        public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(
            this IQueryable<TEntity> source,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            return _builder.Include(source, navigationPropertyPath);
        }

        /// <summary>
        ///     Specifies related entities to include in the query results. The navigation property to be included is
        ///     specified starting with the type of entity being queried (<typeparamref name="TEntity" />). Further
        ///     navigation properties to be included can be appended, separated by the '.' character.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-load-related-data">Loading related entities</see> for more information
        ///     and examples.
        /// </remarks>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="navigationPropertyPath">A string of '.' separated navigation property names to be included.</param>
        /// <returns>A new query with the related data included.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" /> or <paramref name="navigationPropertyPath" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="navigationPropertyPath" /> is empty or whitespace.</exception>
        public static IQueryable<TEntity> Include<TEntity>(
            this IQueryable<TEntity> source,
            string navigationPropertyPath)
            where TEntity : class
        {
            return _builder.Include(source, navigationPropertyPath);
        }

        /// <summary>
        ///     Specifies additional related data to be further included based on a related type that was just included.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-load-related-data">Loading related entities</see> for more information
        ///     and examples.
        /// </remarks>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TPreviousProperty">The type of the entity that was just included.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="navigationPropertyPath">
        ///     A lambda expression representing the navigation property to be included (<c>t => t.Property1</c>).
        /// </param>
        /// <returns>A new query with the related data included.</returns>
        public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            return _builder.ThenInclude(source, navigationPropertyPath);
        }

        /// <summary>
        ///     Specifies additional related data to be further included based on a related type that was just included.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-load-related-data">Loading related entities</see> for more information
        ///     and examples.
        /// </remarks>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TPreviousProperty">The type of the entity that was just included.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="navigationPropertyPath">
        ///     A lambda expression representing the navigation property to be included (<c>t => t.Property1</c>).
        /// </param>
        /// <returns>A new query with the related data included.</returns>
        public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableQueryable<TEntity, TPreviousProperty> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            return _builder.ThenInclude(source, navigationPropertyPath);
        }
    }

    /// <summary>
    ///     Supports queryable Include/ThenInclude chaining operators.
    /// </summary>
    /// <remarks>
    ///     See <see href="https://aka.ms/efcore-docs-providers">Implementation of database providers and extensions</see>
    ///     and <see href="https://aka.ms/efcore-docs-how-query-works">How EF Core queries work</see> for more information and examples.
    /// </remarks>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    public interface IIncludableQueryable<out TEntity, out TProperty> : IQueryable<TEntity>
    {
    }

    public interface IIncludableQueryBuilder
    {
        /// <summary>
        ///     Specifies related entities to include in the query results. The navigation property to be included is specified starting with the
        ///     type of entity being queried (<typeparamref name="TEntity" />). If you wish to include additional types based on the navigation
        ///     properties of the type being included, then chain a call to
        ///     <see
        ///         cref="ThenInclude{TEntity, TPreviousProperty, TProperty}(IIncludableQueryable{TEntity, IEnumerable{TPreviousProperty}}, Expression{Func{TPreviousProperty, TProperty}})" />
        ///     after this call.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-load-related-data">Loading related entities</see> for more information
        ///     and examples.
        /// </remarks>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="navigationPropertyPath">
        ///     A lambda expression representing the navigation property to be included (<c>t => t.Property1</c>).
        /// </param>
        /// <returns>A new query with the related data included.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" /> or <paramref name="navigationPropertyPath" /> is <see langword="null" />.
        /// </exception>
        IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(
            IQueryable<TEntity> source,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class;

        /// <summary>
        ///     Specifies related entities to include in the query results. The navigation property to be included is
        ///     specified starting with the type of entity being queried (<typeparamref name="TEntity" />). Further
        ///     navigation properties to be included can be appended, separated by the '.' character.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-load-related-data">Loading related entities</see> for more information
        ///     and examples.
        /// </remarks>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="navigationPropertyPath">A string of '.' separated navigation property names to be included.</param>
        /// <returns>A new query with the related data included.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" /> or <paramref name="navigationPropertyPath" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="navigationPropertyPath" /> is empty or whitespace.</exception>
        IQueryable<TEntity> Include<TEntity>(
            IQueryable<TEntity> source,
            string navigationPropertyPath)
            where TEntity : class;

        /// <summary>
        ///     Specifies additional related data to be further included based on a related type that was just included.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-load-related-data">Loading related entities</see> for more information
        ///     and examples.
        /// </remarks>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TPreviousProperty">The type of the entity that was just included.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="navigationPropertyPath">
        ///     A lambda expression representing the navigation property to be included (<c>t => t.Property1</c>).
        /// </param>
        /// <returns>A new query with the related data included.</returns>
        IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class;

        /// <summary>
        ///     Specifies additional related data to be further included based on a related type that was just included.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-load-related-data">Loading related entities</see> for more information
        ///     and examples.
        /// </remarks>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TPreviousProperty">The type of the entity that was just included.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="navigationPropertyPath">
        ///     A lambda expression representing the navigation property to be included (<c>t => t.Property1</c>).
        /// </param>
        /// <returns>A new query with the related data included.</returns>
        IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            IIncludableQueryable<TEntity, TPreviousProperty> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class;
    }
}
