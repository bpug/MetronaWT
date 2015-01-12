//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoGtzPeriod.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Meteo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.InteropServices.ComTypes;

    public class MeteoGtzPeriod
    {
        [Column(Order = 1, TypeName = "System.String")]
        public int Monat { get; set; }

        [Column("Aktuelles Jahr", Order = 3)]
        [Display(Name = "Aktuelles Jahr")]
        public double Period1 { get; set; }

        [Column("Vorjahr", Order = 4)]
        [Display(Name = "Vorjahr")]
        public double Period2 { get; set; }

        [Column("Vorvorjahr", Order = 5)]
        [Display(Name = "Vorvorjahr")]
        public double Period3 { get; set; }

        [Column(Order = 6)]
        public double Lgtz { get; set; }

        [Column(Order = 2)]
        public double Promille { get; set; }
    }
}