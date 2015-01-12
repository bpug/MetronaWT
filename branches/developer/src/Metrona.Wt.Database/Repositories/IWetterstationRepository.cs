//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IWetterstationRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System.Threading.Tasks;

    using Metrona.Wt.Database.Repositories.Core;
    using Metrona.Wt.Model;

    public interface IWetterStationRepository : IEntityAsyncRepository<WetterStation>
    {
        Task<WetterStation> GetByPlz(int plz);
    }
}