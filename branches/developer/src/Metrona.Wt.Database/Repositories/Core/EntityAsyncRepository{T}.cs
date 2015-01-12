//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EntityReadOnlyRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using EntityFramework.Extensions;

    using Metrona.Wt.Core.Extensions;

    public class EntityAsyncRepository<TEntity> : IEntityAsyncRepository<TEntity> where TEntity : class, new()
    {
        protected readonly DbSet<TEntity> DbSet;

        protected readonly IEntitiesContext Entities;

        protected EntityAsyncRepository(IEntitiesContext entities)
        {
            this.Entities = entities;
            this.DbSet = this.Entities.Set<TEntity>();
        }

        protected Database Database
        {
            get
            {
                return this.Entities.Database;
            }
        }

        public async Task<IEnumerable<TEntity>> FindByAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            bool fromCache,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this.PrepareIncludes(includeProperties);
            return fromCache
                ? await query.Where(predicate).FromCacheAsync()
                : await query.Where(predicate).ToArrayAsync(cancellationToken);
        }

        public Task<IEnumerable<TEntity>> FindByAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool fromCache,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return this.FindByAsync(predicate, CancellationToken.None, fromCache, includeProperties);
        }

        //public IQueryable<TEntity> GetAll(bool fromCache = false)
        //{
        //    return this.DbSet;
        //}

       

        public Task<IEnumerable<TEntity>> GetAllAsync(bool fromCache)
        {
            return this.GetAllAsync(CancellationToken.None, fromCache);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken, bool fromCache)
        {
            //return await this.GetAsync(q => q, cancellationToken);
            return fromCache ? await DbSet.FromCacheAsync() : await this.DbSet.ToArrayAsync(cancellationToken);
        }

        public Task<TEntity> GetAsync(long id)
        {
            return this.DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper,
            CancellationToken cancellationToken,
            bool fromCache)
        {
            var query = queryShaper(this.DbSet);
            return fromCache ? await query.FromCacheAsync() : await query.ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, bool fromCache)
        {
            return await this.GetAsync(queryShaper, CancellationToken.None, fromCache);
        }

        public async Task<IEnumerable<T>> GetByAsync<T>(Func<IQueryable<T>, IQueryable<T>> queryShaper, CancellationToken cancellationToken, bool fromCache) where T : class
        {
            var query = queryShaper(this.Entities.Set<T>());
            return fromCache ? await query.FromCacheAsync() : await query.ToArrayAsync(cancellationToken);
        }

        public Task<IEnumerable<T>> GetByAsync<T>(Func<IQueryable<T>, IQueryable<T>> queryShaper, bool fromCache) where T : class
        {
            return this.GetByAsync(queryShaper, CancellationToken.None, fromCache);
        }

        public async Task<IEnumerable<T>> GetByAsync<T>(
            Expression<Func<T, bool>> predicate,
            bool fromCache = false,
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var query = this.PrepareIncludes(includeProperties);

            return fromCache
                ? await query.Where(predicate).FromCacheAsync()
                : await query.Where(predicate).ToArrayAsync();
        }

        public async Task<TResult> GetAsync<TResult>(
            Func<IQueryable<TEntity>, TResult> queryShaper,
            CancellationToken cancellationToken)
        {
            var factory = Task<TResult>.Factory;
            var result = await factory.StartNew(() => queryShaper(this.DbSet), cancellationToken);
            return result;
        }

        public Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper)
        {
            return this.GetAsync(queryShaper, CancellationToken.None);
        }

        public Task<TEntity> GetFirstAsync(
           Expression<Func<TEntity, bool>> predicate,
           bool fromCache,
           params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var items = this.GetFirstAsync(predicate, CancellationToken.None, fromCache, includeProperties);
            return items;
        }

        public Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            bool fromCache,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var items = fromCache
                ?  this.DbSet.FirstOrDefaultAsync(predicate, cancellationToken)
                :  this.DbSet.Where(predicate).FromCacheFirstOrDefaultAsync();
            return items;
        }

        public Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool fromCache,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return this.GetSingleAsync(predicate, CancellationToken.None, fromCache, includeProperties);
        }

        public async Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            bool fromCache,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var items = await this.FindByAsync(predicate, cancellationToken, fromCache, includeProperties);
            return items.SingleOrDefault();
        }

        private IQueryable<T> PrepareIncludes<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T> query = this.Entities.Set<T>();

            if (includeProperties != null)
            {
                includeProperties.ForEach(property => query = query.Include(property));
            }
            return query;

            //return includeProperties == null
            //    ? query
            //    : includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}