//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoCalculateExtensions.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Service.Extensions
{
    using Metrona.Wt.Core;
    using Metrona.Wt.Model.Meteo;

    public static class MeteoCalculateExtensions
    {

        public static MeteoGtzYear ToRelativeData(this MeteoGtzYear meteoGtzSumYears)
        {
            var relVorJahr = Utils.GetProzentual(meteoGtzSumYears.Period1, meteoGtzSumYears.Period2);
            var relVorVorJahr = Utils.GetProzentual(meteoGtzSumYears.Period1, meteoGtzSumYears.Period3);
            var relLgtz = Utils.GetProzentual(meteoGtzSumYears.Period1, meteoGtzSumYears.Lgtz);

            var result = new MeteoGtzYear
            {
                //IsHeizperiode = isHeizperiode,
                //Period1 = 100,
                Period2 = relVorJahr,
                Period3 = relVorVorJahr,
                Lgtz = relLgtz
            };
            return result;
        }


        public static MeteoGtzYear ToRelativeDataForChart(this MeteoGtzYear meteoGtzSumYears)
        {
            var relativ = meteoGtzSumYears.ToRelativeData();

            relativ.Period1 = 100;
            relativ.Period2 = 100 + relativ.Period2;
            relativ.Period3 = 100 + relativ.Period3;
            relativ.Lgtz = 100 + relativ.Lgtz;

            return relativ;
        }

        //public static IEnumerable<MeteoGtzPeriodRelative> GetRelativeVerteilung(this IEnumerable<MeteoGtzPeriod> source, bool isHeizperiode)
        //{
        //    source = isHeizperiode ? source.Where(p => p.Monat.IsHeizMonat()) : source;

        //    var meteoGtzPeriods = source.ToList();

        //    var sumLgtz = meteoGtzPeriods.Sum(p => p.Lgtz);

        //    var result = meteoGtzPeriods.Select(p =>
        //        new MeteoGtzPeriodRelative
        //        {
        //            Monat = p.Monat.ToString(),
        //            Promille = p.Promille,
        //            Period1 = (p.Lgtz - p.Period1) / sumLgtz * 100,
        //            Period2 = (p.Lgtz -p.Period2) / sumLgtz * 100,
        //        });

        //    return result;
        //}

        //public static double? GetJahrBedarfWithPromille(this IEnumerable<MeteoGtzPeriod> source, bool isHeizperiode)
        //{
        //    source = isHeizperiode ? source.Where(p => p.Monat.IsHeizMonat()) : source;

        //    var sum = source.Aggregate<MeteoGtzPeriod, double?>(0.0, (current, item) => current + item.Period1 / item.Period2 * item.Promille);

        //    var result = (sum / 97 - 1) * 100;

        //    return result;
        //}
        
        //public static MeteoGtzYear GetYearsData(this IEnumerable<MeteoGtzPeriod> source, bool isHeizperiode)
        //{
        //    source = isHeizperiode ? source.Where(p => p.Monat.IsHeizMonat()) : source;

        //    var meteoGtzPeriods = source.ToList();

        //    var result = new MeteoGtzYear
        //    {
        //        //IsHeizperiode = isHeizperiode,
        //        Period1 = meteoGtzPeriods.Sum(p => p.Period1),
        //        Period2 = meteoGtzPeriods.Sum(p => p.Period2),
        //        Period3 = meteoGtzPeriods.Sum(p => p.Period3),
        //        Lgtz = meteoGtzPeriods.Sum(p => p.Lgtz)
        //    };
        //    return result;
        //}

        //public static MeteoGtzYear GetYearsDataRelativeToCurrentYear(this IEnumerable<MeteoGtzPeriod> source, bool isHeizperiode)
        //{
        //    source = isHeizperiode ? source.Where(p => p.Monat.IsHeizMonat()) : source;

        //    var meteoGtzPeriods = source.ToList();
        //    var sumAktuellYear = meteoGtzPeriods.Sum(p => p.Period1);

        //    var sumVorYear = meteoGtzPeriods.Sum(p => p.Period2);
        //    var sumVorVorYear = meteoGtzPeriods.Sum(p => p.Period3);
        //    var sumLgtz = meteoGtzPeriods.Sum(p => p.Lgtz);

        //    var relVorJahr = Utils.GetProzentual(sumVorYear, sumAktuellYear);
        //    var relVorVorJahr = Utils.GetProzentual(sumVorVorYear, sumAktuellYear);
        //    var relLgtz = Utils.GetProzentual(sumLgtz, sumAktuellYear );

        //    var result = new MeteoGtzYear
        //    {
        //        //IsHeizperiode = isHeizperiode,
        //        Period1 = 100,
        //        Period2 = relVorJahr,
        //        Period3 = relVorVorJahr,
        //        Lgtz = relLgtz
        //    };
        //    return result;
        //}
    }
}