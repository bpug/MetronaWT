//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ChartDAL.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Data
{
    using System;
    using System.Configuration;
    using System.Data;

    using Metrona.Wt.Core;
    using Metrona.Wt.Core.Extensions;

    using MySql.Data.MySqlClient;

    public class ChartDal
    {
        private static readonly string _strConn;

        static ChartDal()
        {
            // Default Constructor
            _strConn = ConfigurationManager.ConnectionStrings["Brunata.KlimaContext"].ConnectionString;
        }

        public static DataTable GetMonatssummenGTZVorJahrByBundesland(
            int bundeslandID,
            DateTime startDate,
            DateTime endDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = endDate;
            var datumBis2 = DateTimeExtensions.GetPastDate(startDate, 13);

            // strSql = " SELECT MGTZ.Monat as Monat," & _
            strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat,"
                     + " PROM.ANTEIL as Promille,  "
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "',"
                     + " LGTZ.GTZ as LGTZ "
                     + " FROM view_meteo_gtz_bundesland_avg AS MGTZ INNER JOIN  view_meteo_langgtz_bundesland_avg AS LGTZ ON MGTZ.Monat = LGTZ.Monat AND MGTZ.BundeslandID = LGTZ.BundeslandID "
                     + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat"
                     + " WHERE MGTZ.BundeslandID =?bundeslandID AND  ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis1 ) "
                     + " GROUP BY MGTZ.Monat "
                     + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

            using (var conn = new MySqlConnection(_strConn))
            {
                using (var cmd = new MySqlCommand(strSql, conn))
                {
                    cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
                    cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = datumBis1;
                    cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
                    cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = datumBis2;
                    cmd.Parameters.Add("?bundeslandID", MySqlDbType.Int32).Value = bundeslandID;
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        public static DataTable GetMonatssummenGTZVor2JahreByBundesland(
            int bundeslandID,
            DateTime startDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = datumVon1.AddYears(-1);
            var datumBis2 = datumBis1.AddYears(-1);
            var datumVon3 = datumVon2.AddYears(-1);
            var datumBis3 = datumBis2.AddYears(-1);

            // strSql = " SELECT MGTZ.Monat as Monat," & _
            strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat,"
                     + " PROM.ANTEIL as Promille, "
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "',"
                      + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon3 AND ?datumBis3) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon3.ToString("MMM yyyy") + " bis " + datumBis3.ToString("MMM yyyy") + "',"
                     + " LGTZ.GTZ as LGTZ " 
                     + " FROM view_meteo_gtz_bundesland_avg AS MGTZ INNER JOIN  view_meteo_langgtz_bundesland_avg AS LGTZ ON MGTZ.Monat = LGTZ.Monat AND MGTZ.BundeslandID = LGTZ.BundeslandID "
                     + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat"
                     + " WHERE MGTZ.BundeslandID =?bundeslandID AND  ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon3 AND ?datumBis1 ) "
                     + " GROUP BY MGTZ.Monat "
                     + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

            using (var conn = new MySqlConnection(_strConn))
            {
                using (var cmd = new MySqlCommand(strSql, conn))
                {
                    cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
                    cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = datumBis1;
                    cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
                    cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = datumBis2;
                    cmd.Parameters.Add("?datumVon3", MySqlDbType.Date).Value = datumVon3;
                    cmd.Parameters.Add("?datumBis3", MySqlDbType.Date).Value = datumBis3;
                    cmd.Parameters.Add("?bundeslandID", MySqlDbType.Int32).Value = bundeslandID;
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }


        public static DataTable GetMonatssummenGTZVorJahrByPLZ(int plz, DateTime startDate, DateTime endDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = endDate;
            var datumBis2 = DateTimeExtensions.GetPastDate(startDate, 13);

            //strSql = " SELECT MGTZ.Monat as Monat," & _
            strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat,"
                     + " PROM.ANTEIL as Promille,  "
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END )  as '"
                     + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "',"
                     + " LGTZ.GTZ as LGTZ " 
                     + " FROM TAB_METEO_GTZ AS MGTZ INNER JOIN  TAB_METEO_LANGGTZ AS LGTZ ON MGTZ.Monat = LGTZ.Monat AND MGTZ.PLZ = LGTZ.PLZ "
                     + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat"
                     + " WHERE MGTZ.plz =?plz AND  ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis1 ) "
                     + " GROUP BY MGTZ.Monat "
                     + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

            using (var conn = new MySqlConnection(_strConn))
            {
                using (var cmd = new MySqlCommand(strSql, conn))
                {
                    cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
                    cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = datumBis1;
                    cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
                    cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = datumBis2;
                    cmd.Parameters.Add("?plz", MySqlDbType.Int32).Value = plz;
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }


        public static DataTable GetMonatssummenGTZVor2JahreByPLZ(int plz, DateTime startDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = datumVon1.AddYears(-1);
            var datumBis2 = datumBis1.AddYears(-1);
            var datumVon3 = datumVon2.AddYears(-1);
            var datumBis3 = datumBis2.AddYears(-1);

            //strSql = " SELECT MGTZ.Monat as Monat," & _
            strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat,"
                     + " PROM.ANTEIL as Promille,  "
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END )  as '"
                     + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon3 AND ?datumBis3) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon3.ToString("MMM yyyy") + " bis " + datumBis3.ToString("MMM yyyy") + "',"
                     + " LGTZ.GTZ as LGTZ "
                     + " FROM TAB_METEO_GTZ AS MGTZ INNER JOIN  TAB_METEO_LANGGTZ AS LGTZ ON MGTZ.Monat = LGTZ.Monat AND MGTZ.PLZ = LGTZ.PLZ "
                     + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat"
                     + " WHERE MGTZ.plz =?plz AND  ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon3 AND ?datumBis1 ) "
                     + " GROUP BY MGTZ.Monat "
                     + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

            using (var conn = new MySqlConnection(_strConn))
            {
                using (var cmd = new MySqlCommand(strSql, conn))
                {
                    cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
                    cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = datumBis1;
                    cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
                    cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = datumBis2;
                    cmd.Parameters.Add("?datumVon3", MySqlDbType.Date).Value = datumVon3;
                    cmd.Parameters.Add("?datumBis3", MySqlDbType.Date).Value = datumBis3;
                    cmd.Parameters.Add("?plz", MySqlDbType.Int32).Value = plz;
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        public static DataTable GetMonatssummenGTZVorJahrDeutschland(DateTime startDate, DateTime endDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = endDate;
            var datumBis2 = DateTimeExtensions.GetPastDate(startDate, 13);

            //strSql = " SELECT MGTZ.Monat as Monat," & _
            strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat,"
                     + " PROM.ANTEIL as Promille,  "
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "',"
                     + " LGTZ.GTZ as LGTZ "
                     + " FROM view_meteo_gtz_deutschland_avg AS MGTZ INNER JOIN  view_meteo_langgtz_deutschland_avg AS LGTZ ON MGTZ.Monat = LGTZ.Monat "
                     + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat"
                     + " WHERE ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis1 ) "
                     + " GROUP BY MGTZ.Monat "
                     + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

            using (var conn = new MySqlConnection(_strConn))
            {
                using (var cmd = new MySqlCommand(strSql, conn))
                {
                    cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
                    cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = datumBis1;
                    cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
                    cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = datumBis2;
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        public static DataTable GetMonatssummenGTZVor2JahreDeutschland(DateTime startDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = datumVon1.AddYears(-1);
            var datumBis2 = datumBis1.AddYears(-1);
            var datumVon3 = datumVon2.AddYears(-1);
            var datumBis3 = datumBis2.AddYears(-1);

            //strSql = " SELECT MGTZ.Monat as Monat," & _
            strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat,"
                     + " PROM.ANTEIL as Promille,  "
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "',"
                      + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon3 AND ?datumBis3) THEN MGTZ.GTZ ELSE NULL END ) as '"
                     + datumVon3.ToString("MMM yyyy") + " bis " + datumBis3.ToString("MMM yyyy") + "',"
                     + " LGTZ.GTZ as LGTZ "
                     + " FROM view_meteo_gtz_deutschland_avg AS MGTZ INNER JOIN  view_meteo_langgtz_deutschland_avg AS LGTZ ON MGTZ.Monat = LGTZ.Monat "
                     + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat"
                     + " WHERE ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon3 AND ?datumBis1 ) "
                     + " GROUP BY MGTZ.Monat "
                     + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

            using (var conn = new MySqlConnection(_strConn))
            {
                using (var cmd = new MySqlCommand(strSql, conn))
                {
                    cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
                    cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = datumBis1;
                    cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
                    cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = datumBis2;
                    cmd.Parameters.Add("?datumVon3", MySqlDbType.Date).Value = datumVon3;
                    cmd.Parameters.Add("?datumBis3", MySqlDbType.Date).Value = datumBis3;
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }


        public static DataTable GetTemperaturByBundesland(int bundeslandID, DateTime startDate, DateTime endDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = endDate;
            var datumBis2 = DateTimeExtensions.GetPastDate(startDate, 13);

            strSql = "SELECT  DATE_FORMAT(WDITAG,'%m-%d')  AS Datum,"
                     + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon1 AND ?datumBis1) THEN TEMPMI ELSE NULL END )  as '"
                     + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon2 AND ?datumBis2) THEN TEMPMI ELSE NULL END ) as '"
                     + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "' "
                     + ", 15 AS Heizgrenztemperatur " + " FROM  " + " ( "
                     + "   SELECT * FROM view_klima_temp_bundesland_avg "
                     + "   WHERE BundeslandID = ?bundeslandID  AND WDITAG BETWEEN ?datumVon AND ?datumBis "
                     + "   ORDER BY WDITAG " + " ) AS Klima " + " GROUP by Datum " + " ORDER BY WDITAG ";

            using (var conn = new MySqlConnection(_strConn))
            {
                using (var cmd = new MySqlCommand(strSql, conn))
                {
                    cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
                    //
                    cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = DateTimeExtensions.GetLastDayOfMonth(datumBis1);
                    cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
                    cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = DateTimeExtensions.GetLastDayOfMonth(datumBis2);

                    cmd.Parameters.Add("?datumVon", MySqlDbType.Date).Value = endDate;
                    cmd.Parameters.Add("?datumBis", MySqlDbType.Date).Value = DateTimeExtensions.GetLastDayOfMonth(startDate);
                    cmd.Parameters.Add("?bundeslandID", MySqlDbType.Int32).Value = bundeslandID;
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        public static DataTable GetTemperaturByWSCode(int wscode, DateTime startDate, DateTime endDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = endDate;
            var datumBis2 = DateTimeExtensions.GetPastDate(startDate, 13);

            strSql = "SELECT  DATE_FORMAT(WDITAG,'%m-%d')  AS Datum,"
                     + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon1 AND ?datumBis1) THEN TEMPMI ELSE NULL END )  as '" + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon2 AND ?datumBis2) THEN TEMPMI ELSE NULL END ) as '" + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "' "
                     + ", 15 AS Heizgrenztemperatur " +
                     " FROM  " +
                     " ( " + "   SELECT * FROM TAB_KLIMA "
                     + "   WHERE WSCODE = ?wscode  AND WDITAG BETWEEN ?datumVon AND ?datumBis " + "   ORDER BY WDITAG "
                     + " ) AS Klima " + " GROUP by Datum " + " ORDER BY WDITAG ";

            using (var conn = new MySqlConnection(_strConn))
            {
                using (var cmd = new MySqlCommand(strSql, conn))
                {
                    cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
                    //
                    cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = DateTimeExtensions.GetLastDayOfMonth(datumBis1);
                    cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
                    cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = DateTimeExtensions.GetLastDayOfMonth(datumBis2);

                    cmd.Parameters.Add("?datumVon", MySqlDbType.Date).Value = endDate;
                    cmd.Parameters.Add("?datumBis", MySqlDbType.Date).Value = DateTimeExtensions.GetLastDayOfMonth(startDate);
                    cmd.Parameters.Add("?wscode", MySqlDbType.Int32).Value = wscode;
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        public static DataTable GetTemperaturDeutschland(DateTime startDate, DateTime endDate)
        {
            string strSql = null;
            var datumVon1 = DateTimeExtensions.GetPastDate(startDate, 12);
            var datumBis1 = startDate;
            var datumVon2 = endDate;
            var datumBis2 = DateTimeExtensions.GetPastDate(startDate, 13);

            strSql = "SELECT  DATE_FORMAT(WDITAG,'%m-%d')  AS Datum,"
                     + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon1 AND ?datumBis1) THEN TEMPMI ELSE NULL END )  as '"
                     + datumVon1.ToString("MMM yyyy") + " bis " + datumBis1.ToString("MMM yyyy") + "',"
                     + " SUM( CASE WHEN (WDITAG BETWEEN ?datumVon2 AND ?datumBis2) THEN TEMPMI ELSE NULL END ) as '"
                     + datumVon2.ToString("MMM yyyy") + " bis " + datumBis2.ToString("MMM yyyy") + "' "
                     + ", 15 AS Heizgrenztemperatur " + " FROM  " + " ( "
                     + "   SELECT * FROM view_klima_temp_deutschland_avg "
                     + "   WHERE WDITAG BETWEEN ?datumVon AND ?datumBis " + "   ORDER BY WDITAG " + " ) AS Klima "
                     + " GROUP by Datum " + " ORDER BY WDITAG ";

            using (var conn = new MySqlConnection(_strConn))
            {
                using (var cmd = new MySqlCommand(strSql, conn))
                {
                    cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
                    //
                    cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = DateTimeExtensions.GetLastDayOfMonth(datumBis1);
                    cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
                    cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = DateTimeExtensions.GetLastDayOfMonth(datumBis2);

                    cmd.Parameters.Add("?datumVon", MySqlDbType.Date).Value = endDate;
                    cmd.Parameters.Add("?datumBis", MySqlDbType.Date).Value = DateTimeExtensions.GetLastDayOfMonth(startDate);
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        //public static DataTable GetMonatssummenGTZVorMonatByBundesland(int bundeslandID, DateTime startDate)
        //{
        //    string strSql = null;
        //    var datumVon1 = startDate;
        //    var datumVon2 = startDate.AddMonths(-1);

        //    strSql = " SELECT " + " STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') as Monat, "
        //             + " MGTZ.GTZ  AS GTZ," + " LGTZ.GTZ as LGTZ "
        //             + " FROM view_meteo_gtz_bundesland_avg AS MGTZ INNER JOIN  view_meteo_langgtz_bundesland_avg AS LGTZ ON MGTZ.Monat = LGTZ.Monat AND MGTZ.BundeslandID = LGTZ.BundeslandID "
        //             + " WHERE MGTZ.BundeslandID =?bundeslandID AND  ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumVon1 ) "
        //             + " GROUP BY MGTZ.Monat "
        //             + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

        //    using (var conn = new MySqlConnection(_strConn))
        //    {
        //        using (var cmd = new MySqlCommand(strSql, conn))
        //        {
        //            cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
        //            cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
        //            cmd.Parameters.Add("?bundeslandID", MySqlDbType.Int32).Value = bundeslandID;
        //            using (var da = new MySqlDataAdapter(cmd))
        //            {
        //                using (var dt = new DataTable())
        //                {
        //                    da.Fill(dt);
        //                    return dt;
        //                }
        //            }
        //        }
        //    }
        //}

        //public static DataTable GetMonatssummenGTZVorMonatByPLZ(int plz, DateTime startDate)
        //{
        //    string strSql = null;
        //    var datumVon1 = startDate;
        //    var datumVon2 = startDate.AddMonths(-1);

        //    strSql = " SELECT " + " STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') as Monat, "
        //             + " MGTZ.GTZ  AS GTZ," + " LGTZ.GTZ as LGTZ "
        //             + " FROM TAB_METEO_GTZ AS MGTZ INNER JOIN  TAB_METEO_LANGGTZ AS LGTZ ON MGTZ.Monat = LGTZ.Monat AND MGTZ.PLZ = LGTZ.PLZ "
        //             + " WHERE MGTZ.plz =?plz AND  ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumVon1 ) "
        //             + " GROUP BY MGTZ.Monat "
        //             + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

        //    using (var conn = new MySqlConnection(_strConn))
        //    {
        //        using (var cmd = new MySqlCommand(strSql, conn))
        //        {
        //            cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
        //            cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
        //            cmd.Parameters.Add("?plz", MySqlDbType.Int32).Value = plz;
        //            using (var da = new MySqlDataAdapter(cmd))
        //            {
        //                using (var dt = new DataTable())
        //                {
        //                    da.Fill(dt);
        //                    return dt;
        //                }
        //            }
        //        }
        //    }
        //}

        //public static DataTable GetMonatssummenGTZVorMonatDeutschland(DateTime startDate)
        //{
        //    string strSql = null;
        //    var datumVon1 = startDate;
        //    var datumVon2 = startDate.AddMonths(-1);

        //    strSql = " SELECT " + " STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') as Monat, "
        //             + " MGTZ.GTZ  AS GTZ," + " LGTZ.GTZ as LGTZ "
        //             + " FROM view_meteo_gtz_deutschland_avg AS MGTZ INNER JOIN  view_meteo_langgtz_deutschland_avg AS LGTZ ON MGTZ.Monat = LGTZ.Monat "
        //             + " WHERE ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumVon1 ) "
        //             + " GROUP BY MGTZ.Monat "
        //             + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

        //    using (var conn = new MySqlConnection(_strConn))
        //    {
        //        using (var cmd = new MySqlCommand(strSql, conn))
        //        {
        //            cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = datumVon1;
        //            cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = datumVon2;
        //            using (var da = new MySqlDataAdapter(cmd))
        //            {
        //                using (var dt = new DataTable())
        //                {
        //                    da.Fill(dt);
        //                    return dt;
        //                }
        //            }
        //        }
        //    }
        //}

        //public static DataTable GetRelativeVerteilungVorJahrByBundesland(
        //    int bundeslandID,
        //    DateTime startDate,
        //    DateTime endDate)
        //{
        //    string strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat," + " PROM.ANTEIL as Promille,  " + "( "
        //                    + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) - "
        //                    + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END )  "
        //                    + " )/sumYear*100 AS Vorjahr " + ",(  "
        //                    + "   SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) - "
        //                    + "   LGTZ.GTZ " + " )/sumYear*100 AS LGTZ "
        //                    + " FROM view_meteo_gtz_bundesland_avg AS MGTZ  INNER JOIN  view_meteo_langgtz_bundesland_avg AS LGTZ ON MGTZ.Monat = LGTZ.Monat AND MGTZ.BundeslandID = LGTZ.BundeslandID "
        //                    + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat, " + "(\t"
        //                    + "   SELECT  SUM( GTZ ) as sumYear FROM view_meteo_gtz_bundesland_avg WHERE BundeslandID = ?bundeslandID AND (STR_TO_DATE(CONCAT_WS('/',Jahr,Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) "
        //                    + " ) as t2 "
        //                    + " WHERE MGTZ.BundeslandID =?bundeslandID AND  ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis1 ) "
        //                    + " GROUP BY MGTZ.Monat "
        //                    + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

        //    using (var conn = new MySqlConnection(_strConn))
        //    {
        //        using (var cmd = new MySqlCommand(strSql, conn))
        //        {
        //            cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = Utils.GetPastDate(startDate, 12);
        //            cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = startDate;
        //            cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = endDate;
        //            cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = Utils.GetPastDate(startDate, 13);
        //            cmd.Parameters.Add("?bundeslandID", MySqlDbType.Int32).Value = bundeslandID;
        //            using (var da = new MySqlDataAdapter(cmd))
        //            {
        //                using (var dt = new DataTable())
        //                {
        //                    da.Fill(dt);
        //                    return dt;
        //                }
        //            }
        //        }
        //    }
        //}

        //public static DataTable GetRelativeVerteilungVorJahrByPLZ(int plz, DateTime startDate, DateTime endDate)
        //{
        //    string strSql = null;

        //    strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat," + " PROM.ANTEIL as Promille,  " + "( "
        //             + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) - "
        //             + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END )  "
        //             + " )/sumYear*100 AS Vorjahr, " + "(  "
        //             + "   SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) - "
        //             + "   LGTZ.GTZ " + " )/sumYear*100 AS LGTZ "
        //             + " FROM TAB_METEO_GTZ AS MGTZ  INNER JOIN  TAB_METEO_LANGGTZ AS LGTZ ON MGTZ.Monat = LGTZ.Monat AND MGTZ.PLZ = LGTZ.PLZ "
        //             + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat, " + "(\t"
        //             + "   SELECT  SUM( GTZ ) as sumYear FROM TAB_METEO_GTZ WHERE plz = ?plz AND (STR_TO_DATE(CONCAT_WS('/',Jahr,Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) "
        //             + " ) as t2 "
        //             + " WHERE MGTZ.plz =?plz AND  ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis1 ) "
        //             + " GROUP BY MGTZ.Monat "
        //             + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

        //    using (var conn = new MySqlConnection(_strConn))
        //    {
        //        using (var cmd = new MySqlCommand(strSql, conn))
        //        {
        //            cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = Utils.GetPastDate(startDate, 12);
        //            cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = startDate;
        //            cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = endDate;
        //            cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = Utils.GetPastDate(startDate, 13);
        //            cmd.Parameters.Add("?plz", MySqlDbType.Int32).Value = plz;
        //            using (var da = new MySqlDataAdapter(cmd))
        //            {
        //                using (var dt = new DataTable())
        //                {
        //                    da.Fill(dt);
        //                    return dt;
        //                }
        //            }
        //        }
        //    }
        //}

        //public static DataTable GetRelativeVerteilungVorJahrDeutschland(DateTime startDate, DateTime endDate)
        //{
        //    string strSql = null;

        //    strSql = " SELECT CAST(MGTZ.Monat AS CHAR(2)) as Monat," + " PROM.ANTEIL as Promille,  " + "( "
        //             + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) - "
        //             + " SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis2) THEN MGTZ.GTZ ELSE NULL END )  "
        //             + " )/sumYear*100 AS Vorjahr " + ",(  "
        //             + "   SUM( CASE WHEN (STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) THEN MGTZ.GTZ ELSE NULL END ) - "
        //             + "   LGTZ.GTZ " + " )/sumYear*100 AS LGTZ "
        //             + " FROM view_meteo_gtz_deutschland_avg AS MGTZ  INNER JOIN  view_meteo_langgtz_deutschland_avg AS LGTZ ON MGTZ.Monat = LGTZ.Monat "
        //             + " INNER JOIN  TAB_PROMILLE AS PROM ON MGTZ.Monat = PROM.Monat, " + "(\t"
        //             + "   SELECT  SUM( GTZ ) as sumYear FROM view_meteo_gtz_deutschland_avg WHERE (STR_TO_DATE(CONCAT_WS('/',Jahr,Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon1 AND ?datumBis1) "
        //             + " ) as t2 "
        //             + " WHERE ( STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') BETWEEN ?datumVon2 AND ?datumBis1 ) "
        //             + " GROUP BY MGTZ.Monat "
        //             + " ORDER BY STR_TO_DATE(CONCAT_WS('/',MGTZ.Jahr,MGTZ.Monat,'01'),'%Y/%m/%d') ";

        //    using (var conn = new MySqlConnection(_strConn))
        //    {
        //        using (var cmd = new MySqlCommand(strSql, conn))
        //        {
        //            cmd.Parameters.Add("?datumVon1", MySqlDbType.Date).Value = Utils.GetPastDate(startDate, 12);
        //            cmd.Parameters.Add("?datumBis1", MySqlDbType.Date).Value = startDate;
        //            cmd.Parameters.Add("?datumVon2", MySqlDbType.Date).Value = endDate;
        //            cmd.Parameters.Add("?datumBis2", MySqlDbType.Date).Value = Utils.GetPastDate(startDate, 13);
        //            using (var da = new MySqlDataAdapter(cmd))
        //            {
        //                using (var dt = new DataTable())
        //                {
        //                    da.Fill(dt);
        //                    return dt;
        //                }
        //            }
        //        }
        //    }
        //}

       
    }
}