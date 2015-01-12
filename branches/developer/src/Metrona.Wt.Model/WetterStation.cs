//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Wetterstation.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class WetterStation : Entity
    {
        public WetterStation()
        {
            this.Plzs =  new HashSet<WetterStationPlz>();
            //this.Klimas = new HashSet<Klima>();
        }

        public int? BundeslandId { get; set; }

        public int WsCode { get; set; }

        public string Dienst { get; set; }

        public string Icaok { get; set; }

        public string Ort { get; set; }

        [DisplayName("WStati")]
        public long StationId { get; set; }

        public virtual ICollection<WetterStationPlz> Plzs { get; private set; }

        //public virtual ICollection<Klima> Klimas { get; private set; }

    }
}