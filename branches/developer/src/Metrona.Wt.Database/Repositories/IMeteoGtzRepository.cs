//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IMeteoGtzRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Metrona.Wt.Database.Repositories.Core;
    using Metrona.Wt.Model.Meteo;

    public interface IMeteoGtzRepository : IEntityAsyncRepository<MeteoGtz>
    {
        Task<IEnumerable<MeteoGtzBundesland>> GetGtzByBundesland(long bundeslandId, DateTime startDate, DateTime endDate);

        //IEnumerable<MeteoGtzData> GetGtzByBundesland2(long bundeslandId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<MeteoGtz>> GetGtzByPlz(int plz, DateTime startDate, DateTime endDate);

        Task<IEnumerable<MeteoGtzDeutschland>> GetGtzDeutschland(DateTime startDate, DateTime endDate);

        //IEnumerable<TestData> GetMonatssummenGTZVor2JahreByPLZ(int plz, DateTime startDate);
    }
}