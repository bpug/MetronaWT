using System;
using System.Collections.Generic;

namespace Metrona.Wt.Database.Models
{
    public partial class KlimafaktorDwd
    {
        public int Klimafaktor_ID { get; set; }
        public Nullable<int> PLZ { get; set; }
        public Nullable<System.DateTime> DATUM_VON { get; set; }
        public Nullable<System.DateTime> DATUM_BIS { get; set; }
        public Nullable<double> KF { get; set; }
    }
}
