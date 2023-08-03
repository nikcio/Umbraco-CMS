using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Umbraco.Cms.Infrastructure.Extensions;
using Umbraco.Cms.Infrastructure.QueryBuilders;
using Umbraco.Cms.Infrastructure.QueryBuilders.IQueryableInterfaces;

namespace Umbraco.Cms.Persistence.EFCore.QueryBuilders
{
    public class EFCoreQueryBuilder : IEFCoreQueryBuilder
    {
        /// <summary>
        /// Helper to convert a <see cref="Microsoft.EntityFrameworkCore.Query.IIncludableQueryable{TEntity, TProperty}"/> to a <see cref="IIncludableQueryable{TEntity, TProperty}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        private sealed class IncludableQueryable<TEntity, TProperty> : IIncludableQueryable<TEntity, TProperty>
        {
            private readonly IQueryable<TEntity> _queryable;

            public IncludableQueryable(IQueryable<TEntity> queryable)
            {
                _queryable = queryable;
            }

            public IncludableQueryable(Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty> query)
                : this(query.AsQueryable())
            {
            }

            public Expression Expression => _queryable.Expression;

            public Type ElementType => _queryable.ElementType;

            public IQueryProvider Provider => _queryable.Provider;

            public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                => ((IAsyncEnumerable<TEntity>)_queryable).GetAsyncEnumerator(cancellationToken);

            public IEnumerator<TEntity> GetEnumerator() => _queryable.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Helper to convert a <see cref="IIncludableQueryable{TEntity, TProperty}"/> to a <see cref="Microsoft.EntityFrameworkCore.Query.IIncludableQueryable{TEntity, TProperty}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        private sealed class EFCoreIncludableQueryable<TEntity, TProperty> : Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty>
        {
            private readonly IQueryable<TEntity> _queryable;

            public EFCoreIncludableQueryable(IQueryable<TEntity> queryable)
            {
                _queryable = queryable;
            }

            public EFCoreIncludableQueryable(IIncludableQueryable<TEntity, TProperty> query)
                : this(query.AsQueryable())
            {
            }

            public Expression Expression => _queryable.Expression;

            public Type ElementType => _queryable.ElementType;

            public IQueryProvider Provider => _queryable.Provider;

            public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                => ((IAsyncEnumerable<TEntity>)_queryable).GetAsyncEnumerator(cancellationToken);

            public IEnumerator<TEntity> GetEnumerator() => _queryable.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <inheritdoc/>
        public IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class => new IncludableQueryable<TEntity, TProperty>(source.Include(navigationPropertyPath));

        /// <inheritdoc/>
        public IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> source, string navigationPropertyPath)
            where TEntity : class => source.Include(navigationPropertyPath);

        /// <inheritdoc/>
        public IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class => new IncludableQueryable<TEntity, TProperty>(new EFCoreIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>(source).ThenInclude(navigationPropertyPath));

        /// <inheritdoc/>
        public IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(IIncludableQueryable<TEntity, TPreviousProperty> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class => new IncludableQueryable<TEntity, TProperty>(new EFCoreIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>(source).ThenInclude(navigationPropertyPath));

        /// <inheritdoc/>
        public string ToQueryString(IQueryable source) => source.ToQueryString();

        /// <inheritdoc/>
        public Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.AnyAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.AnyAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public Task<bool> AllAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.AllAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public Task<int> CountAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.CountAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<int> CountAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.CountAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public Task<long> LongCountAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.LongCountAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<long> LongCountAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.LongCountAsync(predicate, cancellationToken);

        // This will be a thing in .NET 8
        ///// <inheritdoc/>
        //public Task<TSource> ElementAtAsync<TSource>(IQueryable<TSource> source, int index, CancellationToken cancellationToken = default) => source.ElementAtAsync(index, cancellationToken);

        ///// <inheritdoc/>
        //public Task<TSource> ElementAtOrDefaultAsync<TSource>(IQueryable<TSource> source, int index, CancellationToken cancellationToken = default) => source.ElementAtOrDefaultAsync(index, cancellationToken);

        /// <inheritdoc/>
        public Task<TSource> FirstAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.FirstAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TSource> FirstAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.FirstAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public Task<TSource?> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TSource?> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.FirstOrDefaultAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public Task<TSource> LastAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.LastAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TSource> LastAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.LastAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public Task<TSource?> LastOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.LastOrDefaultAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TSource?> LastOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.LastOrDefaultAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public Task<TSource> SingleAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.SingleAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TSource> SingleAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.SingleAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public Task<TSource?> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.SingleOrDefaultAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TSource?> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) => source.SingleOrDefaultAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public Task<TSource> MinAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.MinAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TResult> MinAsync<TSource, TResult>(IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default) => source.MinAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<TSource> MaxAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.MaxAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TResult> MaxAsync<TSource, TResult>(IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default) => source.MaxAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<decimal> SumAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<decimal?> SumAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<decimal> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<decimal?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<int> SumAsync(IQueryable<int> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<int?> SumAsync(IQueryable<int?> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<int> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<int?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<long> SumAsync(IQueryable<long> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<long?> SumAsync(IQueryable<long?> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<long> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<long?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<double> SumAsync(IQueryable<double> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<double?> SumAsync(IQueryable<double?> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<double> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<double?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<float> SumAsync(IQueryable<float> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<float?> SumAsync(IQueryable<float?> source, CancellationToken cancellationToken = default) => source.SumAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<float> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<float?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default) => source.SumAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<decimal> AverageAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<decimal?> AverageAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<decimal> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<decimal?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<double> AverageAsync(IQueryable<int> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<double?> AverageAsync(IQueryable<int?> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<double> AverageAsync(IQueryable<long> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<double?> AverageAsync(IQueryable<long?> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<double> AverageAsync(IQueryable<double> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<double?> AverageAsync(IQueryable<double?> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<float> AverageAsync(IQueryable<float> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<float?> AverageAsync(IQueryable<float?> source, CancellationToken cancellationToken = default) => source.AverageAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<float> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<float?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default) => source.AverageAsync(selector, cancellationToken);

        /// <inheritdoc/>
        public Task<bool> ContainsAsync<TSource>(IQueryable<TSource> source, TSource item, CancellationToken cancellationToken = default) => source.ContainsAsync(item, cancellationToken);

        /// <inheritdoc/>
        public Task<List<TSource>> ToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.ToListAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TSource[]> ToArrayAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.ToArrayAsync(cancellationToken);

        /// <inheritdoc/>
        public IQueryable<TEntity> IgnoreAutoIncludes<TEntity>(IQueryable<TEntity> source)
            where TEntity : class => source.IgnoreAutoIncludes();

        /// <inheritdoc/>
        public IQueryable<TEntity> IgnoreQueryFilters<TEntity>(IQueryable<TEntity> source)
            where TEntity : class => source.IgnoreQueryFilters();

        /// <inheritdoc/>
        public IQueryable<TEntity> AsNoTracking<TEntity>(IQueryable<TEntity> source)
            where TEntity : class => source.AsNoTracking();

        /// <inheritdoc/>
        public IQueryable<TEntity> AsNoTrackingWithIdentityResolution<TEntity>(IQueryable<TEntity> source)
            where TEntity : class => source.AsNoTrackingWithIdentityResolution();

        /// <inheritdoc/>
        public IQueryable<TEntity> AsTracking<TEntity>(IQueryable<TEntity> source)
            where TEntity : class => source.AsTracking();

        /// <inheritdoc/>
        public IQueryable<TEntity> AsTracking<TEntity>(IQueryable<TEntity> source, Infrastructure.QueryBuilders.QueryTrackingBehavior track)
            where TEntity : class => source.AsTracking(Enum.Parse<Microsoft.EntityFrameworkCore.QueryTrackingBehavior>(track.ToString()));

        /// <inheritdoc/>
        public void Load<TSource>(IQueryable<TSource> source) => source.Load();

        /// <inheritdoc/>
        public Task LoadAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default) => source.LoadAsync(cancellationToken);

        // This will be a thing in .NET 8
        ///// <inheritdoc/>
        //public Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default)
        //    where TKey : notnull => source.ToDictionaryAsync<TSource, TKey>(source, keySelector, cancellationToken);

        /// <inheritdoc/>
        public Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
            where TKey : notnull => source.ToDictionaryAsync(keySelector, comparer, cancellationToken);

        /// <inheritdoc/>
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default)
            where TKey : notnull => source.ToDictionaryAsync(keySelector, elementSelector, cancellationToken);

        /// <inheritdoc/>
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default)
            where TKey : notnull => source.ToDictionaryAsync(keySelector, elementSelector, comparer, cancellationToken);

        /// <inheritdoc/>
        public Task ForEachAsync<T>(IQueryable<T> source, Action<T> action, CancellationToken cancellationToken = default) => source.ForEachAsync(action, cancellationToken);

        /// <inheritdoc/>
        public IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(IQueryable<TSource> source) => source.AsAsyncEnumerable();
    }
}
