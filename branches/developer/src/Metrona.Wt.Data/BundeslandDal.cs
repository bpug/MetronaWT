//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BundeslandRepositoty.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;

    using Metrona.Wt.Model;

    using MySql.Data.MySqlClient;

    public class BundeslandDal
    {
        private static readonly string _strConn;

        static BundeslandDal()
        {
            _strConn = ConfigurationManager.ConnectionStrings["KliBrunata.KlimaContext"].ConnectionString;
        }

        public static List<Bundesland> GetAll()
        {
            string strSql = "SELECT * FROM TAB_BUNDESLAND";

            using (var conn = new MySqlConnection(_strConn))
            {
                try
                {
                    var bundeslender = new List<Bundesland>();
                    var cmd = new MySqlCommand(strSql, conn);
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        bundeslender.Add(GetDataFromReader(reader));
                    }
                    reader.Close();
                    return bundeslender;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static Bundesland GetById(int id)
        {
            string strSql = "SELECT * FROM TAB_BUNDESLAND WHERE  BundeslandId=?id";

            using (var conn = new MySqlConnection(_strConn))
            {
                try
                {
                    var bundeslend = new Bundesland();
                    var cmd = new MySqlCommand(strSql, conn);
                    conn.Open();
                    cmd.Parameters.AddWithValue("?id", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        bundeslend = GetDataFromReader(reader);
                    }
                    reader.Close();
                    return bundeslend;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private static Bundesland GetDataFromReader(IDataRecord reader)
        {
            var bl = new Bundesland();
            if (!Convert.IsDBNull(reader["BundeslandID"]))
            {
                bl.Id = reader.GetInt16(reader.GetOrdinal("BundeslandID"));
            }
            if (!Convert.IsDBNull(reader["Bundesland"]))
            {
                bl.Name = reader.GetString(reader.GetOrdinal("Bundesland"));
            }
            return bl;
        }
    }
}