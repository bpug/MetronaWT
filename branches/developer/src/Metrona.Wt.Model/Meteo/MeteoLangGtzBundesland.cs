//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoLangGtzBundesland.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Meteo
{
    using System.Collections.Generic;

    public partial class MeteoLangGtzBundesland
    {
        public long BundeslandId { get; set; }

        public double Gtz { get; set; }

        public int Monat { get; set; }

        //public ICollection<MeteoGtzBundesland> MeteoGtzBundeslands { get; set; }
    }
}