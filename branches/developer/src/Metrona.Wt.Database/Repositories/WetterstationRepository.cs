//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WetterstationRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using EntityFramework.Extensions;

    using Metrona.Wt.Database.Repositories.Core;
    using Metrona.Wt.Model;

    public class WetterStationRepository : EntityAsyncRepository<WetterStation>, IWetterStationRepository
    {
        public WetterStationRepository(IEntitiesContext entities)
            : base(entities)
        {
        }

        public async Task<WetterStation> GetByPlz(int plz)
        {
            //            string strSql =
            //                @"SELECT  TAB_WETTERSTATION.* FROM TAB_WETTERSTATION_PLZ  INNER JOIN TAB_WETTERSTATION ON TAB_WETTERSTATION_PLZ.WSTATI = TAB_WETTERSTATION.WSTATI 
            //                            WHERE  (TAB_WETTERSTATION_PLZ.bis >= ?plz) AND (TAB_WETTERSTATION_PLZ.von <= ?plz) ORDER BY TAB_WETTERSTATION.WSCODE DESC LIMIT 1";
            //            var station = this.Database.SqlQuery<WetterStation>(strSql, new MySqlParameter("?plz", plz));
            //            return station.FirstOrDefault();

            var station = await this.GetFirstAsync(p => p.Plzs.Any(p1 => (p1.Von <= plz && p1.Bis >= plz)), false);
            return station;
        }
    }
}