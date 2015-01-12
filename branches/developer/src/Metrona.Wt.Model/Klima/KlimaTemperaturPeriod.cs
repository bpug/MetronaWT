//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaTemperaturPeriod.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Klima
{
    using System;

    public class KlimaTemperaturPeriod
    {
        public DateTime Datum { get; set; }

        public double? Period1 { get; set; }

        public double? Period2 { get; set; }

        public int Heizgrenztemperatur { get; set; }
    }
}