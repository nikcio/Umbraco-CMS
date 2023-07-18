namespace Umbraco.Cms.Infrastructure.QueryBuilders
{
    public interface IElementAtQueryBuilder
    {
        /// <summary>
        ///     Asynchronously returns the element at a specified index in a sequence.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Multiple active operations on the same context instance are not supported. Use <see langword="await" /> to ensure
        ///         that any asynchronous operations have completed before calling another method on this context.
        ///         See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information and examples.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-async-linq">Querying data with EF Core</see> for more information and examples.
        ///     </para>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IQueryable{T}" /> to return the element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the element at a specified index in a <paramref name="source" /> sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <para>
        ///         <paramref name="index" /> is less than zero.
        ///     </para>
        /// </exception>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
        Task<TSource> ElementAtAsync<TSource>(IQueryable<TSource> source, int index, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Asynchronously returns the element at a specified index in a sequence, or a default value if the index is out of range.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Multiple active operations on the same context instance are not supported. Use <see langword="await" /> to ensure
        ///         that any asynchronous operations have completed before calling another method on this context.
        ///         See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information and examples.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-async-linq">Querying data with EF Core</see> for more information and examples.
        ///     </para>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IQueryable{T}" /> to return the element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the element at a specified index in a <paramref name="source" /> sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
        Task<TSource> ElementAtOrDefaultAsync<TSource>(IQueryable<TSource> source, int index, CancellationToken cancellationToken = default);
    }
}
