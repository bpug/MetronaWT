//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IKlimaRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Metrona.Wt.Database.Repositories.Core;
    using Metrona.Wt.Model.Klima;

    public interface IKlimaRepository : IEntityAsyncRepository<Klima>
    {
        Task<IEnumerable<KlimaTemperatur>> GetTemperaturByBundesland(
            int bundeslandId,
            DateTime startDate,
            DateTime endDate);

        Task<IEnumerable<KlimaTemperatur>> GetTemperaturByWsCode(int wscode, DateTime startDate, DateTime endDate);

        Task<IEnumerable<KlimaTemperatur>> GetTemperaturDeutschland(DateTime startDate, DateTime endDate);

        //IEnumerable<KlimaTemperaturPeriod> GetTemperaturDeutschland2(DateTime startDate, DateTime endDate);

        //IEnumerable<KlimaTemperaturPeriod> GetTemperaturByWsCode2(int wscode, DateTime startDate, DateTime endDate);

        //IEnumerable<KlimaTemperaturPeriod> GetTemperaturByBundesland2(int bundeslandId, DateTime startDate, DateTime endDate);
    }
}