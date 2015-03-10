//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WitterungsTelegramm.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Data;

    using Infragistics.UltraChart.Shared.Events;
    using Infragistics.WebUI.UltraWebChart;

    using Metrona.Wt.Core;
    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Enums;
    using Metrona.Wt.Reports.Charts;
    using Metrona.Wt.Reports.Excel;
    using Metrona.Wt.Reports.Pdf;
    using Metrona.Wt.Service;
    using Metrona.Wt.Service.Extensions;
    using Metrona.Wt.Web.Extensions;
    using Metrona.Wt.Web.Models;
    using Metrona.Wt.Web.UI;

    public partial class WitterungsTelegramm : PageBase
    {
        private CalculateRequest calculateRequest
        {
            get
            {
                return SessionData.CalculateRequest;
            }
        }
        
        [Microsoft.Practices.Unity.Dependency]
        public IBundeslandService BundeslandService { get; set; }

        [Microsoft.Practices.Unity.Dependency]
        public IKlimaService KlimaService { get; set; }

        [Microsoft.Practices.Unity.Dependency]
        public IMeteoGtzService MeteoGtzService { get; set; }

        [Microsoft.Practices.Unity.Dependency]
        public IExcelExporter ExcelExporter { get; set; }

        [Microsoft.Practices.Unity.Dependency]
        public IPdfReport PdfReport { get; set; }
        

        private const string Chart1Title =
            "1. Jahresbetrachtung der Temperatur des aktuellen Jahres im Vergleich zu den Vorjahren und Langzeitmittel<sup>2</sup>";

        private const string Chart2Title =
            "2. Monatsbetrachtung der Temperatur des aktuellen Jahres im Vergleich zum Vorjahr und Langzeitmittel<sup>2</sup>";

        private const string Chart3Title =
            "3. Tagesmitteltemperaturen<sup>3</sup> des gewählten Abrechnungszeitraumes und des Vorjahres";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.WebForms;
            MeteoGtzService.OnError += (o, args) => { };

            if (Page.IsPostBack)
            {
                return;
            }
            this.pnlReport.Visible = Context.User.Identity.IsAuthenticated;
        }

        public IQueryable<Bundesland>GetBundeslands()
        {
           //RegisterAsyncTask(new PageAsyncTask(() => BundeslandService.GetAll()));
           var bl = AsyncHelper.RunSync(() => BundeslandService.GetAll()).AsQueryable();
           return bl;
        }

        public IEnumerable<Zeitraum> GetZeitraeume()
        {
            var zr = AsyncHelper.RunSync(() => MeteoGtzService.GetAktuelleZeitraeme(24));
            return zr;
        }

        public SelectList GetRequestTypes()
        {
            var result = new RequestType().ToSelectList();
            return result;
        }

        public CalculateRequestViewModel GetCalculateRequest()
        {
            var result = new CalculateRequestViewModel
            {
               //RequestType = RequestType.None, //User.Identity.IsAuthenticated ? RequestType.Plz : RequestType.Bundesland
            };
            return result;
        }

        public async void Calculate(CalculateRequestViewModel request)
        {
            SessionData.CalculateRequest = request.ToModel();

            if (ModelState.IsValid)
            {
                if (request.RequestType == RequestType.Bundesland && request.BundeslandId == null)
                {
                    ShowPanels(false);
                    ModelState.AddModelError("", "Bitte treffen Sie beim Bundesland Ihre Auswahl.");
                    return;
                }

                if (request.RequestType == RequestType.Plz && request.Plz == null)
                {
                    ShowPanels(false);
                    ModelState.AddModelError("", "Geben Sie bitte eine Postleitzahl ein.");
                    return;
                }

                if (await this.MeteoGtzService.CheckPlz(calculateRequest))
                {
                    ShowPanels(true);
                    ShowCurrentAuswahl();
                    await GetJahresbetrachtungChart();
                    await GetMonatsRelativeVerteilungJahr();
                    await this.GetChartTemperatur();
                    btnDrillBack.Visible = false;
                }
                else
                {
                    ShowPanels(false);
                    ModelState.AddModelError("", "Überprüfen Sie bitte PLZ.");
                }
               
            }
            else
            {
                ShowPanels(false);
            }
        }

        private async void ShowCurrentAuswahl()
        {
            var region = string.Empty;
            switch (calculateRequest.RequestType)
            {
                case RequestType.Plz:
                    region = "PLZ: " + calculateRequest.Value;
                    break;
                case RequestType.Bundesland:
                    var bl = await BundeslandService.GetById(calculateRequest.Value);
                    region = "Bundesland: " + bl.Name;
                    break;
                case RequestType.Deutschland:
                    region = "Deutschland";
                    break;
            }

            lblAbrechnungszeitraum.Text = Utils.GetZeitraume(calculateRequest.Stichtag).OrderByDescending(p => p.Start).FirstOrDefault().Interval;
            lblregion.Text = region;
       }
       private void ShowPanels(bool visible)
       {
           this.pnlWDS.Visible = visible;
           pnlJahresbetrachtung.Visible = visible;
           pnlRelativeVerteilungJahr.Visible = visible;
           pnlChartTemperatur.Visible = visible;
           pnlInfoHinweis.Visible = visible;
       }

        private async Task GetJahresbetrachtungChart()
        {
            pnlJahresbetrachtungChart.Visible = false;
            lblahresbetrachtungNoData.Visible = false;
            
            //chartService = new ChartService(calculateRequest.Stichtag, calculateRequest.Value, calculateRequest.RequestType);
            //var dt2 = chartService.GetJahresSummenGtz(IntervalType.M36);
            var gtzYearsSum = await MeteoGtzService.GetGtzYearsSum(this.calculateRequest, true);
            if (gtzYearsSum == null)
            {
                lblahresbetrachtungNoData.Visible = true;
                return;
            }

            lblChartVergleichJahrTitle.Text = Chart1Title;

            UltraChart myChart = JahresbetrachtungChart.GetChart(gtzYearsSum, 710, 300);
            this.chartVergleichJahr.Controls.Add(myChart);
                
            //var dt2 = chartService.GetJahresbetrachtungProzentual();
            var relativeData = gtzYearsSum.ToRelativeData(); // await MeteoGtzService.GetYearsDataRelativeToCurrentYear(this.calculateRequest, true);

            //DataRow firstRow = dt2.Rows[0];
            double vorjahrBedarf = relativeData.Period2; // firstRow["Vorjahr"].ConvertTo<double>();
            double lgtzBedarf = relativeData.Lgtz; // firstRow["LGTZ"].ConvertTo<double>();

            lblVorjahrBedarf.Text = string.Format(
                        @"In der gewählten Region war es im gleichen Zeitraum des Vorjahres {0}% {1} als im
                        betrachtenten Zeiraum des aktuellen Jahres.<br/>
                        Entsprechend ist im aktuellen Jahr im Vergleich zum Vorjahreszeitraum mit einem Heiz{2}bedarf zu rechnen",
                Math.Abs(Math.Round(vorjahrBedarf, 2)), (vorjahrBedarf > 0 ? "wärmer" : "kälter"), (vorjahrBedarf > 0 ? "mehr" : "minder"));
            lblLGTZBedarf.Text = string.Format("In der gewählten Region ist das Langzeitmittel {0}% {1} als das aktuellen Jahr.", Math.Abs(Math.Round(lgtzBedarf, 2)), (lgtzBedarf > 0 ? "wärmer" : "kälter"));

            pnlJahresbetrachtungChart.Visible = true;
        }

        private async Task GetMonatsRelativeVerteilungJahr()
        {
            pnlMonatsbetrachtungChart.Visible = false;
            lblRelativeVerteilungJahrNoData.Visible = false;
           
            //var dt2 = chartService.GetMonatsRelativeVerteilungJahr(false);
            var results = ( await MeteoGtzService.GetRelativeVerteilung(this.calculateRequest, false)).ToList();
            if (results.Count == 0)
            {
                lblRelativeVerteilungJahrNoData.Visible = true;
                return;
            }

            var dt = results.ToDataTable();

            lblChartRelativeVerteilungJahrTitle.Text = Chart2Title;

            //Dummy Column für Legende Langzeitmittel
            var column = new DataColumn
            {
                DataType = typeof(double)
            };
            dt.Columns.Add(column);

            var columnsLabels = Utils.GetZeitraume(calculateRequest.Stichtag).OrderBy(p => p.Start).GetFormatted(true);
            columnsLabels.Add("Langzeitmittel (Nulllinie)");

            var myChart = MonatsRelativeVerteilungJahrChart.GetChart(dt, 800, 400, columnsLabels.ToArray());
            this.chartMonatsbetrachtung.Controls.Add(myChart);

            var row = results.LastOrDefault();
            if (row != null)
            {
                var vorjahr = row.Period1;
                var aktuellJqahr = row.Period2;

                var monat = Utils.GetMonthName(row.Monat.ConvertTo<int>(), "MMMM");
                this.lblVorjahrBedarf2.Text = string.Format("im Monat {2} des Vorjahres ca. {0}% {1}", Math.Abs(Math.Round(vorjahr, 2)), (vorjahr > 0 ? "wärmer" : "kälter"), monat);
                this.lblAktuelJahrBedarf2.Text = string.Format("im Monat {2} des aktuellen Jahres ca. {0}% {1}", Math.Abs(Math.Round(aktuellJqahr, 2)), (aktuellJqahr > 0 ? "wärmer" : "kälter"), monat);
            }
            pnlMonatsbetrachtungChart.Visible = true;
        }

        private async Task GetChartTemperatur()
        {
            pnlChartTemperatur.Visible = false;
            
            //var dt = chartService.TemperaturData;
            //dt.Columns["Heizgrenztemperatur"].ColumnName = "Heizgrenztemperatur³";
            ////this.ChartTemperatur = TemperaturChart.GetChart(this.ChartTemperatur, dt, 800, 520);

            lblChartTemperaturTitle.Text = Chart3Title;
            var columnsLabels = Utils.GetZeitraume(calculateRequest.Stichtag).OrderBy(p => p.Start).GetFormatted(true);

            columnsLabels.Add("Heizgrenztemperatur⁴");

            var ds = await KlimaService.GetTemperaturGroupedByPeriods(calculateRequest);
            this.ChartTemperatur = TemperaturChart.GetChart(this.ChartTemperatur, ds.ToList(), 800, 520, columnsLabels.ToArray());

            pnlChartTemperatur.Visible = true;
        }
        
        protected async void ChartOnChartDataClicked(object sender, ChartDataEventArgs e)
        {
            await GetChartTemperatur();
            if (btnDrillBack.Visible)
            {
                btnDrillBack.Visible = false;
                SessionData.TemperaturDrillMonat = 0;
                return;
            }

            var month = DateTime.Parse(e.ColumnLabel).Month;
            SessionData.TemperaturDrillMonat = month;
            this.ChartTemperatur.Drill.DrillElements[0].DrillDown.Drill(
                e.DataRow,
                e.DataColumn,
                Infragistics.UltraChart.Shared.Styles.ChartType.LineChart,
                (await KlimaService.GetTemperaturMohtsDrill(calculateRequest, month)).ToList());
            // Me.UltraChart1.Drill.DrillBack()
            btnDrillBack.Visible = true;
            //btnDrillBack.Focus();
        }
        
        protected async void OnBtnDrillBackClick(object sender, EventArgs e)
        {
            await GetChartTemperatur();
            btnDrillBack.Visible = false;
        }

        protected async void OnBtnPrintPdf(object sender, EventArgs e)
        {
            var logo = Server.MapPath("img/bm_logo.jpg");
            await PdfReport.Download(calculateRequest, SessionData.TemperaturDrillMonat, logo, "Witterungstelegramm.pdf");
        }

        protected async void OnBtnExport(object sender, EventArgs e)
        {
           await ExcelExporter.Export(calculateRequest);
        }


        //protected void BundeslandOnDataBound(object sender, EventArgs e)
        //{
        //    var ddlBundesland = sender as DropDownList;
        //    if (ddlBundesland != null)
        //    {
        //        ddlBundesland.Items.Insert(0, new ListItem("Bitte wählen", "0"));
        //        ddlBundesland.SelectedIndex = 0;
        //    }
        //}
       
    }
}