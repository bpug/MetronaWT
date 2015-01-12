//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoGtzPeriod.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Meteo
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MeteoGtzYear
    {
        [Display(Name = "Aktuelles Jahr")]
        public double Period1 { get; set; }

        [Display(Name = "Vorjahr")]
        public double Period2 { get; set; }

        [Display(Name = "Vorvorjahr")]
        public double Period3 { get; set; }

        [Display(Name = "LGTZ")]
        [Column("LGTZ")]
        public double Lgtz { get; set; }

        //public bool IsHeizperiode { get; set; }
        
    }
}