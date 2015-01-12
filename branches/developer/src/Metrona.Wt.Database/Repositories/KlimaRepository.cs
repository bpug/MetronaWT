//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    using Metrona.Wt.Database.Repositories.Core;
    using Metrona.Wt.Model.Klima;

  

    public class KlimaRepository : EntityAsyncRepository<Klima>, IKlimaRepository
    {
        public KlimaRepository(IEntitiesContext entities)
            : base(entities)
        {
        }

        public async Task<IEnumerable<KlimaTemperatur>> GetTemperaturByWsCode(int wscode, DateTime startDate, DateTime endDate)
        {
            var result = (await this.GetAsync( q=> q.Where(p => p.WsCode.Equals(wscode) && (p.Datum >= startDate && p.Datum <= endDate)).OrderBy(p=> p.Datum), true))
                .Select(p => new KlimaTemperatur
                {
                    Datum = p.Datum,
                    Temperatur = p.Temperatur
                })
                .OrderBy(p => p.Datum);
            return result;
        }

        public async Task<IEnumerable<KlimaTemperatur>> GetTemperaturByBundesland(int bundeslandId, DateTime startDate, DateTime endDate)
        {
            var result = (await this.GetByAsync<KlimaTemperaturBundesland>( q => q.Where(p => p.BundeslandId.Equals(bundeslandId) && (p.Datum >= startDate && p.Datum <= endDate)).OrderBy(p=> p.Datum), true))
                .Select(p => new KlimaTemperatur
                {
                    Datum = p.Datum,
                    Temperatur = p.Temperatur
                })
                .OrderBy(p => p.Datum);

            return result;
        }

        public async Task<IEnumerable<KlimaTemperatur>> GetTemperaturDeutschland(DateTime startDate, DateTime endDate)
        {

            var result = (await this.GetByAsync<KlimaTemperaturDeutschland>( q => q.Where(p =>  p.Datum >= startDate && p.Datum <= endDate).OrderBy(p=> p.Datum), true))
                .Select(p => new KlimaTemperatur
                {
                    Datum = p.Datum,
                    Temperatur = p.Temperatur
                })
                .OrderBy(p => p.Datum);

            return result;
        }

       // public IEnumerable<KlimaTemperaturPeriod> GetTemperaturByWsCode2(int wscode, DateTime startDate, DateTime endDate)
       // {
       //     var datumVon1 = startDate.GetPastDate(12);
       //     var datumBis1 = startDate;
       //     var datumVon2 = endDate;
       //     var datumBis2 = startDate.GetPastDate(13);

       //     var strSql = "SELECT  DATE_FORMAT(WDITAG,'%m-%d')  AS Datum,"
       //              + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon1 AND ?datumBis1) THEN TEMPMI ELSE NULL END )  as Period1, " 
       //              //+ datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
       //              + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon2 AND ?datumBis2) THEN TEMPMI ELSE NULL END ) as Period2, " 
       //              //+ datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "' "
       //              + " 15 AS Heizgrenztemperatur " +
       //              " FROM  " +
       //              " ( " + "   SELECT * FROM TAB_KLIMA "
       //              + "   WHERE WSCODE = ?wscode  AND WDITAG BETWEEN ?datumVon AND ?datumBis " + "   ORDER BY WDITAG "
       //              + " ) AS Klima " + " GROUP by Datum " + " ORDER BY WDITAG ";

       //     var result = this.Database.SqlQuery<KlimaTemperaturPeriod>(strSql,
       //          new MySqlParameter("?datumVon1", datumVon1),
       //          new MySqlParameter("?datumBis1", datumBis1.GetLastDayOfMonth()),
       //          new MySqlParameter("?datumVon2", datumVon2),
       //          new MySqlParameter("?datumBis2", datumBis2.GetLastDayOfMonth()),
       //          new MySqlParameter("?datumVon", endDate),
       //          new MySqlParameter("?datumBis", startDate.GetLastDayOfMonth()),
       //          new MySqlParameter("?wscode", wscode));
       //     return result;
       //}

       // public IEnumerable<KlimaTemperaturPeriod> GetTemperaturByBundesland2(int bundeslandId, DateTime startDate, DateTime endDate)
       // {
       //     var datumVon1 = startDate.GetPastDate(12);
       //     var datumBis1 = startDate;
       //     var datumVon2 = endDate;
       //     var datumBis2 = startDate.GetPastDate(13);

       //     var strSql = "SELECT  DATE_FORMAT(WDITAG,'%m-%d')  AS Datum,"
       //              + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon1 AND ?datumBis1) THEN TEMPMI ELSE NULL END )  as Period1, "
       //              //+ datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
       //              + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon2 AND ?datumBis2) THEN TEMPMI ELSE NULL END ) as Period2, "
       //             // + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "' "
       //              + " 15 AS Heizgrenztemperatur " + " FROM  " + " ( "
       //              + "   SELECT * FROM view_klima_temp_bundesland_avg "
       //              + "   WHERE BundeslandID = ?bundeslandID  AND WDITAG BETWEEN ?datumVon AND ?datumBis "
       //              + "   ORDER BY WDITAG " + " ) AS Klima " + " GROUP by Datum " + " ORDER BY WDITAG ";

       //     var result = this.Database.SqlQuery<KlimaTemperaturPeriod>(strSql,
       //         new MySqlParameter("?datumVon1", datumVon1),
       //         new MySqlParameter("?datumBis1", datumBis1.GetLastDayOfMonth()),
       //         new MySqlParameter("?datumVon2", datumVon2),
       //         new MySqlParameter("?datumBis2", datumBis2.GetLastDayOfMonth()),
       //         new MySqlParameter("?datumVon", endDate),
       //         new MySqlParameter("?datumBis", startDate.GetLastDayOfMonth()),
       //         new MySqlParameter("?bundeslandID", bundeslandId));

       //     return result;
            
       // }

       // public IEnumerable<KlimaTemperaturPeriod> GetTemperaturDeutschland2(DateTime startDate, DateTime endDate)
       // {
       //     var datumVon1 = startDate.GetPastDate(12);
       //     var datumBis1 = startDate;
       //     var datumVon2 = endDate;
       //     var datumBis2 = startDate.GetPastDate(13);

       //     var strSql = "SELECT  DATE_FORMAT(WDITAG,'%m-%d')  AS Datum,"
       //              + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon1 AND ?datumBis1) THEN TEMPMI ELSE NULL END )  as as Period1, "
       //              //+ datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
       //              + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon2 AND ?datumBis2) THEN TEMPMI ELSE NULL END ) as as Period2 "
       //              //+ datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "' "
       //              + " 15 AS Heizgrenztemperatur " + " FROM  " + " ( "
       //              + "   SELECT * FROM view_klima_temp_deutschland_avg "
       //              + "   WHERE WDITAG BETWEEN ?datumVon AND ?datumBis " + "   ORDER BY WDITAG " + " ) AS Klima "
       //              + " GROUP by Datum " + " ORDER BY WDITAG ";

       //     var result = this.Database.SqlQuery<KlimaTemperaturPeriod>(strSql,
       //        new MySqlParameter("?datumVon1", datumVon1),
       //        new MySqlParameter("?datumBis1", datumBis1.GetLastDayOfMonth()),
       //        new MySqlParameter("?datumVon2", datumVon2),
       //        new MySqlParameter("?datumBis2", datumBis2.GetLastDayOfMonth()),
       //        new MySqlParameter("?datumVon", endDate),
       //        new MySqlParameter("?datumBis", startDate.GetLastDayOfMonth()));

       //     return result;
       // }
        
    }
}
