//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoMfaktor.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Meteo
{
    public partial class MeteoFaktor
    {
        public int Id { get; set; }

        public double Faktor { get; set; }

        public int? Monat { get; set; }
    }
}