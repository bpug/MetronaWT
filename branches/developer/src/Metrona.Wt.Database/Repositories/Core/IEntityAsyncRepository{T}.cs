//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IReadOnlyRepository{T}.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     Defines the behavior of a read-only repository of items.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Type">type</see> of item in the repository.</typeparam>
    public interface IEntityAsyncRepository<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> FindByAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            bool fromCache,
            params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        ///     Searches for items in the repository that match the specified predicate asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="predicate">
        ///     The <see cref="Expression{T}">expression</see> representing the predicate used to
        ///     match the requested <typeparamref name="T">items</typeparamref>.
        /// </param>
        /// <param name="fromCache"></param>
        /// <param name="includeProperties"></param>
        /// <returns>
        ///     A <see cref="Task{T}">task</see> containing the matched <see cref="IEnumerable{T}">sequence</see>
        ///     of <typeparamref name="T">items</typeparamref>.
        /// </returns>
        Task<IEnumerable<TEntity>> FindByAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool fromCache,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAllAsync(bool fromCache);

        /// <summary>
        ///     Retrieves all items in the repository asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken">cancellation token</see> that can be used to cancel
        ///     the operation.
        /// </param>
        /// <param name="id"></param>
        /// <returns>
        ///     A <see cref="Task{T}">task</see> containing the <see cref="IEnumerable{T}">sequence</see>
        ///     of all <typeparamref name="T">items</typeparamref> in the repository.
        /// </returns>
        Task<TEntity> GetAsync(long id);

        /// <summary>
        ///     Retrieves all items in the repository satisfied by the specified query asynchronously.
        /// </summary>
        /// <param name="queryShaper">
        ///     The <see cref="Func{T,TResult}">function</see> that shapes the
        ///     <see cref="IQueryable{T}">query</see> to execute.
        /// </param>
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken">cancellation token</see> that can be used to cancel
        ///     the operation.
        /// </param>
        /// <returns>
        ///     A <see cref="Task{T}">task</see> containing the retrieved <see cref="IEnumerable{T}">sequence</see>
        ///     of <typeparamref name="T">items</typeparamref>.
        /// </returns>
        Task<IEnumerable<TEntity>> GetAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper,
            CancellationToken cancellationToken,
            bool fromCache);

        /// <summary>
        ///     Retrieves a query result asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The <see cref="Type">type</see> of result to retrieve.</typeparam>
        /// <param name="queryShaper">
        ///     The <see cref="Func{T,TResult}">function</see> that shapes the
        ///     <see cref="IQueryable{T}">query</see> to execute.
        /// </param>
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken">cancellation token</see> that can be used to cancel
        ///     the operation.
        /// </param>
        /// <returns>
        ///     A <see cref="Task{T}">task</see> containing the <typeparamref name="TResult">result</typeparamref> of the
        ///     operation.
        /// </returns>
        Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, bool fromCache);

        Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper);

        Task<IEnumerable<T>> GetByAsync<T>(Func<IQueryable<T>, IQueryable<T>> queryShaper, CancellationToken cancellationToken, bool fromCache = false) where T : class;

        Task<IEnumerable<T>> GetByAsync<T>(Func<IQueryable<T>, IQueryable<T>> queryShaper, bool fromCache = false) where T : class;

        Task<IEnumerable<T>> GetByAsync<T>(
            Expression<Func<T, bool>> predicate,
            bool fromCache = false,
            params Expression<Func<T, object>>[] includeProperties) where T : class;

        /// <summary>
        ///     Retrieves a single item in the repository matching the specified predicate asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="predicate">
        ///     The <see cref="Expression{T}">expression</see> representing the predicate used to
        ///     match the requested <typeparamref name="T">item</typeparamref>.
        /// </param>
        /// <param name="fromCache"></param>
        /// <param name="includeProperties"></param>
        /// <returns>
        ///     A <see cref="Task{T}">task</see> containing the matched <typeparamref name="T">item</typeparamref>
        ///     or null if no match was found.
        /// </returns>
        Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool fromCache,
            params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        ///     Retrieves a single item in the repository matching the specified predicate asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="predicate">
        ///     The <see cref="Expression{T}">expression</see> representing the predicate used to
        ///     match the requested <typeparamref name="T">item</typeparamref>.
        /// </param>
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken">cancellation token</see> that can be used to cancel
        ///     the operation.
        /// </param>
        /// <param name="fromCache"></param>
        /// <param name="includeProperties"></param>
        /// <returns>
        ///     A <see cref="Task{T}">task</see> containing the matched <typeparamref name="T">item</typeparamref>
        ///     or null if no match was found.
        /// </returns>
        Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            bool fromCache,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> GetFirstAsync(
           Expression<Func<TEntity, bool>> predicate,
           bool fromCache,
           params Expression<Func<TEntity, object>>[] includeProperties);
    }
}