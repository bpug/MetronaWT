//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IBundeslandRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System.Threading.Tasks;

    using Metrona.Wt.Database.Repositories.Core;
    using Metrona.Wt.Model;

    public interface IBundeslandRepository : IEntityAsyncRepository<Bundesland>
    {
        /// <summary>
        /// Gets the by identifier asynch.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;Bundesland&gt;.</returns>
        Task<Bundesland> GetByIdAsynch(int id);
    }
}