using System;
using System.Collections.Generic;

namespace Metrona.Wt.Database.Models
{
    public partial class KlimaDwd
    {
        public long KLIMA_ID { get; set; }
        public Nullable<int> WSCODE { get; set; }
        public string WSTATI { get; set; }
        public string DATUM { get; set; }
        public Nullable<int> QN { get; set; }
        public Nullable<decimal> TG { get; set; }
        public Nullable<decimal> TNK { get; set; }
        public Nullable<decimal> TEMPMI { get; set; }
        public Nullable<decimal> TXK { get; set; }
        public Nullable<decimal> UPM { get; set; }
        public Nullable<decimal> FMK { get; set; }
        public Nullable<decimal> FX { get; set; }
        public Nullable<decimal> SD { get; set; }
        public Nullable<decimal> NM { get; set; }
        public Nullable<decimal> RS { get; set; }
        public Nullable<decimal> PM { get; set; }
        public string WDIENS { get; set; }
        public Nullable<System.DateTime> WDITAG { get; set; }
        public Nullable<bool> HEIZTG { get; set; }
        public Nullable<decimal> HGT15 { get; set; }
        public Nullable<decimal> GT2015 { get; set; }
        public Nullable<System.DateTime> AENDDT { get; set; }
        public string OPID { get; set; }
        public Nullable<decimal> GTPROM { get; set; }
        public Nullable<decimal> VPM { get; set; }
    }
}
