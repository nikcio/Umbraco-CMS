using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Umbraco.Cms.Infrastructure.Extensions;

namespace Umbraco.Cms.Persistence.EFCore.QueryBuilders
{
    public class IncludableQueryBuilder : IIncludableQueryBuilder
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

            public Expression Expression
                => _queryable.Expression;

            public Type ElementType
                => _queryable.ElementType;

            public IQueryProvider Provider
                => _queryable.Provider;

            public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                => ((IAsyncEnumerable<TEntity>)_queryable).GetAsyncEnumerator(cancellationToken);

            public IEnumerator<TEntity> GetEnumerator()
                => _queryable.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
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

            public Expression Expression
                => _queryable.Expression;

            public Type ElementType
                => _queryable.ElementType;

            public IQueryProvider Provider
                => _queryable.Provider;

            public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                => ((IAsyncEnumerable<TEntity>)_queryable).GetAsyncEnumerator(cancellationToken);

            public IEnumerator<TEntity> GetEnumerator()
                => _queryable.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        }

        /// <inheritdoc/>
        public IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class => new IncludableQueryable<TEntity, TProperty>(source.Include(navigationPropertyPath));

        /// <inheritdoc/>
        public IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> source, string navigationPropertyPath) where TEntity : class => source.Include(navigationPropertyPath);

        /// <inheritdoc/>
        public IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class => new IncludableQueryable<TEntity, TProperty>(new EFCoreIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>(source).ThenInclude(navigationPropertyPath));

        /// <inheritdoc/>
        public IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(IIncludableQueryable<TEntity, TPreviousProperty> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class => new IncludableQueryable<TEntity, TProperty>(new EFCoreIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>(source).ThenInclude(navigationPropertyPath));
    }
}
