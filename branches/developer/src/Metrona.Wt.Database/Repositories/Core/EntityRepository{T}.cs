//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EntityRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories.Core
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using EntityFramework.Extensions;

    using Metrona.Wt.Model;

    public class EntityRepository
    {
        protected readonly IEntitiesContext Entities;

        public EntityRepository(IEntitiesContext entities)
        {
            this.Entities = entities;
        }

        protected Database Database
        {
            get
            {
                return this.Entities.Database;
            }
        }
    }

    public class EntityRepository<TEntity> : EntityRepository, IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        //private readonly IEntitiesContext entities;

        //public EntityRepository(IEntitiesContext entities)
        //{
        //    this.entities = entities;
        //}

        public EntityRepository(IEntitiesContext entities)
            : base(entities)
        {
        }

        public TEntity Get(long id)
        {
            return this.Entities.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetAsync(long id)
        {
            return  await this.Entities.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> GetAll(bool fromCache = false)
        {
            return this.GetAll(false, fromCache);
        }

        public IQueryable<TEntity> GetAll(bool asNoTracking, bool fromCache)
        {
            if (fromCache) return asNoTracking ? this.Entities.Set<TEntity>().AsNoTracking().FromCache().AsQueryable() : this.Entities.Set<TEntity>().FromCache().AsQueryable();
            return asNoTracking ? this.Entities.Set<TEntity>().AsNoTracking() : this.Entities.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate, bool fromCache = false,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var query = this.PrepareIncludes(includeProperties);
            return fromCache ? query.Where(predicate).FromCache().AsQueryable() : query.Where(predicate);
        }

        public IQueryable<T> GetAllBy<T>(
            Expression<Func<T, bool>> predicate, bool fromCache = false,
            params Expression<Func<T, object>>[] includeProperties) where T : class, new()
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var query = this.PrepareIncludes(includeProperties);

            return fromCache ? query.Where(predicate).FromCache().AsQueryable() : query.Where(predicate);
        }

        public DbEntityEntry<TEntity> GetEntry(TEntity entity)
        {
            return this.Entities.Entry(entity);
        }

        public TEntity GetFirstBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var query = this.PrepareIncludes(includeProperties);

            return query.FirstOrDefault(predicate);
        }

        public TEntity GetSingleBy(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var query = this.PrepareIncludes(includeProperties);

            return query.SingleOrDefault(predicate);
        }

        //public IEnumerable<T> GetIncluding(params Expression<Func<T, object>>[] includeProperties)
        //{
        //    var query = this.PrepareIncludes(includeProperties);
        //    return query;
        //}

        private IQueryable<T> PrepareIncludes<T>(params Expression<Func<T, object>>[] includeProperties)where T : class, new()
        {
            IQueryable<T> query = this.Entities.Set<T>();

            return includeProperties == null
                ? query
                : includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}