//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IUserRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System.Threading.Tasks;

    using Metrona.Wt.Database.Models;
    using Metrona.Wt.Database.Repositories.Core;

    public interface IUserRepository : IEntityAsyncRepository<User>
    {
        Task<User> FindByNameAsync(string name);
    }
}