//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaBundeslandRepository.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Metrona.Wt.Model;

    public class KlimaBundeslandRepository : EntityRepository<KlimaTemperaturBundesland>, IKlimaBundeslandRepository
    {
        public KlimaBundeslandRepository(IEntitiesContext entities)
            : base(entities)
        {
        }

        public IEnumerable<KlimaTemperaturBundesland> Get(int id, DateTime startDate, DateTime endDate)
        {
            return this.GetAll().Where(p => p.Datum >= startDate && p.Datum <= endDate);
        }
    }
}