using System;
using System.Collections.Generic;

namespace Metrona.Wt.Database.Models
{
    using Metrona.Wt.Model;

    public partial class Eventlog : Entity
    {
        public string AccessKey { get; set; }
        public DateTime? CreateAt { get; set; }
        public string ClientIp { get; set; }
        public string Method { get; set; }
        public string Parameters { get; set; }
        public string SoapMessage { get; set; }
        public bool Error { get; set; }
        public string ErrMessage { get; set; }
        public int? UserId { get; set; }
        public sbyte? Region { get; set; }
    }
}
