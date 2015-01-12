//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SessionData.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web
{
    using System.Web;

    using Metrona.Wt.Model;

    public class SessionData
    {
        private const string CALCULATE_REQUEST = "CALCULATE_REQUEST";

        private const string TEMPERATUR_DRILL_MONAT = "SD_TEMPERATUR_DRILL_MONAT";

        public static CalculateRequest CalculateRequest
        {
            get
            {
                var value = HttpContext.Current.Session[CALCULATE_REQUEST] as CalculateRequest;
                return value;
            }
            set
            {
                HttpContext.Current.Session[CALCULATE_REQUEST] = value;
            }
        }

        public static int TemperaturDrillMonat
        {
            get
            {
                var value = HttpContext.Current.Session[TEMPERATUR_DRILL_MONAT] as int?;
                return value.GetValueOrDefault();
            }
            set
            {
                HttpContext.Current.Session[TEMPERATUR_DRILL_MONAT] = value;
            }
        }
    }
}