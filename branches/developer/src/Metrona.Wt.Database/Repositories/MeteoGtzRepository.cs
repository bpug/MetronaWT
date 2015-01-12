//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoGtzRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using EntityFramework.Extensions;

    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Database.Repositories.Core;
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Meteo;

    using MySql.Data.MySqlClient;

    public class MeteoGtzRepository : EntityAsyncRepository<MeteoGtz>, IMeteoGtzRepository
    {
        public MeteoGtzRepository(IEntitiesContext entities)
            : base(entities)
        {
        }

        public async Task<IEnumerable<MeteoGtzBundesland>> GetGtzByBundesland(
            long bundeslandId,
            DateTime startDate,
            DateTime endDate)
        {
            var result =
                this.GetByAsync<MeteoGtzBundesland>(
                    p =>
                        p.BundeslandId == bundeslandId
                        && (DbFunctions.CreateDateTime(p.Jahr, p.Monat, 1, null, null, null) >= startDate)
                        && (DbFunctions.CreateDateTime(p.Jahr, p.Monat, 1, null, null, null) <= endDate),
                    true,
                    p => p.Lgtz,
                    p => p.Promille);
            return  await result;
        }

        //public IEnumerable<MeteoGtzData> GetGtzByBundesland2(long bundeslandId, DateTime startDate, DateTime endDate)
        //{
        //    var result = this.GetAll(
        //        p =>
        //            p.BundeslandPlz.BundeslandId == bundeslandId
        //            && (DbFunctions.CreateDateTime(p.Jahr, p.Monat, 1, null, null, null) >= startDate)
        //            && (DbFunctions.CreateDateTime(p.Jahr, p.Monat, 1, null, null, null) <= endDate),
        //        false,
        //        p => p.Lgtz,
        //        p => p.Promille).GroupBy(
        //            p => new
        //            {
        //                Promile = p.Promille.Anteil,
        //                p.Monat,
        //                p.Jahr
        //            }).Select(
        //                p => new MeteoGtzData
        //                {
        //                    Monat = p.Key.Monat,
        //                    Jahr = p.Key.Jahr,
        //                    Gtz = p.Average(a => a.Gtz),
        //                    Promille = p.Key.Promile,
        //                    Lgtz = p.Average(a => a.Lgtz.Gtz)
        //                }).OrderBy(p => p.Jahr).ThenBy(p => p.Monat);
        //    return result.FromCache();
        //}

        public async Task<IEnumerable<MeteoGtz>> GetGtzByPlz(int plz, DateTime startDate, DateTime endDate)
        {
            var result =
                this.FindByAsync(
                    p =>
                        p.Plz.Equals(plz)
                        && (DbFunctions.CreateDateTime(p.Jahr, p.Monat, 1, null, null, null) >= startDate)
                        && (DbFunctions.CreateDateTime(p.Jahr, p.Monat, 1, null, null, null) <= endDate),
                    true,
                    p => p.Lgtz,
                    p => p.Promille);
            return await result;
        }

        public async Task<IEnumerable<MeteoGtzDeutschland>> GetGtzDeutschland(DateTime startDate, DateTime endDate)
        {
            var result =
                this.GetByAsync<MeteoGtzDeutschland>(
                    p =>
                        DbFunctions.CreateDateTime(p.Jahr, p.Monat, 1, null, null, null) >= startDate
                        && DbFunctions.CreateDateTime(p.Jahr, p.Monat, 1, null, null, null) <= endDate,
                    true,
                    p => p.Lgtz,
                    p => p.Promille);
            return await result;
        }

        public IEnumerable<TestData> GetMonatssummenGTZVor2JahreByPLZ(int plz, DateTime startDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = datumVon1.AddYears(-1);
            var datumBis2 = datumBis1.AddYears(-1);
            var datumVon3 = datumVon2.AddYears(-1);
            var datumBis3 = datumBis2.AddYears(-1);

            //strSql = " SELECT MGTZ.Monat as Monat," & _
            strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat," + " PROM.ANTEIL as Promille,  "
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END )  as StartDate, "
                // + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END ) as StartDate2, "
                //+ datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon3 AND ?datumBis3) THEN MGTZ.GTZ ELSE NULL END ) as StartDate3, "
                //+ datumVon3.ToString("MMM yyyy") + " bis " + datumBis3.ToString("MMM yyyy") + "',"
                     + " LGTZ.GTZ as LGTZ "
                     + " FROM TAB_METEO_GTZ AS MGTZ INNER JOIN  TAB_METEO_LANGGTZ AS LGTZ ON MGTZ.Monat = LGTZ.Monat AND MGTZ.PLZ = LGTZ.PLZ "
                     + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat"
                     + " WHERE MGTZ.plz =?plz AND  ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon3 AND ?datumBis1 ) "
                     + " GROUP BY MGTZ.Monat "
                     + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

            var test = this.Database.SqlQuery<TestData>(
                strSql,
                new MySqlParameter("?datumVon1", datumVon1),
                new MySqlParameter("?datumBis1", datumBis1),
                new MySqlParameter("?datumVon2", datumVon2),
                new MySqlParameter("?datumBis2", datumBis2),
                new MySqlParameter("?datumVon3", datumVon3),
                new MySqlParameter("?datumBis3", datumBis3),
                new MySqlParameter("?plz", plz));
            return test;
        }
    }
}