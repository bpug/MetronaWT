//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaTemperaturModel.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.Models
{
    using System;

    public class KlimaTemperaturModel
    {
        public DateTime Date { get; set; }

        public double? Period1 { get; set; }

        public double? Period2 { get; set; }

        public int Heizgrenztemperatur { get; set; }
    }
}