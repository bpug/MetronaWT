//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Promille.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model
{
    using System.Collections.Generic;

    using Metrona.Wt.Model.Meteo;

    public partial class Promille : Entity
    {
        public double Anteil { get; set; }

        public int Monat { get; set; }

        //public ICollection<MeteoGtz> MeteoGtzes { get; set; }

        //public ICollection<MeteoGtzDeutschland> MeteoGtzDeutschlands { get; set; }

        //public ICollection<MeteoGtzBundesland> MeteoGtzBundeslands { get; set; }
        
    }
}