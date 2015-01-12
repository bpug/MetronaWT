//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoGtzBundesland.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Meteo
{
    public partial class MeteoGtzBundesland
    {
        public long? BundeslandId { get; set; }

        public double Gtz { get; set; }

        public int Jahr { get; set; }

        public int? Monat { get; set; }

        public Promille Promille { get; set; }

        public MeteoLangGtzBundesland Lgtz { get; set; }
    }
}