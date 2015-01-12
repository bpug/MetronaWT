//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Langzeitmittel.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model
{
    public partial class Langzeitmittel : Entity
    {
        public double Gtz { get; set; }

        public double Hgt { get; set; }

        public double RelGtz { get; set; }

        public double RelHgt { get; set; }

        public int StationId { get; set; }
    }
}