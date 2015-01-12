using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrona.Wt.Reports.Excel
{
    using System.Drawing;

    using Infragistics.Documents.Excel;
    using Infragistics.Web.UI.GridControls;

    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Enums;
    using Metrona.Wt.Service;

    public class ExcelExporter : IExcelExporter
    {
        private const string Title1 =
            "Jahresbetrachtung der Temperatur des Aktuellen Jahres im Vergleich zu den Vorjahren und Langzeitmittel";

        private const string Title2 =
            "Monatssbetrachtung der Temperatur des Aktuellen Jahres im Vergleich zum Vorjahr und Langzeitmittel";

        private CalculateRequest calculateRequest;

        private readonly IMeteoGtzService meteoGtzService;

        private readonly IBundeslandService bundeslandService;

        private readonly WebExcelEsporter webExcelExporter;

        public ExcelExporter(IMeteoGtzService meteoGtzService, IBundeslandService bundeslandService)
        {
            this.meteoGtzService = meteoGtzService;
            this.bundeslandService = bundeslandService;
            webExcelExporter = new WebExcelEsporter
            {
                EnableStylesExport = false,
                DownloadName = "Berechnungsgrundlage",
                ExportMode = ExportMode.Download
            };
            webExcelExporter.Exporting += WebExcelExporterOnExporting;
        }

        public async Task Export(CalculateRequest calculateRequest)
        {
            this.calculateRequest = calculateRequest;
            //InitOldData();
            var grid1 = new WebDataGrid
            {
                //DataSource = this.chartService.GetJahresbetrachtungProzentual(IntervalType.M36)
                DataSource = (await meteoGtzService.GetYearsDataRelativeToCurrentYear(calculateRequest, true)).ToDataTable()
            };

            var grid2 = new WebDataGrid
            {
                //DataSource = ChartService.CalculateSum(this.chartService.GetMonatsRelativeVerteilungJahr(false))
                DataSource = (await meteoGtzService.GetRelativeVerteilung(calculateRequest, false)).ToDataTable().CalculateSum()
            };

            var grid3 = new WebDataGrid
            {
                //DataSource = ChartService.CalculateSum(this.chartService.MonatsSummenGtzJahr)
                DataSource = (await meteoGtzService.GetGtzByPeriods(calculateRequest)).ToDataTable().CalculateSum()
            };

            grid1.DataBind();
            grid2.DataBind();
            grid3.DataBind();

            var wb = new Workbook();
            var font = wb.Styles.NormalStyle.StyleFormat.Font;
            font.Name = "Microsoft Sans Serif";
            font.Height = 9 * 20;

            var ws = wb.Worksheets.Add("Daten");

            ws.Rows[3].Cells[0].Value = Title1;
            ws.Rows[3].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            ws.Rows[12].Cells[0].Value = Title2;
            ws.Rows[12].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            ws.Rows[29].Cells[0].Value = "Monatssummen der GTZ im Vergleich";
            ws.Rows[29].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            ws.Rows[8].Cells[0].Value = "Heizbedarf für das Abrechnungsjahr";
            ws.Rows[8].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            //ws.Rows[9].Cells[0].Value = this.chartService.GetJahrBedarfWithPromille();
            ws.Rows[9].Cells[0].Value = await meteoGtzService.GetJahrBedarfWithPromille(calculateRequest, true);

            var wdge1 = new WebDataGridExport
            {
                Grid = grid1,
                RowOffset = 4
            };
            var wdge2 = new WebDataGridExport
            {
                Grid = grid2,
                RowOffset = 13
            };
            var wdge3 = new WebDataGridExport
            {
                Grid = grid3,
                RowOffset = 30
            };

            RegionFormat(ws.GetRegion("A5:C5"), true);
            RegionFormat(ws.GetRegion("A14:F14"), true);
            RegionFormat(ws.GetRegion("A27:F27"));
            RegionFormat(ws.GetRegion("A28:F28"));
            RegionFormatBold(ws.GetRegion("A27:A28"));

            RegionFormat(ws.GetRegion("A31:F31"), true);
            RegionFormat(ws.GetRegion("A44:F44"));
            RegionFormat(ws.GetRegion("A45:F45"));
            RegionFormatBold(ws.GetRegion("A44:A45"));
            RegionFormatBold(ws.GetRegion("A27:A28"));

            webExcelExporter.Export(ws, wdge1, wdge2, wdge3);
        }

        private void RegionFormat(IEnumerable<WorksheetCell> region, bool isHeader = false)
        {
            foreach (var cell in region)
            {
                cell.CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                if (isHeader)
                    cell.CellFormat.Alignment = HorizontalCellAlignment.Center;
            }
        }
        private void RegionFormatBold(IEnumerable<WorksheetCell> region)
        {
            foreach (var cell in region)
            {
                cell.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            }
        }

        private async void WebExcelExporterOnExporting(object sender, ExcelExportingEventArgs e)
        {
            string title = string.Empty;

            switch (calculateRequest.RequestType)
            {
                case RequestType.Plz:
                    title = "PLZ: " + calculateRequest.Value;
                    break;
                case RequestType.Bundesland:
                    var bl = await bundeslandService.GetById(calculateRequest.Value);
                    title = "Bundesland: " + bl.Name;
                    break;
                case RequestType.Deutschland:
                    title = "Deutscland";
                    break;
            }

            // Add a title to the report
            e.Worksheet.Rows[0].Cells[0].Value = title;
            e.Worksheet.Rows[0].Cells[0].CellFormat.Font.ColorInfo = Color.Black;
            e.Worksheet.Rows[0].Cells[0].CellFormat.Font.Height = 10 * 20;

            e.Worksheet.Rows[1].Cells[0].Value = "Stichtag: " + calculateRequest.Stichtag.ToString("MM.yyyy");
            e.Worksheet.Rows[1].Cells[0].CellFormat.Font.ColorInfo = Color.Black;
            e.Worksheet.Rows[1].Cells[0].CellFormat.Font.Height = 10 * 20;
        }

        //private void GridOnInitializeRow(object sender, RowEventArgs e)
        //{
        //    var test = e.Row;
        //    var month = e.Row.Items.FindItemByKey("MONAT").Value.ConvertTo<int>();
        //    var monthName = Utils.GetMonthName(month, "MMMM");
        //    var cell = e.Row.Items.FindItemByKey("MONAT");

        //    //cell.Tag = month;

        //    if (Utils.IsHeizMonat(month))
        //    {
        //        cell.Text = monthName;
        //    }
        //    else
        //    {
        //        cell.Text = string.Format("({0})", monthName);
        //    }
        //}


        //private void WebGrid_InitializeFooter(ref WebDataGrid webGrid)
        //{
        //    var newRow = new GridRecord();
        //    var uwgCell = new UnboundGridRecordItem(newRow.Items);

        //    //Summe Heizperiode

        //    uwgCell.Text = "Summe Heizperiode";

        //    dynamic cellsCount = webGrid.Columns.Count - 1;
        //    for (int i = 1; i <= cellsCount; i++)
        //    {
        //        uwgCell = new UnboundGridRecordItem(newRow.Items);
        //        uwgCell.Text = CalcColumnSumme(ref webGrid, i, true).ToString();
        //    }

        //    //Summe Jahr
        //    var newRow2 = new GridRecord();
        //    uwgCell = new UnboundGridRecordItem(newRow2.Items);

        //    uwgCell.Text = "Summe Jahr";


        //    for (int i = 1; i <= cellsCount; i++)
        //    {
        //        uwgCell = new UnboundGridRecordItem(newRow2.Items);
        //        uwgCell.Text = CalcColumnSumme(ref webGrid, i, false).ToString();

        //    }
        //    webGrid.Rows.Add(newRow);
        //    webGrid.Rows.Add(newRow2);

        //}


        //private double CalcColumnSumme(ref WebDataGrid webGrid, int colNumber, bool heizperiode = false)
        //{
        //    double sum = 0.0;
        //    foreach ( GridRecord row in webGrid.Rows) {
        //        if ((heizperiode == true)) {
        //            int month = row.Items.FindItemByKey("MONAT").Value.ConvertTo<int>();
        //            if ((month < 6 | month > 8)) {
        //                sum += Convert.ToDouble(row.Items[colNumber].Value);
        //            }
        //        } else {
        //            sum += Convert.ToDouble(row.Items[colNumber].Value);
        //        }
        //    }
        //    return sum;
        //}


        //private void initGridLayout(ref UltraGrid webGrid)
        //{

        //    var font = webGrid.Font;
        //    font.Name = "Microsoft Sans Serif";
        //    font.Size = System.Web.UI.WebControls.FontUnit.Point(8);
        //    var _with3 = webGrid.DisplayLayout;
        //    _with3.Rows. = Color.Silver;
        //    //_with3.RowStyleDefault.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);

        //    //webGrid.fo .HeaderStyleDefault.BackColor = Color.LightGray;
        //    webGrid.HeaderStyleDefault.HorizontalAlign = HorizontalAlign.Center;
        //    webGrid.RowHeightDefault = System.Web.UI.WebControls.Unit.Pixel(20);
        //    webGrid.FooterStyleDefault.HorizontalAlign = HorizontalAlign.Right;
        //}
    }
}
