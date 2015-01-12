//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WetterStationPlz.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model
{
    public partial class WetterStationPlz
    {
        public long? Bis { get; set; }

        public long Id { get; set; }

        public long? Von { get; set; }

        // Foreign key 
        public long StationId { get; set; }

        // Navigation properties 
        public WetterStation Station { get; set; }
    }
}