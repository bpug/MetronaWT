//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Configuration.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.App_Start
{
    public class Configuration
    {
        public string ConnectionString
        {
            get
            {
                return ReadConnectionString("Brunata.KlimaContext");
            }
        }

        private static string ReadConnectionString(string connectionStringName)
        {
            //return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            return string.Format("Name={0}", connectionStringName);
        }
    }
}