using System;
using System.Collections.Generic;

namespace Metrona.Wt.Database.Models
{
    public partial class VIEW_EVENTLOG
    {
        public Nullable<System.DateTime> Datum { get; set; }
        public string IP { get; set; }
        public string Username { get; set; }
        public long Anzahl { get; set; }
    }
}
