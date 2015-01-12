//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Utils.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Core
{
    using System;
    using System.Collections.Generic;

    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Model.Enums;

    public static class Utils
    {
        public static string GetMonthName(int monthNum, string monthformat = "MMM")
        {
            try
            {
                DateTime strDate = new DateTime(1, monthNum, 1);
                return strDate.ToString(monthformat);

            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public static double GetProzentual(double value, double compareValue, int? roundDigits = null)
        {
            var prozent = (value - compareValue) / value * 100;
            if ((!double.IsNaN(prozent)))
            {
                return (roundDigits != null) ? Math.Round(prozent, roundDigits.Value) : prozent;
            }
            return 0;
        }

        public static bool IsHeizMonat(this int monat)
        {
            if ((monat > 5 & monat < 9))
            {
                return false;
            }
            return true;
        }

        public static List<string> GetPeriode(
            DateTime stichTag,
            IntervalType intervalType = IntervalType.M24,
            string dayFormat = "MMM yyyy")
        {
            var datumVon1 = stichTag.GetPastDate(12);
            var datumBis1 = stichTag;
            var datumVon2 = datumVon1.AddYears(-1);
            var datumBis2 = datumBis1.AddYears(-1);
            var datumVon3 = datumVon1.AddYears(-2);
            var datumBis3 = datumBis1.AddYears(-2);

            var periodFormat = "{0:" + dayFormat + "} bis {1:" + dayFormat + "}";

            var result = new List<string>
            {
                string.Format(periodFormat, datumVon1, datumBis1),
                string.Format(periodFormat, datumVon2, datumBis2)
            };
            if (intervalType == IntervalType.M36)
            {
                result.Add(string.Format(periodFormat, datumVon3, datumBis3));
            }
            return result;
        }
    }
}