//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IBundeslandService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Metrona.Wt.Model;

    public interface IBundeslandService
    {
        Task<IEnumerable<Bundesland>> GetAll();

        Task<Bundesland> GetById(int id);
    }
}