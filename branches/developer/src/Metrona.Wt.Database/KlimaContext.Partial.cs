//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaContext.Partial.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Models
{
    public partial class KlimaContext : IEntitiesContext
    {
        public KlimaContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}