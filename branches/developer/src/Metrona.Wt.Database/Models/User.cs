using System;
using System.Collections.Generic;

namespace Metrona.Wt.Database.Models
{
    using Metrona.Wt.Model;

    public partial class User : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool Ws { get; set; }
        public string WsAccessKey { get; set; }
        public int? Haus { get; set; }
    }
}
