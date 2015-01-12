//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IEntityRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Metrona.Wt.Model;

    public interface IEntityRepository<TEntity>
        where TEntity : IEntity, new()
    {
        TEntity Get(long id);

        Task<TEntity> GetAsync(long id);

        IQueryable<TEntity> GetAll(bool fromCache = false);

        IQueryable<TEntity> GetAll(bool asNoTracking, bool fromCache);

        IQueryable<T> GetAllBy<T>(
            Expression<Func<T, bool>> predicate, bool fromCache = false,
            params Expression<Func<T, object>>[] includeProperties) where T : class, new();

        IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate, bool fromCache = false,
            params Expression<Func<TEntity, object>>[] includeProperties);
    }
}