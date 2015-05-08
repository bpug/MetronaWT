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

    /// <summary>
    /// Class BundeslandRepository.
    /// </summary>
    public class BundeslandRepository : EntityAsyncRepository<Bundesland>, IBundeslandRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BundeslandRepository"/> class.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public BundeslandRepository(IEntitiesContext entities)
            : base(entities)
        {
        }

        /// <summary>
        /// Gets the by identifier asynch.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;Bundesland&gt;.</returns>
        public async Task<Bundesland> GetByIdAsynch(int id)
        {
            var result = await this.GetSingleAsync(p => p.Id == id, true);
            return result;
        }
    }
}