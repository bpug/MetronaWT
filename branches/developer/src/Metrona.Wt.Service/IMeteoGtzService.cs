//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IMeteoGtzService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Enums;
    using Metrona.Wt.Model.Meteo;

    public interface IMeteoGtzService
    {
        event EventHandler<EventArgs> OnError;

        Task<bool> CheckPlz(CalculateRequest calculateRequest);

        Task<IEnumerable<MeteoGtzData>> GetMeteoGtz(
            CalculateRequest calculateRequest,
            IntervalType intervalType = IntervalType.M36);

        Task<IEnumerable<MeteoGtzPeriod>> GetGtzByPeriods(
            CalculateRequest calculateRequest,
            IntervalType intervalType = IntervalType.M36);

        Task<IEnumerable<MeteoGtzPeriodRelative>> GetRelativeVerteilung(
            CalculateRequest calculateRequest,
            bool isHeizperiode,
            IntervalType intervalType = IntervalType.M36);

        Task<double?> GetJahrBedarfWithPromille(
            CalculateRequest calculateRequest,
            bool isHeizperiode,
            IntervalType intervalType = IntervalType.M36);

        Task<MeteoGtzYear> GetGtzYearsSum(
            CalculateRequest calculateRequest,
            bool isHeizperiode,
            IntervalType intervalType = IntervalType.M36);

        Task<MeteoGtzYear> GetYearsDataRelativeToCurrentYear(
            CalculateRequest calculateRequest,
            bool isHeizperiode,
            IntervalType intervalType = IntervalType.M36);
       
    }
}