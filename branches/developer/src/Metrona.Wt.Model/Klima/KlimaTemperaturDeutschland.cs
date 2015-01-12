//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaTemperaturDeutschland.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Klima
{
    using System;

    public partial class KlimaTemperaturDeutschland
    {
        public DateTime Datum { get; set; }

        public double? Temperatur { get; set; }
    }
}