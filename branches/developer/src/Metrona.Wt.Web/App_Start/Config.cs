//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Config.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.App_Start
{
    using System;
    using System.Configuration;

    using Metrona.Wt.Core.Extensions;

    public class Config
    {
        private static readonly Lazy<Config> Lazy = new Lazy<Config>(() => new Config());

        public static Config Instance
        {
            get
            {
                return Lazy.Value;
            }
        }

        public bool EnableAuthenticate
        {
            get
            {
                return ConfigurationManager.AppSettings["EnableAuthenticate"].ConvertTo<bool>();
            }
        }
    }
}