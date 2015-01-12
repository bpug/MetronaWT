//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Charts.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Reports.Charts
{
    using System;
    using System.Collections;
    using System.Drawing;

    using Infragistics.UltraChart.Core;
    using Infragistics.UltraChart.Core.Primitives;
    using Infragistics.UltraChart.Resources;
    using Infragistics.UltraChart.Resources.Appearance;
    using Infragistics.UltraChart.Shared.Events;
    using Infragistics.UltraChart.Shared.Styles;
    using Infragistics.WebUI.UltraWebChart;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public static class MonatsRelativeVerteilungJahrChart
    {
        

        private static readonly WebDeploymentScenario deploymentScenario = new WebDeploymentScenario();

      

        static MonatsRelativeVerteilungJahrChart()
        {
            
            deploymentScenario.Scenario = ImageDeploymentScenario.Session;
        }

        public static UltraChart GetChart(object datasource, int width, int height)
        {
            var chart = new UltraChart();

            chart.FillSceneGraph += ChartOnFillSceneGraph;
            //.ID = "ChartVergleichJahr"
            chart.DeploymentScenario = deploymentScenario;
            chart.Width = width;
            chart.Height = height;
            chart.Border.Thickness = 0;

            var colorModel = chart.ColorModel;
            colorModel.ModelStyle = ColorModels.CustomLinear;
            //.AlphaLevel = 150
            colorModel.CustomPalette = Constants.ChartColors;

            var gradientEffect = new GradientEffect
            {
                Coloring = GradientColoringStyle.Darken
            };
            chart.Effects.Add(gradientEffect);


            chart.ChartType = ChartType.ColumnChart;
            chart.ImagePipePageName = Constants.ImagePipePageName;

            var data = chart.Data;
            data.SetColumnLabels(new string[] {
		        "Vorjahr",
		        "Aktuelles Jahr",
		        "Langzeitmittel (Nulllinie)"
	        });
            data.UseRowLabelsColumn = true;
            data.ZeroAligned = true;

            var cta = new ChartTextAppearance();
            cta.ItemFormatString = "<DATA_VALUE:#0.00>";
            cta.Visible = false;
            cta.ChartTextFont = new Font("Arial", 5, FontStyle.Regular, GraphicsUnit.Point);
            cta.Column = -2;
            cta.Row = -2;
            cta.VerticalAlign = StringAlignment.Far;
            cta.HorizontalAlign = StringAlignment.Center;

            var columnChart = chart.ColumnChart;
            columnChart.ChartText.Add(cta);
            columnChart.ColumnSpacing = 0;
            columnChart.SeriesSpacing = 1;


            var legend = chart.Legend;
            legend.BackgroundColor = Color.Transparent;
            legend.Visible = true;
            legend.Location = LegendLocation.Top;
            legend.SpanPercentage = 10;
            legend.BorderThickness = 0;
            legend.DataAssociation = ChartTypeData.LineData;
            var margins = legend.Margins;
            margins.Bottom = 1;
            margins.Top = 1;
            margins.Left = 50;
            margins.Right = 1;

            //'*** Implementierungs IRenderLabel für die Labels-Formatierung 
            Hashtable labelHash = new Hashtable();
            labelHash.Add("MY_MONTH_LABEL", new MonthLabelRenderer());
            chart.LabelHash = labelHash;
            chart.Axis.X.Labels.SeriesLabels.Format = Infragistics.UltraChart.Shared.Styles.AxisSeriesLabelFormat.Custom;
            chart.Axis.X.Labels.SeriesLabels.FormatString = "<MY_MONTH_LABEL>";
            //'*** END Implementierungs IRenderLabel-Interface für die Labels-Formatierung

            var axisX = chart.Axis.X;
            axisX.Extent = 5;
            axisX.LineColor = Color.Green;
            axisX.LineThickness = 1;
            axisX.TickmarkStyle = AxisTickStyle.Smart;
            axisX.TickmarkInterval = 1;
            axisX.Labels.Visible = false;
            //.Labels.ItemFormatString = "<ITEM_LABEL>"
            axisX.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            axisX.Labels.HorizontalAlign = StringAlignment.Near;
            axisX.Labels.VerticalAlign = StringAlignment.Center;
            axisX.Labels.Font = new Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point);
            axisX.Labels.FontColor = Color.DimGray;

            //.Labels.SeriesLabels.Format = Infragistics.UltraChart.Shared.Styles.AxisSeriesLabelFormat.Custom
            //.Labels.SeriesLabels.FormatString = "<MY_MONTH_LABEL>"
            axisX.Labels.SeriesLabels.Font = new Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point);
            axisX.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisX.Labels.SeriesLabels.FontColor = Color.DimGray;
            axisX.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Far;
            axisX.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;
            axisX.MajorGridLines.Visible = false;
            axisX.MinorGridLines.Visible = false;

            var axisY = chart.Axis.Y;
            axisY.Extent = 15;
            axisY.LineThickness = 1;
            axisY.TickmarkInterval = 1;
            axisY.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            //.RangeType = AxisRangeType.Custom
            //.RangeMin = -3
            //.RangeMax = 2
            //.Margin.Near.Value = 20 'New Infragistics.UltraChart.Resources.Appearance.AxisMargin(
            //.Margin.Far.Value = 20

            axisY.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisY.Labels.HorizontalAlign = StringAlignment.Far;
            axisY.Labels.ItemFormatString = "<DATA_VALUE:0.######>";

            axisY.Labels.SeriesLabels.Font = new Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point);
            axisY.Labels.SeriesLabels.Orientation = TextOrientation.VerticalLeftFacing;
            axisY.Labels.SeriesLabels.FontColor = Color.DimGray;
            axisY.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Far;
            axisY.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;

            var titleLeft = chart.TitleLeft;
            titleLeft.Visible = true;
            titleLeft.Extent = 30;
            titleLeft.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point);
            titleLeft.HorizontalAlign = StringAlignment.Center;
            titleLeft.Text = "Monat im Vergleich zum Langzeitmittel war" + Environment.NewLine + " wärmer / kälter [%]";
            var titleLeftmargins = titleLeft.Margins;
            titleLeftmargins.Bottom = 1;
            titleLeftmargins.Top = 1;
            titleLeftmargins.Left = 1;
            titleLeftmargins.Right = 1;

            var titleTop = chart.TitleTop;
            titleTop.Extent = 33;
            titleTop.Font = new Font("Arial", 8, FontStyle.Bold, GraphicsUnit.Point);

            var titleTopMargins = titleTop.Margins;
            titleTopMargins.Bottom = 10;
            titleTopMargins.Top = 0;
            titleTopMargins.Left = 15;
            titleTopMargins.Right = 0;

            var titleBottom = chart.TitleBottom;
            titleBottom.Text = "";
            titleBottom.Extent = 33;
            titleBottom.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point);
            titleBottom.HorizontalAlign = StringAlignment.Center;
            var titleBottomMargins = titleBottom.Margins;
            titleBottomMargins.Bottom = 1;
            titleBottomMargins.Top = 1;
            titleBottomMargins.Left = 1;
            titleBottomMargins.Right = 1;

            var tooltips = chart.Tooltips;
            tooltips.FormatString = "<DATA_VALUE:0.000000>";

            //var _with16 = chart.Data;
            data.DataSource = datasource;
            data.DataBind();
            try
            {
                data.IncludeColumn("Promille", false);
                data.IncludeColumn("Vorjahr gewichtet", false);
                data.IncludeColumn("Aktuelles Jahr gewichtet", false);

            }
            catch (Exception ex)
            {
            }


            return chart;
        }
      
        private static void ChartOnFillSceneGraph(object sender, FillSceneGraphEventArgs e)
        {
           
            var axisY = e.Grid["Y"] as IAdvanceAxis;
            var axisX = e.Grid["X"] as IAdvanceAxis;

            int targetYCoord = Convert.ToInt32(axisY.Map(0));

            int xStart = Convert.ToInt32(axisX.MapMinimum);
            int xEnd = Convert.ToInt32(axisX.MapMaximum);
            Line targetLine = new Line(new Point(xStart, targetYCoord), new Point(xEnd, targetYCoord))
            {
                PE =
                {
                    Stroke = Color.Green,
                    StrokeWidth = 2
                },
                lineStyle =
                {
                    DrawStyle = LineDrawStyle.Solid
                     
                }
            };
            
            e.SceneGraph.Add(targetLine);
        }

       

        public class MonthLabelRenderer : IRenderLabel
        {
            public string ToString(Hashtable context)
            {

                int month = Convert.ToInt32(context["SERIES_LABEL"]);
                System.DateTime datum = Convert.ToDateTime("01." + month + ".2000");
                string strDatum = String.Format("{0:MMM}", datum);
                //Dim datum As DateTime = CType(context("ITEM_LABEL"), DateTime)
                if ((month > 5 & month < 9))
                {
                    strDatum = string.Format("({0})", strDatum);
                }
                return strDatum;
            }
        }

    }
}