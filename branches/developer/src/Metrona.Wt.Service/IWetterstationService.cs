//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IWetterstationService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Service
{
    using System.Threading.Tasks;

    using Metrona.Wt.Model;

    public interface IWetterstationService
    {
        Task<WetterStation> GetByPlz(int plz);
    }
}