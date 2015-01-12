//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IKlimaService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Enums;
    using Metrona.Wt.Model.Klima;

    public interface IKlimaService
    {
        Task<IEnumerable<KlimaTemperatur>> GetTemperatur(
            CalculateRequest calculateRequest,
            IntervalType intervalType = IntervalType.M24);

        Task<IEnumerable<KlimaTemperaturPeriod>> GetTemperaturGroupedByPeriods(
            CalculateRequest calculateRequest,
            IntervalType intervalType = IntervalType.M24);

        Task<IEnumerable<KlimaTemperaturPeriod>> GetTemperaturMohtsDrill(
            CalculateRequest calculateRequest,
            int selectedMonth,
            IntervalType intervalType = IntervalType.M24);
    }
}