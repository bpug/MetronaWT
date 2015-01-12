//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IEntitiesContext.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IEntitiesContext
    {
        int SaveChanges();

        DbSet<T> Set<T>() where T : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Database Database { get; }
    }
}