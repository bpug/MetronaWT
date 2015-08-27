//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WitterungstelegramCreateReport.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Reports.Pdf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Metrona.Wt.Core;
    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Enums;
    using Metrona.Wt.Service;

    public class PdfReport : IPdfReport
    {
        private CalculateRequest calculateRequest;

        private readonly IBundeslandService bundeslandService;

        private readonly IKlimaService klimaService;

        private readonly IMeteoGtzService meteoGtzService;

        public PdfReport (IMeteoGtzService meteoGtzService, IKlimaService klimaService, IBundeslandService bundeslandService)
        {
            this.meteoGtzService = meteoGtzService;
            this.bundeslandService = bundeslandService;
            this.klimaService = klimaService;
        }

        public async Task Download(CalculateRequest calcRequest, int drillMonth, string logoPath, string fileName)
        {
            using (var reporter = await GetReporter(calcRequest, drillMonth, logoPath))
            {
                reporter.Download(fileName);
            }
        }

        public async Task<IReporter> GetReporter(CalculateRequest calcRequest, int drillMonth, string logoPath)
        {
            this.calculateRequest = calcRequest;
            var report = new Reporter();
            var periods = Utils.GetZeitraume(calculateRequest.Stichtag);

            report.AddNewSection();
            report = await this.CreateHeader(report, logoPath);
            report.AddInfo();
            report.AddJahresbetrachtung(await meteoGtzService.GetGtzYearsSum(this.calculateRequest, true));

            var results = (await meteoGtzService.GetRelativeVerteilung(this.calculateRequest, false)).ToList();
            var dt = results.ToDataTable();
            report.AddMonatsbetrachtung(dt, periods);

            var tempData = drillMonth > 0 ?
                await this.klimaService.GetTemperaturMohtsDrill(calculateRequest, drillMonth) :
                await this.klimaService.GetTemperaturGroupedByPeriods(calculateRequest);

            
            report.AddTagesmitteltemperaturen(tempData.ToList(), periods);

            report.AddSpacing(30);
            report.AddRemarks();
            return report;
        }

        private async Task<Reporter> CreateHeader(Reporter report, string logoPath)
        {
            string requestLabel = string.Empty;
            switch (this.calculateRequest.RequestType)
            {
                case RequestType.Plz:
                    requestLabel = "PLZ: " + calculateRequest.Plz;
                    break;
                case RequestType.Bundesland:
                    var bl = await bundeslandService.GetById(calculateRequest.Value);
                    requestLabel = "Bundesland: " +  bl.Name;
                    break;
                case RequestType.Deutschland:
                    requestLabel = "Deutschland";
                    break;
            }

            report.AddHeader(this.calculateRequest.Stichtag, requestLabel, logoPath);
            return report;
        }
    }
}