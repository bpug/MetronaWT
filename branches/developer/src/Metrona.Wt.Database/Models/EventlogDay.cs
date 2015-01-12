using System;
using System.Collections.Generic;

namespace Metrona.Wt.Database.Models
{
    using Metrona.Wt.Model;

    public partial class EventlogDay : Entity
    {
        public DateTime Date { get; set; }
        public string Ip { get; set; }
        public string Username { get; set; }
        public int Count { get; set; }
        public string Method { get; set; }
    }
}
