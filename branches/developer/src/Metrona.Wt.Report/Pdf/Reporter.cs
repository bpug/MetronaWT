namespace Metrona.Wt.Reports.Pdf
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;

    using Infragistics.Documents.Reports.Graphics;
    using Infragistics.Documents.Reports.Report;
    using Infragistics.Documents.Reports.Report.Band;
    using Infragistics.Documents.Reports.Report.Grid;
    using Infragistics.Documents.Reports.Report.QuickList;
    using Infragistics.Documents.Reports.Report.Section;
    using Infragistics.Documents.Reports.Report.Segment;
    using Infragistics.Documents.Reports.Report.Text;
    using Infragistics.WebUI.UltraWebChart;

    using Metrona.Wt.Core;
    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Meteo;
    using Metrona.Wt.Reports.Charts;
    using Metrona.Wt.Service.Extensions;

    public class Reporter : IReporter
    {

        #region "Attributes"

        protected Report document;
        protected ISegment segment;

        protected IBand currentBand;
        public Report WTReport
        {
            get { return this.document; }
        }

        protected enum PdfFont
        {
            DefaultSmallReg,
            DefaultSmallerReg,
            DefaultSmallerUl,
            DefaultNormalReg,
            DefaultNormalBold,
            DefaultNormalBoldUl,
            DefaultNormalBoldIt,
            DefaultNormalUl,
            DefaultLargeReg,
            DefaultLargeBold,
            DefaultLargerReg,
            DefaultLargerBold,
            HeadingLargeBold,
            HeaderNormalReg,
            HeaderNormalBold
        }

        protected Dictionary<PdfFont, Infragistics.Documents.Reports.Graphics.Font> pdfFonts = new Dictionary<PdfFont, Infragistics.Documents.Reports.Graphics.Font>();
        #endregion
        protected Dictionary<PdfFont, System.Drawing.Font> GdiFonts = new Dictionary<PdfFont, System.Drawing.Font>();

        #region "Constructor and dispose"

        public Reporter()
        {
            this.document = new Report();
            this.document.Info.Title = "Witterungstelegram";
            this.InitializeFonts();
        }

        private void InitializeFonts()
        {
            String strUnifiedFontName = "Arial";

            this.GdiFonts = new Dictionary<PdfFont, System.Drawing.Font>();
            this.GdiFonts.Add(PdfFont.DefaultSmallReg, new System.Drawing.Font(strUnifiedFontName, 7));
            this.GdiFonts.Add(PdfFont.DefaultSmallerReg, new System.Drawing.Font(strUnifiedFontName, 6));
            this.GdiFonts.Add(PdfFont.DefaultSmallerUl, new System.Drawing.Font(strUnifiedFontName, 6, System.Drawing.FontStyle.Underline));
            this.GdiFonts.Add(PdfFont.DefaultNormalReg, new System.Drawing.Font(strUnifiedFontName, 8));
            this.GdiFonts.Add(PdfFont.DefaultNormalBold, new System.Drawing.Font(strUnifiedFontName, 8, System.Drawing.FontStyle.Bold));
            this.GdiFonts.Add(PdfFont.DefaultNormalBoldUl, new System.Drawing.Font(strUnifiedFontName, 8, System.Drawing.FontStyle.Underline | System.Drawing.FontStyle.Bold));
            this.GdiFonts.Add(PdfFont.DefaultNormalBoldIt, new System.Drawing.Font(strUnifiedFontName, 8, System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Bold));
            this.GdiFonts.Add(PdfFont.DefaultNormalUl, new System.Drawing.Font(strUnifiedFontName, 8, System.Drawing.FontStyle.Underline));
            this.GdiFonts.Add(PdfFont.DefaultLargeReg, new System.Drawing.Font(strUnifiedFontName, 9));
            this.GdiFonts.Add(PdfFont.DefaultLargeBold, new System.Drawing.Font(strUnifiedFontName, 9, System.Drawing.FontStyle.Bold));
            this.GdiFonts.Add(PdfFont.DefaultLargerReg, new System.Drawing.Font(strUnifiedFontName, 10));
            this.GdiFonts.Add(PdfFont.DefaultLargerBold, new System.Drawing.Font(strUnifiedFontName, 10, System.Drawing.FontStyle.Bold));
            this.GdiFonts.Add(PdfFont.HeadingLargeBold, new System.Drawing.Font(strUnifiedFontName, 14, System.Drawing.FontStyle.Bold));
            this.GdiFonts.Add(PdfFont.HeaderNormalReg, new System.Drawing.Font(strUnifiedFontName, 7));
            this.GdiFonts.Add(PdfFont.HeaderNormalBold, new System.Drawing.Font(strUnifiedFontName, 7, System.Drawing.FontStyle.Bold));

            foreach (PdfFont enmKnownFont in this.GdiFonts.Keys)
            {
                this.pdfFonts.Add(enmKnownFont, new Font(this.GdiFonts[enmKnownFont]));
            }


        }


        // To detect redundant calls

        private bool disposedValue = false;
        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.currentBand = null;
                    this.document = null;
                }

                // TODO: free your own state (unmanaged objects).
                // TODO: set large fields to null.
            }
            this.disposedValue = true;
        }

        #region " IDisposable Support "
        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #endregion

        public virtual void AddNewSection()
        {
            // Add new section to the document
            ISection newSection = this.document.AddSection();

            var footer = newSection.AddFooter();
            footer.Height = 20;
            var text = footer.AddText(20, 0);
            text.Style.Font = this.pdfFonts[PdfFont.DefaultSmallerReg];
            text.AddContent("Erzeugungsdatum: " + DateTime.Now.ToShortDateString());

            // Set page size and paddings
            newSection.PageSize = PageSizes.A4;
            newSection.PagePaddings.Left = 20;
            newSection.PagePaddings.Right = 20;
            newSection.PagePaddings.Top = 10;
            newSection.PagePaddings.Bottom = 20;

            //Add page numbering
            PageNumbering pageNumber = newSection.PageNumbering;
            pageNumber.Style = new  Style(this.pdfFonts[PdfFont.DefaultSmallReg], Brushes.Black);
            pageNumber.Template = "Seite [Page #]";
            // von [LastPageInSection]"
            pageNumber.SkipFirst = false;
            pageNumber.Continue = false;
            pageNumber.Alignment.Horizontal = Alignment.Right;
            pageNumber.Alignment.Vertical = Alignment.Bottom;
            pageNumber.OffsetY = -12;
            pageNumber.OffsetX = 0;

            // Add a new band to the newly created section
            this.currentBand = newSection.AddBand();
        }


        public void AddHeader(DateTime startDate, string requestText, string logoPath)
        {
            // Add a repeating band header to the main band
            IBandHeader bandHeader = this.currentBand.Header;

            // Header settings
            bandHeader.Repeat = true;
            bandHeader.Height = new FixedHeight(80);
            bandHeader.Alignment = new ContentAlignment(Alignment.Left, Alignment.Middle);
            var logo = new Infragistics.Documents.Reports.Graphics.Image(logoPath);
            logo.Preferences.Compressor = ImageCompressors.Flate;

            //' Add a grid to header
            IGrid grid = bandHeader.AddGrid();

            // Arrays for columns and cells
            IGridColumn[] headerColumn = new IGridColumn[5];
            IGridCell[] headerGridCell = new IGridCell[4];

            // Add two columns to the grid
            headerColumn[0] = grid.AddColumn();
            headerColumn[0].Width = new FixedWidth(270);
            headerColumn[1] = grid.AddColumn();
            headerColumn[1].Width = new FixedWidth(20);
            headerColumn[2] = grid.AddColumn();

            // Add one row to the grid
            IGridRow gridRow = grid.AddRow();

            // Add grid cell for logo
            headerGridCell[0] = gridRow.AddCell();
            //cellPattern.Apply(headerGridCell(0))
            headerGridCell[0].Alignment = ContentAlignment.Left;
            headerGridCell[0].AddImage(logo);
            gridRow.AddCell();


            headerGridCell[1] = gridRow.AddCell();
            IGrid dataGrid = headerGridCell[1].AddGrid();
            headerColumn[0] = dataGrid.AddColumn();
            headerColumn[0].Width = new FixedWidth(150);
            dataGrid.AddColumn();

            GridCellPattern dataCellPattern = new GridCellPattern();
            dataCellPattern.Alignment = new ContentAlignment(Alignment.Center, Alignment.Middle);
            dataCellPattern.Paddings.Left = 5;
            dataCellPattern.Paddings.Bottom = 10;

            gridRow = dataGrid.AddRow();

            IText text ;
            headerGridCell[0] = gridRow.AddCell();
            dataCellPattern.Apply(headerGridCell[0]);
            text = headerGridCell[0].AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent("Ende Abrechnungszeitraum (Stichtag)");

            headerGridCell[1] = gridRow.AddCell();
            dataCellPattern.Apply(headerGridCell[1]);
            text = headerGridCell[1].AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent(startDate.ToShortDateString());

            gridRow = dataGrid.AddRow();

            headerGridCell[0] = gridRow.AddCell();
            dataCellPattern.Apply(headerGridCell[0]);
            text = headerGridCell[0].AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent("Region");

            headerGridCell[1] = gridRow.AddCell();
            dataCellPattern.Apply(headerGridCell[1]);
            text = headerGridCell[1].AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent(requestText);


            //'cellPattern.Apply(headerGridCell(1))
            //'headerGridCell(1).ColSpan = 1
            //Dim headingText As IText = headerGridCell(1).AddText()
            //headingText.Style.Font = pdfFonts(PdfFont.DefaultSmallReg)
            //headingText.AddContent(requestText)

        }

        public void AddInfo()
        {
            IText text = this.currentBand.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent(
@"Um Heizenergieverbräuche und die damit verbundenen Kosten beurteilen zu können, muss die Witterung während der Erfassungsperiode als bedeutsamer Einflussfaktor berücksichtigt werden. Dabei sind sowohl starke regionale Unterschiede als auch Unterschiede im zeitlichen Verlauf von einer Heizperiode zur anderen möglich. 
Mit dem Witterungstelegramm erhalten Sie Informationen zum ortsgenauen Klimaverlauf  über mehrere Jahre bis hin zu tagesgenauen Temperaturwerten.");
            text.AddLineBreak();
            text.AddLineBreak();
        }

        public void AddJahresbetrachtung( MeteoGtzYear meteoGtzYear)
        {

            this.CreateHeading("1. Jahresbetrachtung der heizwirksamen Temperatur des aktuellen Jahres im Vergleich zu den Vorjahren und Langszeitmittel²");

            // Add grid 
            IGrid grid = this.currentBand.AddGrid();

            IText text;
            Style textStyle = new Style(this.pdfFonts[PdfFont.DefaultNormalReg], Brushes.Black);
            Style headerStyle = new Style(this.pdfFonts[PdfFont.DefaultNormalBold], Brushes.Black);


            // Grid pattern for borders around block
            var gridPattern = new GridPattern
            {
                Borders = new Borders(new Pen(Colors.Black)),
                Paddings =
                {
                    All = 10
                }
            };
            gridPattern.Apply(grid);

            var column = grid.AddColumn();
            var gridRow = grid.AddRow();
            var gridCell = gridRow.AddCell();


            GridCellPattern cellPattern = new GridCellPattern
            {
                Alignment = new ContentAlignment(Alignment.Center, Alignment.Middle),
                Paddings =
                {
                    Left = 10,
                    Bottom = 0
                }
            };

            //cellPattern.Apply(gridCell);
            //text = gridCell.AddText();
            //text.Style.Font = pdfFonts[PdfFont.DefaultNormalReg];
            //text.AddContent("Alle Angaben sind immer konkret bezogen auf die getroffene Auswahl hinsichtlich Abrech-nungszeitraum (Stichtag) " + "und die ausgewählte Region. Basis sind dabei immer die Tages-mitteltemperaturen. Betrachtet werden immer " + "ganzjährige Abrechnungszeiträume.");
            //text.AddLineBreak();
            //text.AddLineBreak();

            //Add Chart
            gridRow = grid.AddRow();
            gridCell = gridRow.AddCell();

            ICanvas canvas = gridCell.AddCanvas();
            canvas.Width = new FixedWidth(440);
            canvas.Height = new FixedHeight(190);
            using (System.Drawing.Graphics g = canvas.CreateGraphics())
            {
                UltraChart myChart = JahresbetrachtungChart.GetChart(meteoGtzYear, 440, 190, true);
                //myChart.ColumnChart.ChartText(0).Visible = True
                myChart.Legend.Visible = false;
                myChart.RenderPdfFriendlyGraphics(g);
            }
                
            gridRow = grid.AddRow();
            gridCell = gridRow.AddCell();
            cellPattern.Apply(gridCell);

            text = gridCell.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            //text.Paddings.Top = 5;

            var relativeData = meteoGtzYear.ToRelativeData();

            double vorjahrBedarf = relativeData.Period2;
            double lgtzBedarf = relativeData.Lgtz;
            var vorjahrBedarfText = string.Format(
                        @"In der gewählten Region war es im gleichen Zeitraum des Vorjahres {0}% {1} als im betrachtenten Zeiraum des aktuellen Jahres.
Entsprechend ist im aktuellen Jahr im Vergleich zum Vorjahreszeitraum mit einem Heiz{2}bedarf zu rechnen",
                Math.Abs(Math.Round(vorjahrBedarf, 2)), (vorjahrBedarf > 0 ? "wärmer" : "kälter"), (vorjahrBedarf > 0 ? "mehr" : "minder"));
            var lgtzBedarfText = string.Format("In der gewählten Region ist das Langszeitmittel {0}% {1} als das aktuellen Jahr.", Math.Abs(Math.Round(lgtzBedarf, 2)), (lgtzBedarf > 0 ? "wärmer" : "kälter"));

            //text.AddLineBreak();
            text.AddContent(vorjahrBedarfText);
            text.AddLineBreak();
            text.AddLineBreak();
            text.AddContent(lgtzBedarfText);
            
        }


        public void AddMonatsbetrachtung(DataTable chartData, List<Zeitraum> zeitraums)
        {
            if ((chartData == null))
            {
                return;
            }

            //Dummy Column für Legende Langszeitmittel
            var columnDummy = new DataColumn
            {
                DataType = typeof(double)
            };
            chartData.Columns.Add(columnDummy);

            var periods = zeitraums.OrderByDescending(p => p.Start).GetFormatted(true);

            this.CreateHeading("2. Monatsbetrachtung der Temperatur des aktuellen Jahres im Vergleich zum Vorjahr und Langszeitmittel²");

            //Dim flow As Infragistics.Documents.Report.Flow.IFlow = currentBand.AddFlow()
            //flow.Borders.All = New Border(New Pen(Colors.Black))
            IText text ;
            ICanvas legendCanvas;

            // Add grid 
            IGrid grid = this.currentBand.AddGrid();

            // Grid pattern for borders around block
            GridPattern gridPattern = new GridPattern
            {
                Borders = new Borders(new Pen(Colors.Black)),
                Paddings =
                {
                    All = 10
                }
            };
            gridPattern.Apply(grid);

            IGridColumn column = grid.AddColumn();
            IGridRow gridRow = grid.AddRow();
            IGridCell gridCell = gridRow.AddCell();

            GridCellPattern cellPattern = new GridCellPattern
            {
                Alignment = new ContentAlignment(Alignment.Left, Alignment.Middle)
            };
            //cellPattern.Borders = New Borders(New Pen(New Color(0, 0, 0)))
            //cellPattern.Paddings.All = 0

            //Add Legend
            gridCell.Alignment = new ContentAlignment(Alignment.Center, Alignment.Middle);
            IGrid legendGrid = gridCell.AddGrid();
            IGridCell legendCell;

            legendGrid.Width = new FixedWidth(450);
            column = legendGrid.AddColumn();
            column.Width = new FixedWidth(7);
            column = legendGrid.AddColumn();
            column = legendGrid.AddColumn();
            column.Width = new FixedWidth(7);
            column = legendGrid.AddColumn();
            column = legendGrid.AddColumn();
            column.Width = new FixedWidth(7);
            column = legendGrid.AddColumn();
            gridRow = legendGrid.AddRow();

            //Vorjahr
            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            legendCanvas = legendCell.AddCanvas();
            legendCanvas.Width = new FixedWidth(5);
            legendCanvas.Paddings.All = 0;
            legendCanvas.Margins.All = 0;
            legendCanvas.Height = new FixedHeight(5);
            legendCanvas.Brush = new SolidColorBrush(new Color(0, 76, 148));
            legendCanvas.DrawRectangle(0, 0, 5, 5, PaintMode.FillStroke);

            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            text = legendCell.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent(periods[0]);

            //Langszeitmittel 
            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            legendCanvas = legendCell.AddCanvas();
            legendCanvas.Width = new FixedWidth(5);
            legendCanvas.Height = new FixedHeight(5);
            legendCanvas.Brush = new SolidColorBrush(new Color(236, 98, 42));
            legendCanvas.DrawRectangle(0, 0, 5, 5, PaintMode.FillStroke);

            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            text = legendCell.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent(periods[1]);

            //Ausgangbasis 
            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            legendCanvas = legendCell.AddCanvas();
            legendCanvas.Width = new FixedWidth(5);
            legendCanvas.Height = new FixedHeight(5);
            legendCanvas.Pen = new Pen(Colors.Green, 2);
            legendCanvas.DrawLine(0, 2.5f, 5, 2.5f);

            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            text = legendCell.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent("Langszeitmittel (Nulllinie)");


            //Add Chart
            gridRow = grid.AddRow();
            gridCell = gridRow.AddCell();

            ICanvas canvas = gridCell.AddCanvas();
            //Me.currentBand.AddCanvas()
            canvas.Width = new FixedWidth(500);
            canvas.Height = new FixedHeight(250);
            using (System.Drawing.Graphics g = canvas.CreateGraphics())
            {
                UltraChart myChart = MonatsRelativeVerteilungJahrChart.GetChart(chartData, 500, 250, periods.ToArray());
                //myChart.ColumnChart.ChartText(0).Visible = True
                myChart.Legend.Visible = false;
               myChart.RenderPdfFriendlyGraphics(g);
            }

            //gridCell.AddStretcher()

            //Add Info
            GridCellPattern cellPattern2 = new GridCellPattern
            {
                Paddings =
                {
                    Left = 10,
                    Bottom = 0
                }
            };

            gridRow = grid.AddRow();
            gridCell = gridRow.AddCell();
            cellPattern2.Apply(gridCell);


            DataRow row = chartData.Rows[chartData.Rows.Count - 1];
            try
            {
                string monat = Utils.GetMonthName(row["Monat"].ConvertTo<int>(), "MMMM");
                double VorjahrBedarf = row.Field<double>("Vorjahr");
                double aktuellJqahr = row.Field<double>("Aktuelljahr");

                text = gridCell.AddText();
                text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
                //text.Paddings.Top = 10
                text.AddLineBreak();
                text.AddContent("Das Langszeitmittel stellt die Nulllinie dar.");
                text.AddLineBreak();
                text.AddLineBreak();
                text.AddContent("Negativer Prozentwert (Balken zeigt nach unten): in dem jeweiligen Monat des Vorjahres / aktuellen Jahres war es kälter als im gleichen Monats des Langszeitmittels.");
                text.AddLineBreak();
                text.AddLineBreak();
                text.AddContent("Positiver Prozentwert (Balken zeigt nach oben): in dem jeweiligen Monat des Vorjahres / aktuellen Jahres war es wärmer als im gleichen Monats des Langszeitmittels.");
                text.AddLineBreak();
                text.AddLineBreak();
                text.AddContent(string.Format("Für die gewählte Region war es - bezogen auf das Langszeitmittel im Monat {0}", monat));

                IQuickList list = gridCell.AddQuickList();
                list.Margins.Bottom = 0;
                list.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
                list.Interval = 1;
                list.AddItem(string.Format("des Vorjahres ca. {0}% {1}", Math.Abs(Math.Round(VorjahrBedarf, 2)), (VorjahrBedarf > 0 ? "wärmer" : "kälter")));
                list.AddItem(string.Format("des aktuellen Jahres ca. {0}% {1}",   Math.Abs(Math.Round(aktuellJqahr, 2)), (aktuellJqahr > 0 ? "wärmer" : "kälter")));
                //gridCell.AddStretcher();

            }
            catch (Exception ex)
            {
            }
            this.currentBand.AddStretcher();
        }



        public void AddTagesmitteltemperaturen(object chartData, List<Zeitraum> zeitraums )
        {
            var periods = zeitraums.OrderByDescending(p => p.Start).GetFormatted(true);

            this.CreateHeading("3. Tagesmitteltemperaturen³ des gewählten Abrechnungszeitraumes und des Vorjahres");
            ICanvas legendCanvas = default(ICanvas);
            IText text = default(IText);

            // Add grid 
            IGrid grid = this.currentBand.AddGrid();
            GridCellPattern cellPattern = new GridCellPattern();
            cellPattern.Alignment = new ContentAlignment(Alignment.Left, Alignment.Middle);

            // Grid pattern for borders around block
            GridPattern gridPattern = new GridPattern();
            gridPattern.Borders = new Borders(new Pen(new Color(0, 0, 0)));
            gridPattern.Paddings.All = 10;
            gridPattern.Apply(grid);

            IGridColumn column = grid.AddColumn();
            IGridRow gridRow = grid.AddRow();
            IGridCell gridCell = gridRow.AddCell();


            //Add Legend
            gridCell.Alignment = new ContentAlignment(Alignment.Center, Alignment.Middle);
            IGrid legendGrid = gridCell.AddGrid();
            IGridCell legendCell;

            legendGrid.Width = new FixedWidth(450);
            column = legendGrid.AddColumn();
            column.Width = new FixedWidth(7);
            column = legendGrid.AddColumn();
            column = legendGrid.AddColumn();
            column.Width = new FixedWidth(7);
            column = legendGrid.AddColumn();
            column = legendGrid.AddColumn();
            column.Width = new FixedWidth(7);
            column = legendGrid.AddColumn();
            gridRow = legendGrid.AddRow();

            //1 Period
            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            legendCanvas = legendCell.AddCanvas();
            legendCanvas.Width = new FixedWidth(5);
            legendCanvas.Paddings.All = 0;
            legendCanvas.Margins.All = 0;
            legendCanvas.Height = new FixedHeight(5);
            legendCanvas.Pen = new Pen(new Color(65, 69, 120), 2);
            legendCanvas.DrawLine(0, 2.5f, 5, 2.5f);

            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            text = legendCell.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent(periods[0]);

            //2 Period 
            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            legendCanvas = legendCell.AddCanvas();
            legendCanvas.Width = new FixedWidth(5);
            legendCanvas.Height = new FixedHeight(5);
            legendCanvas.Pen = new Pen(new Color(204, 76, 24), 2);
            legendCanvas.DrawLine(0, 2.5f, 5, 2.5f);

            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            text = legendCell.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent(periods[1]);

            //Ausgangbasis 
            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            legendCanvas = legendCell.AddCanvas();
            legendCanvas.Width = new FixedWidth(5);
            legendCanvas.Height = new FixedHeight(5);
            legendCanvas.Pen = new Pen(Colors.Green, 2);
            legendCanvas.DrawLine(0, 2.5f, 5, 2.5f);

            legendCell = gridRow.AddCell();
            cellPattern.Apply(legendCell);
            text = legendCell.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent("Heizgrenztemperatur");
            //text.AddContent("Heizgrenztemperatur⁴");

            //Add Chart
            //chartData.Columns("Heizgrenztemperatur").ColumnName = "Heizgrenztemperatur³"
            gridRow = grid.AddRow();
            gridCell = gridRow.AddCell();

            ICanvas canvas = gridCell.AddCanvas();
            //Me.currentBand.AddCanvas()
            canvas.Width = new FixedWidth(530);
            canvas.Height = new FixedHeight(300);
            using (System.Drawing.Graphics g = canvas.CreateGraphics())
            {
                using (UltraChart myChart = new UltraChart())
                {
                    TemperaturChart.GetChart(myChart, chartData, 530, 300);
                    myChart.Legend.Visible = false;
                    myChart.RenderPdfFriendlyGraphics(g);
                }
            }
            //this.currentBand.AddStretcher();
        }

        public void AddSpacing(int height)
        {
            var flow = this.currentBand.AddFlow();
            flow.Height = new FixedHeight(height);
        }


        public void AddRemarks()
        {
            
            this.CreateHeading("Erläuterungen");

            Style style1 = new Style(this.pdfFonts[PdfFont.DefaultLargeBold], Brushes.Black);
            Style style2 = new Style(this.pdfFonts[PdfFont.DefaultNormalReg], Brushes.Black);
            Style style3 = new Style(new Font("Arial Black", 9, FontStyle.Bold), Brushes.Black);


            //IGroup flow = this.currentBand.AddGroup();
            //flow.Borders.All = new Border(new Pen(Colors.Black));
            //flow.Paddings.All = 10;

            IGrid grid = this.currentBand.AddGrid();
            grid.Borders.All = new Border(new Pen(Colors.Black));
            grid.Paddings.All = 10;
            IGridColumn column = grid.AddColumn();
            IGridRow gridRow = grid.AddRow();
            IGridCell gridCell = gridRow.AddCell();



            IText text = gridCell.AddText();
            //text.Style.Font = pdfFonts(PdfFont.DefaultLargeBold)
            //text.Style.Font.Bold = True

            text.AddContent("¹ ", style3);
            text.AddContent("Heizperiode:", style1);
            text.AddLineBreak();
            text.AddContent("Als Heizperiode werden üblicherweise die Monate September bis Mai angesehen. Nicht zur Heizperiode gehören die Monate Juni bis August eines Jahres.", style2);
            text.AddLineBreak();
            text.AddLineBreak();

            text.AddContent("² ", style3);
            text.AddContent("Langszeitmittel:", style1);
            text.AddLineBreak();

            text = gridCell.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent("Beim Langszeitmittel werden die seit 1993 vorliegenden Klimadaten der gewählten Region in Abhängigkeit des jeweiligen Abrechnungszeitraums zugrunde gelegt.", style2);
            text.AddLineBreak();
            text.AddLineBreak();

            text.AddContent("³", style3);
            text.AddContent(" Tagesmitteltemperatur:", style1);
            text.AddLineBreak();
            text.AddContent("Die Tagesmitteltemperatur ist die Durchschnittstemperatur im Zeitraum von 0 bis 0 Uhr des jeweiligen Tages.", style2);
            text.AddLineBreak();
            text.AddLineBreak();

            text.AddContent("⁴ ", style3);
            text.AddContent("Heizgrenztemperatur:", style1);
            text.AddLineBreak();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultNormalReg];
            text.AddContent("Die Heizgrenztemperatur liegt bei 15° C. Bei Temperaturen von unter 15° C wird normativ  von der Inbetriebnahme der Heizanlage ausgegangen.", style2);
            text.AddLineBreak();
            text.AddLineBreak();
           
        }

        public void GetStream(Stream stream)
        {
            this.WTReport.Publish(stream, FileFormat.PDF);
        }

        public byte[] GetArray()
        {
            byte[] output;
            using (var stream = new MemoryStream())
            {
                this.WTReport.Publish(stream, FileFormat.PDF);
                output = stream.ToArray();
            }
            return output;
        }

        public void Download(string fileName)
        {
            var response = HttpContext.Current.Response;

            byte[] output = this.GetArray();
            response.Clear();
            response.ClearHeaders();
            //Response.AddHeader("Content-Disposition", "inline")
            response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            response.AddHeader("Content-Length", output.Length.ToString());
            response.ContentType = "application/pdf";
            response.Buffer = true;
            response.BinaryWrite(output);
            response.Flush();
            response.End();
        }


        public virtual void CreateHeading(string headingText)
        {
            // Heading
            IText text = this.currentBand.AddText();
            text.Style.Font = this.pdfFonts[PdfFont.DefaultLargeReg];
            text.Alignment = TextAlignment.Center;
            text.AddContent(headingText);
            text.Paddings.Top = 3;
            text.Paddings.Bottom = 3;
            text.Borders.Left = new Border(new Pen(Colors.Black));
            text.Borders.Right = new Border(new Pen(Colors.Black));
            text.Borders.Top = new Border(new Pen(Colors.Black));
            text.Borders.Bottom = new Border(new Pen(Colors.Black));
            text.Background = new Background(Colors.LightGray);
        }

        private System.Drawing.SizeF GetTextSize(string text, System.Drawing.Font font)
        {
            System.Drawing.SizeF textSize ;

            using (System.Drawing.Bitmap b = new System.Drawing.Bitmap(1, 1))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b))
                {
                    textSize = g.MeasureString(text, font);
                }
            }
            return textSize;
        }

    }
}
