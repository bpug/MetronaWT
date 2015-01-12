//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaTemperaturBundesland.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Klima
{
    using System;

    public partial class KlimaTemperaturBundesland
    {
        public DateTime Datum { get; set; }

        public int BundeslandId { get; set; }

        public double? Temperatur { get; set; }
    }
}