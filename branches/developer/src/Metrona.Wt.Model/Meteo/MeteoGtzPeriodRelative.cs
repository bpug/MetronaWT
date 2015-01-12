//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoGtzPeriodRelative.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Meteo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MeteoGtzPeriodRelative
    {
        [Column(Order = 1, TypeName = "System.String")]
        public int Monat { get; set; }

        [Column("Aktuelles Jahr", Order = 3)]
        [Display(Name = "Aktuelles Jahr")]
        public double Period1 { get; set; }

        [Column(Order = 5)]
        [Display(Name = "Aktuelles Jahr gewichtet")]
        public double Period1Gewichtet
        {
            get
            {
                return this.Period1 * (this.Promille / 100);
            }
        }

        [Column("Vorjahr", Order = 4)]
        [Display(Name = "Vorjahr")]
        public double Period2 { get; set; }

        [Column(Order = 6)]
        [Display(Name = "Vorjahr gewichtet")]
        public double Period2Gewichtet
        {
            get
            {
                return this.Period2 * (this.Promille / 100);
            }
        }

        [Column(Order = 2)]
        public double Promille { get; set; }
    }
}