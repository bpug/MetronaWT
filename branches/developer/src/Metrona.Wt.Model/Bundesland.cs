//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Bundesland.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model
{
    using System.Collections.Generic;

    public class Bundesland : Entity
    {
        public string Name { get; set; }

        public  ICollection<BundeslandPlz> Plzs { get; set; }
    }
}