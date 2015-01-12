//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BundeslandRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System.Threading.Tasks;

    using Metrona.Wt.Database.Repositories.Core;
    using Metrona.Wt.Model;

    public class BundeslandRepository : EntityAsyncRepository<Bundesland>, IBundeslandRepository
    {
        public BundeslandRepository(IEntitiesContext entities)
            : base(entities)
        {
        }

        public async Task<Bundesland> GetByIdAsynch(int id)
        {
            var result = await this.GetSingleAsync(p => p.Id == id, true);
            return result;
        }
    }
}