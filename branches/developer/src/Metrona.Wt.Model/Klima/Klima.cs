//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Klima.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Klima
{
    using System;

    public partial class Klima : Entity
    {
        public DateTime? AendDatum { get; set; }

        public DateTime Datum { get; set; }

        public decimal? Fmk { get; set; }

        public decimal? Fx { get; set; }

        public decimal? Gt2015 { get; set; }

        public decimal? Gtprom { get; set; }

        public decimal? Hgt15 { get; set; }

        public bool? IsHeizTag { get; set; }

        public decimal? Nm { get; set; }

        public string OpId { get; set; }

        public decimal? Pm { get; set; }

        public decimal? Rs { get; set; }

        public decimal? Sd { get; set; }

        public double? Temperatur { get; set; }

        public decimal? Tnk { get; set; }

        public decimal? Txk { get; set; }

        public decimal? Upm { get; set; }

        public decimal? Vpm { get; set; }

        public int WsCode { get; set; }

        public string WsDienst { get; set; }

        //public long StationId { get; set; }

        //public virtual WetterStation Station { get; set; }


    }
}