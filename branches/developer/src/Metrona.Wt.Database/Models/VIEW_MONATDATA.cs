using System;
using System.Collections.Generic;

namespace Metrona.Wt.Database.Models
{
    public partial class VIEW_MONATDATA
    {
        public int WSCODE { get; set; }
        public string WDIMON { get; set; }
        public long MoTG { get; set; }
        public Nullable<decimal> MoTEMPMI { get; set; }
        public Nullable<decimal> MoHEIZTG { get; set; }
        public Nullable<decimal> MoTEMPMI_HT { get; set; }
        public Nullable<decimal> MoHGT15 { get; set; }
        public Nullable<decimal> MoGT2015 { get; set; }
        public Nullable<decimal> MoPM { get; set; }
        public Nullable<decimal> MoTNK { get; set; }
        public Nullable<decimal> MoTXK { get; set; }
        public Nullable<decimal> MoUPM { get; set; }
        public Nullable<decimal> MoVPM { get; set; }
        public Nullable<decimal> MoFMK { get; set; }
        public Nullable<decimal> MoFX { get; set; }
        public Nullable<decimal> MoNM { get; set; }
        public Nullable<decimal> MoSD { get; set; }
        public Nullable<decimal> MoRS { get; set; }
    }
}
