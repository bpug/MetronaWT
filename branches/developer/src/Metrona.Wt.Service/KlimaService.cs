//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="KlimaService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Database.Repositories;
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Enums;
    using Metrona.Wt.Model.Klima;

    public class KlimaService : IKlimaService
    {
        
        private readonly IKlimaRepository klimaRepository;

        private readonly IWetterStationRepository wetterStationRepository;

        public KlimaService(IKlimaRepository klimaRepository, IWetterStationRepository wetterStationRepository)
        {
            this.klimaRepository = klimaRepository;
            this.wetterStationRepository = wetterStationRepository;
        }

        public async Task<IEnumerable<KlimaTemperaturPeriod>> GetTemperaturGroupedByPeriods(CalculateRequest calculateRequest, IntervalType intervalType = IntervalType.M24)
        {
            var temperaturs = await GetTemperatur(calculateRequest);
            var result = this.GroupTemperaturByPeriods(temperaturs, calculateRequest.Stichtag);
            return result;
        }

        public async Task<IEnumerable<KlimaTemperatur>> GetTemperatur(CalculateRequest calculateRequest, IntervalType intervalType = IntervalType.M24)
        {
            var startDate = calculateRequest.Stichtag.GetPastDate((int)intervalType);
            var endDate = calculateRequest.Stichtag.GetLastDayOfMonth();
            
           IEnumerable<KlimaTemperatur> temperaturs = null;

           switch (calculateRequest.RequestType)
            {
                case RequestType.Plz:
                    var station = await wetterStationRepository.GetByPlz(calculateRequest.Value);
                    if (station == null) return new List<KlimaTemperatur>();
                    temperaturs = await this.GetTemperaturByPlz(station.WsCode, startDate, endDate);
                    break;
                case RequestType.Bundesland:
                    temperaturs = await this.GetTemperaturByBundesland(calculateRequest.Value, startDate, endDate);
                    break;
                case RequestType.Deutschland:
                    temperaturs = await this.GetTemperaturDeutschland(startDate, endDate);
                    break;
            }
            return temperaturs;
        }

        public IEnumerable<KlimaTemperaturPeriod> GroupTemperaturByPeriods(
            IEnumerable<KlimaTemperatur> klimaTemperaturs, 
            DateTime startDate, 
            IntervalType intervalType = IntervalType.M24)
        {
            var endDate = startDate.GetPastDate((int)intervalType);

            var datumVon1 = startDate.GetPastDate(12);
            var datumBis1 = startDate.GetLastDayOfMonth();
            var datumVon2 = endDate;
            var datumBis2 = startDate.GetPastDate(13).GetLastDayOfMonth();

            //For dayCounter important only month and day (used in chart X-Axis)
            var dayCounter = datumVon1; 
            var result = klimaTemperaturs
                .Where(p => !(p.Datum.Month == 2 && p.Datum.Day == 29 )) 
                .OrderBy( p=> p.Datum)
                .GroupBy(p => new { p.Datum.Day, p.Datum.Month })
                .Select((gr, index) => new KlimaTemperaturPeriod
                {
                    Datum = dayCounter.AddDays(index),
                    Period1 = gr.Where(p => p.Datum.IsBetween(datumVon1, datumBis1)).Select(p => p.Temperatur).FirstOrDefault(),
                    Period2= gr.Where(p => p.Datum.IsBetween(datumVon2, datumBis2)).Select(p => p.Temperatur).FirstOrDefault(),
                    Heizgrenztemperatur = 15
                });
            return  result;
        }

        public async Task<IEnumerable<KlimaTemperaturPeriod>> GetTemperaturMohtsDrill(CalculateRequest calculateRequest, int selectedMonth, 
            IntervalType intervalType = IntervalType.M24)
        {
            var temperaturGrouperByPeriods = await GetTemperaturGroupedByPeriods(calculateRequest);

            var stichTag = calculateRequest.Stichtag;
            int month1 = 0, month3 = 0;
            var month2 = selectedMonth;

            var startMonth = stichTag.Month;
            int endMonth = stichTag.GetPastDate((int)intervalType).Month;

            if (selectedMonth != startMonth)
            {
                month3 = selectedMonth == 12 ? 1 : selectedMonth + 1;
            }
            if (selectedMonth != endMonth)
            {
                month1 = selectedMonth == 1 ? 12 : selectedMonth - 1;
            }

            var result =
                 temperaturGrouperByPeriods.Where(
                    p => p.Datum.Month == month1 || p.Datum.Month == month2 || p.Datum.Month == month3);
         
            return result;
        }

        private async Task<IEnumerable<KlimaTemperatur>> GetTemperaturByPlz(int wscode, DateTime startDate, DateTime endDate)
        {
            var result = (await klimaRepository.FindByAsync(p => p.WsCode.Equals(wscode) && (p.Datum >= startDate && p.Datum <= endDate), true))
                 .Select(p => new KlimaTemperatur
                 {
                     Datum = p.Datum,
                     Temperatur = p.Temperatur
                 })
                .OrderBy(p => p.Datum);
            return result;
        }

        private async Task<IEnumerable<KlimaTemperatur>> GetTemperaturByBundesland(int bundeslandId, DateTime startDate, DateTime endDate)
        {
            var result = (await klimaRepository.GetByAsync<KlimaTemperaturBundesland>(q => q.Where(p=> p.BundeslandId.Equals(bundeslandId) && (p.Datum >= startDate && p.Datum <= endDate)), true))
                .Select(p => new KlimaTemperatur
                {
                    Datum = p.Datum,
                    Temperatur = p.Temperatur
                })
               .OrderBy(p => p.Datum);
            return result;
        }

        private async Task<IEnumerable<KlimaTemperatur>> GetTemperaturDeutschland(DateTime startDate, DateTime endDate)
        {
            var result = (await klimaRepository.GetByAsync<KlimaTemperaturDeutschland>(q => q.Where(p => p.Datum >= startDate && p.Datum <= endDate), true))
                .Select(p => new KlimaTemperatur
                {
                    Datum = p.Datum,
                    Temperatur = p.Temperatur
                })
               .OrderBy(p => p.Datum);
            return result;
        }
        
    }
}