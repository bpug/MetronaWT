//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BundeslandRepositoty.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Data
{
    using System;
    using System.Configuration;
    using System.Data;

    using Metrona.Wt.Model;

    using MySql.Data.MySqlClient;

    public class StationDal
    {
        private static readonly string _strConn;

        static StationDal()
        {
            _strConn = ConfigurationManager.ConnectionStrings["Brunata.KlimaContext"].ConnectionString;
        }

        public static StationOld GetByPlz(int plz)
        {
            string strSql = "SELECT  TAB_WETTERSTATION.* FROM TAB_WETTERSTATION_PLZ  INNER JOIN TAB_WETTERSTATION ON TAB_WETTERSTATION_PLZ.WSTATI = TAB_WETTERSTATION.WSTATI WHERE     (TAB_WETTERSTATION_PLZ.bis >= ?plz) AND (TAB_WETTERSTATION_PLZ.von <= ?plz) ORDER BY TAB_WETTERSTATION.WSCODE DESC LIMIT 1";

            using (var conn = new MySqlConnection(_strConn))
            {
                try
                {
                    StationOld st = null;
                    var cmd = new MySqlCommand(strSql, conn);
                    conn.Open();
                    cmd.Parameters.AddWithValue("?plz", plz);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        st = GetDataFromReader(reader);
                    }
                    reader.Close();
                    return st;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private static StationOld GetDataFromReader(IDataRecord reader)
        {
            var st = new StationOld();
            if (!Convert.IsDBNull(reader["WSCODE"]))
            {
                st.WsCode = reader.GetInt16(reader.GetOrdinal("WSCODE"));
            }
            if (!Convert.IsDBNull(reader["WSTATI"]))
            {
                st.WStati = reader.GetInt16(reader.GetOrdinal("WSTATI"));
            }

            if (!Convert.IsDBNull(reader["WDIENS"]))
            {
                st.WDiens = reader.GetString(reader.GetOrdinal("WDIENS"));
            }

            if (!Convert.IsDBNull(reader["WDSORT"]))
            {
                st.WdsOrt = reader.GetString(reader.GetOrdinal("WDSORT"));
            }

            return st;
        }
    }
}