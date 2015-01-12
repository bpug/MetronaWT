//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IMeteoGtzData.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Meteo
{
    public class MeteoGtzData
    {
        public double Gtz { get; set; }

        public int Jahr { get; set; }

        public double Lgtz { get; set; }

        public int Monat { get; set; }

        public double Promille { get; set; }
    }
}