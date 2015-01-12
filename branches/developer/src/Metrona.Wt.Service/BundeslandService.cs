//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BundeslandService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Metrona.Wt.Database.Repositories;
    using Metrona.Wt.Model;

    public class BundeslandService : IBundeslandService
    {
        private readonly IBundeslandRepository bundeslandRepository;

        public BundeslandService(IBundeslandRepository bundeslandRepository)
        {
            this.bundeslandRepository = bundeslandRepository;
        }

        public async Task<IEnumerable<Bundesland>> GetAll()
        {
            return await this.bundeslandRepository.GetAllAsync(true);
        }

        public async Task<Bundesland> GetById(int id)
        {
            var result = await this.bundeslandRepository.GetByIdAsynch(id);
            return result;
        }
    }
}