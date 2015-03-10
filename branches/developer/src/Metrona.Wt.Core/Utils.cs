//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Utils.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Model;
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

        public static List<string> GetZeitraumeFormatted(
            DateTime stichTag,
            IntervalType intervalType = IntervalType.M24,
            string dayFormat = "MMM. yyyy")
        {
            var datumVon1 = stichTag.GetPastDate(12);
            var datumBis1 = stichTag;
            var datumVon2 = datumVon1.AddYears(-1);
            var datumBis2 = datumBis1.AddYears(-1);
            var datumVon3 = datumVon1.AddYears(-2);
            var datumBis3 = datumBis1.AddYears(-2);

            var periodFormat = "{0:" + dayFormat + "} - {1:" + dayFormat + "}";

            var result = new List<string>();
           
            if (intervalType == IntervalType.M36)
            {
                result.Add("Vorvorjahr " + string.Format(periodFormat, datumVon3, datumBis3));
            }
            result.Add("Vorjahr " +  string.Format(periodFormat, datumVon2, datumBis2));
            result.Add("Aktuelles Jahr " + string.Format(periodFormat, datumVon1, datumBis1));
           
            return result;
        }

        public static List<string> GetFormatted(this IEnumerable<Zeitraum> zeitraums, bool withName = false, string dayFormat = "MMM. yyyy")
        {
            
            var periodFormat = "{0:" + dayFormat + "} - {1:" + dayFormat + "}";
            if (withName)
            {
                periodFormat = "{2} {0:" + dayFormat + "} - {1:" + dayFormat + "}";
            }

            var result =  zeitraums.Select(zeitraum => string.Format(periodFormat, zeitraum.Start, zeitraum.End, zeitraum.Name)).ToList();
            return result;
        }

        public static List<Zeitraum> GetZeitraume(
            DateTime stichTag,
            IntervalType intervalType = IntervalType.M24)
        {
            var datumVon1 = stichTag.GetPastDate(12);
            var datumBis1 = stichTag;
            var datumVon2 = datumVon1.AddYears(-1);
            var datumBis2 = datumBis1.AddYears(-1);
            var datumVon3 = datumVon1.AddYears(-2);
            var datumBis3 = datumBis1.AddYears(-2);

            var result = new List<Zeitraum>
            {
                new Zeitraum
                {
                    Name = "Aktuelles Jahr",
                    Start = datumVon1,
                    End = datumBis1
                },
                new Zeitraum
                {
                    Name = "Vorjahr",
                    Start = datumVon2,
                    End = datumBis2
                }
            };

            if (intervalType == IntervalType.M36)
            {
                result.Add(new Zeitraum { Name = "Vorvorjahr", Start = datumVon3, End = datumBis3 });
            }

            return result;
        }
    }
}