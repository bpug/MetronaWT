//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BundeslandPlz.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model
{
    public partial class BundeslandPlz : Entity
    {
        public string BundeslandName { get; set; }

        public long BundeslandId { get; set; }

        public int Plz { get; set; }

        public int StationId { get; set; }

        public Bundesland Bundesland { get; set; }
    }
}