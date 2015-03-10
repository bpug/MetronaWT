//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoGtzService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Metrona.Wt.Core;
    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Database.Repositories;
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Enums;
    using Metrona.Wt.Model.Meteo;
    using Metrona.Wt.Service.Extensions;

    public class MeteoGtzService : IMeteoGtzService
    {
        private readonly IMeteoGtzRepository meteoGtzRepository;

        public event EventHandler<EventArgs> OnError;

        public MeteoGtzService(IMeteoGtzRepository meteoGtzRepository)
        {
            this.meteoGtzRepository = meteoGtzRepository;
        }

        public async Task<bool> CheckPlz(CalculateRequest calculateRequest)
        {
            switch (calculateRequest.RequestType)
            {
                case RequestType.Plz:
                    return (await this.meteoGtzRepository.FindByAsync(p => p.Plz == calculateRequest.Value, true)).Any();
            }
            return true;
        }

        public async Task<DateTime?> GetDataLastDate()
        {
            var result = await this.meteoGtzRepository.GetDataLastDate();
            return result;
        }

        public async Task<IEnumerable<Zeitraum>> GetAktuelleZeitraeme(int lastMonths)
        {
            var lastDate = await this.meteoGtzRepository.GetDataLastDate();

            var zeitraeme = new List<Zeitraum>();

            if (!lastDate.HasValue)
            {
                return zeitraeme;
            }

            for (int i = 0; i < lastMonths; i++)
            {
                var stichTag = lastDate.Value.AddMonths(-i);
                zeitraeme.Add(
                    new Zeitraum
                    {
                        End = stichTag,
                        Start = stichTag.AddYears(-1).AddMonths(1)
                    }
                );
            }
            return zeitraeme;
        }

        public async Task<IEnumerable<MeteoGtzPeriod>> GetGtzByPeriods(
            CalculateRequest calculateRequest,
            IntervalType intervalType = IntervalType.M36)
        {
            var meteoGtzDatas =  await this.GetMeteoGtz(calculateRequest, intervalType);

            var startDate = calculateRequest.Stichtag;
            var datumVon1 = startDate.GetPastDate(12);
            var datumBis1 = startDate;
            var datumVon2 = datumVon1.AddYears(-1);
            var datumBis2 = datumBis1.AddYears(-1);
            var datumVon3 = datumVon2.AddYears(-1);
            var datumBis3 = datumBis2.AddYears(-1);

            var result = meteoGtzDatas.GroupBy(
                p => new
                {
                    p.Monat,
                    p.Lgtz,
                    p.Promille
                }).Select(
                    (gr, index) => new MeteoGtzPeriod
                    {
                        Monat = gr.Key.Monat,
                        Lgtz = gr.Key.Lgtz,
                        Promille = gr.Key.Promille,
                        Period1 =
                            gr.Where(p => this.GetDate(p.Jahr, p.Monat).IsBetween(datumVon1, datumBis1))
                                .Select(p => p.Gtz)
                                .FirstOrDefault(),
                        Period2 =
                            gr.Where(p => this.GetDate(p.Jahr, p.Monat).IsBetween(datumVon2, datumBis2))
                                .Select(p => p.Gtz)
                                .FirstOrDefault(),
                        Period3 =
                            gr.Where(p => this.GetDate(p.Jahr, p.Monat).IsBetween(datumVon3, datumBis3))
                                .Select(p => p.Gtz)
                                .FirstOrDefault()
                    });
            return result;
        }

        public async Task<double?> GetJahrBedarfWithPromille(
            CalculateRequest calculateRequest,
            bool isHeizperiode,
            IntervalType intervalType = IntervalType.M36)
        {
            var source = await this.GetGtzByPeriods(calculateRequest, intervalType);

            source = isHeizperiode ? source.Where(p => p.Monat.IsHeizMonat()) : source;

            var sum = source.Aggregate<MeteoGtzPeriod, double?>(
                0.0,
                (current, item) => current + item.Period1 / item.Period2 * item.Promille);

            var result = (sum / 97 - 1) * 100;

            return result;
        }

        public async Task<IEnumerable<MeteoGtzData>> GetMeteoGtz(
            CalculateRequest calculateRequest,
            IntervalType intervalType = IntervalType.M36)
        {
            var startDate = calculateRequest.Stichtag.GetPastDate((int)intervalType);
            var endDate = calculateRequest.Stichtag.GetLastDayOfMonth();

            IEnumerable<MeteoGtzData> meteoGtzDatas = null;

            switch (calculateRequest.RequestType)
            {
                case RequestType.Plz:
                    meteoGtzDatas = await  this.GetGtzByPlz(calculateRequest.Value, startDate, endDate);
                    break;
                case RequestType.Bundesland:
                    meteoGtzDatas = await this.GetGtzByBundesland(calculateRequest.Value, startDate, endDate);
                    break;
                case RequestType.Deutschland:
                    meteoGtzDatas = await this.GetGtzDeutschland(startDate, endDate);
                    break;
            }
            return meteoGtzDatas;
        }

        private void ErrorEventRaiser(EventArgs e)
        {
            if (OnError != null)
                OnError(this, e);
        }

        public async Task<IEnumerable<MeteoGtzPeriodRelative>> GetRelativeVerteilung(
            CalculateRequest calculateRequest,
            bool isHeizperiode,
            IntervalType intervalType = IntervalType.M36)
        {
            var source = await this.GetGtzByPeriods(calculateRequest, intervalType);
            source = isHeizperiode ? source.Where(p => p.Monat.IsHeizMonat()) : source;

            var meteoGtzPeriods = source.ToList();

            double sumLgtz = meteoGtzPeriods.Sum(p => p.Lgtz);

            var result = meteoGtzPeriods.Select(
                p => new MeteoGtzPeriodRelative
                {
                    Monat = p.Monat,
                    Promille = p.Promille,
                    Period1 = (p.Lgtz - p.Period1) / sumLgtz * 100,
                    Period2 = (p.Lgtz - p.Period2) / sumLgtz * 100,
                });

            return result;
        }

        public async Task<MeteoGtzYear> GetGtzYearsSum(
            CalculateRequest calculateRequest,
            bool isHeizperiode,
            IntervalType intervalType = IntervalType.M36)
        {
            var source = await this.GetGtzByPeriods(calculateRequest, intervalType);
            source = isHeizperiode ? source.Where(p => p.Monat.IsHeizMonat()) : source;

            var meteoGtzPeriods = source.ToList();

            if (meteoGtzPeriods.Count == 0)
            {
                return null;
            }

            var result = new MeteoGtzYear
            {
                //IsHeizperiode = isHeizperiode,
                Period1 = meteoGtzPeriods.Sum(p => p.Period1),
                Period2 = meteoGtzPeriods.Sum(p => p.Period2),
                Period3 = meteoGtzPeriods.Sum(p => p.Period3),
                Lgtz = meteoGtzPeriods.Sum(p => p.Lgtz)
            };
            return result;
        }

        public async Task<MeteoGtzYear> GetYearsDataRelativeToCurrentYear(
            CalculateRequest calculateRequest,
            bool isHeizperiode,
            IntervalType intervalType = IntervalType.M36)
        {
            var meteoGtzSumYears = await this.GetGtzYearsSum(calculateRequest, isHeizperiode, intervalType);
            var result = meteoGtzSumYears.ToRelativeData();

            return result;
        }

        //public MeteoGtzYear GetYearsDataRelativeToCurrentYear(MeteoGtzYear meteoGtzSumYears)
        //{
        //    var relVorJahr = Utils.GetProzentual(meteoGtzSumYears.Period2, meteoGtzSumYears.Period1);
        //    var relVorVorJahr = Utils.GetProzentual(meteoGtzSumYears.Period3, meteoGtzSumYears.Period1);
        //    var relLgtz = Utils.GetProzentual(meteoGtzSumYears.Lgtz, meteoGtzSumYears.Period1);

        //    var result = new MeteoGtzYear
        //    {
        //        //IsHeizperiode = isHeizperiode,
        //        //Period1 = 100,
        //        Period2 = relVorJahr,
        //        Period3 = relVorVorJahr,
        //        Lgtz = relLgtz
        //    };
        //    return result;
        //}


       private DateTime GetDate(int year, int month)
        {
            return new DateTime(year, month, 1);
        }

        private async Task<IEnumerable<MeteoGtzData>> GetGtzByBundesland(long bundeslandid, DateTime startDate, DateTime endDate)
        {
           // var  result2 =  this.meteoGtzRepository.GetGtzByBundesland2(bundeslandid, startDate, endDate).ToList();
            var result = (await this.meteoGtzRepository.GetGtzByBundesland(bundeslandid, startDate, endDate))
                .Select(
                    p => new MeteoGtzData
                    {
                        Monat = p.Monat.GetValueOrDefault(),
                        Jahr = p.Jahr,
                        Gtz = p.Gtz,
                        Promille = p.Promille != null ? p.Promille.Anteil : 0,
                        Lgtz = p.Lgtz != null ? p.Lgtz.Gtz : 0
                    }).OrderBy(p => p.Jahr).ThenBy(p => p.Monat);
            return result;
        }

        private async Task<IEnumerable<MeteoGtzData>> GetGtzByPlz(int plz, DateTime startDate, DateTime endDate)
        {
            var result = ( await this.meteoGtzRepository.GetGtzByPlz(plz, startDate, endDate)).Select(
                p => new MeteoGtzData
                {
                    Monat = p.Monat,
                    Jahr = p.Jahr,
                    Gtz = p.Gtz,
                    Promille = p.Promille.Anteil,
                    Lgtz = p.Lgtz.Gtz
                }).OrderBy(p => p.Jahr).ThenBy(p => p.Monat);
            return result;
        }

        private async Task<IEnumerable<MeteoGtzData>> GetGtzDeutschland(DateTime startDate, DateTime endDate)
        {
            var result = (await this.meteoGtzRepository.GetGtzDeutschland(startDate, endDate)).Select(
                p => new MeteoGtzData
                {
                    Monat = p.Monat.GetValueOrDefault(),
                    Jahr = p.Jahr,
                    Gtz = p.Gtz,
                    Promille = p.Promille.Anteil,
                    Lgtz = p.Lgtz.Gtz
                }).OrderBy(p => p.Jahr).ThenBy(p => p.Monat);
            return result;
        }

        
    }
}