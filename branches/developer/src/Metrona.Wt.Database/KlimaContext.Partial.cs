//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaContext.Partial.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

//http://weblogs.asp.net/manavi/associations-in-ef-4-1-code-first-part-6-many-valued-associations

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