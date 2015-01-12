//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IKlimaBundeslandRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System;
    using System.Collections.Generic;

    using Metrona.Wt.Model;

    public interface IKlimaBundeslandRepository : IEntityRepository<KlimaTemperaturBundesland>
    {
        IEnumerable<KlimaTemperaturBundesland> Get(int id, DateTime startDate, DateTime endDate);
    }
}