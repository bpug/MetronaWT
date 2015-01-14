//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaContextInitializer.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database
{
    using System;
    using System.Data.Entity;

    using Metrona.Wt.Database.Models;

    using MySql.Data.MySqlClient;
    using MySql.Data.Entity;

    public class KlimaContextInitializer : IDatabaseInitializer<KlimaContext>
    {
        public void InitializeDatabase(KlimaContext context)
        {
            if (context.Database.Exists())
            {
                if (context.Database.CompatibleWithModel(false))
                {
                    this.CreateDateTimeFunction(context);
                }
            }
        }

        private void CreateDateTimeFunction(KlimaContext context)
        {
            string sql = @"DROP FUNCTION IF EXISTS CreateDateTime;
            CREATE FUNCTION CreateDateTime(year int, month int, day int, hours int, min int, sec int) RETURNS Date
            DETERMINISTIC
            BEGIN
                RETURN DATE(CONCAT_WS('-', year, month, day)); 
            END;";
            try
            {
                context.Database.ExecuteSqlCommand(sql);
            }
            catch (MySqlException e)
            {
                throw new ApplicationException("Execute the following in the MySQL console: SET GLOBAL log_bin_trust_function_creators = 1;", e);
            }
            
        }
    }
}