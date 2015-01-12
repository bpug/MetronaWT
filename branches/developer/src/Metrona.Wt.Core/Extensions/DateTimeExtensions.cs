//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DateTimeExtensions.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Core.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime GetFirstDayOfMonth(this DateTime myDate)
        {
            return new DateTime(myDate.Year, myDate.Month, 1);
        }

        public static DateTime GetLastDayOfMonth(this DateTime myDate)
        {
            return new DateTime(myDate.Year, myDate.Month, DateTime.DaysInMonth(myDate.Year, myDate.Month));
        }

        public static DateTime GetPastDate(this DateTime startDate, int monthsNumber)
        {
            return startDate.AddMonths(-monthsNumber + 1);
        }
    }
}