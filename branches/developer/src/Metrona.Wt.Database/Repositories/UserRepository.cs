//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UserRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System.Threading.Tasks;

    using Metrona.Wt.Database.Models;
    using Metrona.Wt.Database.Repositories.Core;

    public class UserRepository : EntityAsyncRepository<User>, IUserRepository
    {
        public UserRepository(IEntitiesContext entities)
            : base(entities)
        {
        }

        public async Task<User> FindByNameAsync(string name)
        {
            var result = await this.GetFirstAsync(p => p.Username == name, true);
            return result;
        }
    }
}