//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Charts.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Reports.Charts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    using Infragistics.UltraChart.Core;
    using Infragistics.UltraChart.Core.Primitives;
    using Infragistics.UltraChart.Core.Util;
    using Infragistics.UltraChart.Resources;
    using Infragistics.UltraChart.Resources.Appearance;
    using Infragistics.UltraChart.Shared.Styles;
    using Infragistics.WebUI.UltraWebChart;

    using Metrona.Wt.Core;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public static class TemperaturChart
    {
        private static readonly WebDeploymentScenario deploymentScenario = new WebDeploymentScenario();
        static TemperaturChart()
        {
            
            deploymentScenario.Scenario = ImageDeploymentScenario.Session;
        }

        //public static UltraChart GetChart2(UltraChart chart, object datasource, int width, int height)
        //{

        //    if ((chart == null))
        //    {
        //        chart = new UltraChart();
        //    }
        //    chart.FillSceneGraph += TemperaturChartFillSceneGraph;
        //    var ultraChart = chart;
        //    ultraChart.AccessKey = "A";
        //    //.ID = "ChartVergleichJahr"
        //    ultraChart.ChartType = ChartType.LineChart;
        //    ultraChart.ImagePipePageName = Constants.ImagePipePageName; ;
        //    ultraChart.DeploymentScenario = deploymentScenario;
        //    ultraChart.Width = width;
        //    ultraChart.Height = height;
        //    ultraChart.Border.Thickness = 0;

        //    var colorModel = ultraChart.ColorModel;
        //    colorModel.ModelStyle = ColorModels.CustomLinear;
        //    //.AlphaLevel = 150
        //    colorModel.CustomPalette = Constants.ChartColors;

        //    var gradientEffect = new GradientEffect
        //    {
        //        Coloring = GradientColoringStyle.Darken
        //    };
        //    ultraChart.Effects.Add(gradientEffect);

        //    //LineAppearance Start
        //    var lineApp1 = new LineAppearance
        //    {
        //        Thickness = 2,
        //        LineStyle =
        //        {
        //            DrawStyle = LineDrawStyle.Dash,
        //            MidPointAnchors = false
        //        },

        //    };
        //    lineApp1.IconAppearance.Icon = SymbolIcon.None;


        //    var lineApp2 = new LineAppearance
        //    {
        //        LineStyle =
        //        {
        //            DrawStyle = LineDrawStyle.Solid,
        //            MidPointAnchors = true,
        //            EndStyle = LineCapStyle.DiamondAnchor
        //        },
        //        Thickness = 2
        //    };
        //    //LineAppearance END

        //    var lineChart = ultraChart.LineChart;
        //    lineChart.LineAppearances.Add(lineApp2);
        //    lineChart.LineAppearances.Add(lineApp2);
        //    lineChart.LineAppearances.Add(lineApp1);
        //    lineChart.NullHandling = NullHandling.InterpolateSimple;
        //    lineChart.TreatDateTimeAsString = false;
        //    lineChart.Thickness = 1;
        //    //*** Line width ****
        //    lineChart.MidPointAnchors = true;

        //    var legend = ultraChart.Legend;
        //    legend.Visible = true;
        //    legend.Location = LegendLocation.Top;
        //    legend.SpanPercentage = 6;
        //    legend.BorderThickness = 0;
        //    legend.AlphaLevel = 15;
        //    legend.DataAssociation = ChartTypeData.DefaultData;
        //    var marginsAppearance = legend.Margins;
        //    marginsAppearance.Bottom = 1;
        //    marginsAppearance.Top = 1;
        //    marginsAppearance.Left = 50;
        //    marginsAppearance.Right = 1;

        //    var axis = ultraChart.Axis;
        //    axis.PE.ElementType = PaintElementType.None;
        //    axis.PE.Fill = Color.Cornsilk;

        //    //'*** Implementierungs IRenderLabel für die Labels-Formatierung 

        //    {
        //        Hashtable labelHash = new Hashtable();
        //        labelHash.Add("MY_MONTH_LABEL", new TempMonthLabelRenderer());
        //        chart.LabelHash = labelHash;
        //        chart.Axis.X.Labels.ItemFormat = AxisItemLabelFormat.Custom;
        //        chart.Axis.X.Labels.ItemFormatString = "<MY_MONTH_LABEL>";
        //    }
        //    //'*** END Implementierungs IRenderLabel-Interface für die Labels-Formatierung

        //    var axisX = ultraChart.Axis.X;
        //    axisX.Extent = 10;
        //    axisX.LineColor = Color.LightGray;
        //    axisX.LineThickness = 1;
        //    axisX.TickmarkStyle = AxisTickStyle.DataInterval;
        //    axisX.TickmarkInterval = 1;
        //    axisX.TickmarkIntervalType = AxisIntervalType.Months;
        //    //axisX.Labels.ItemFormatString = "<ITEM_LABEL:MMM>";
        //    axisX.Labels.Orientation = TextOrientation.Horizontal;
        //    axisX.Labels.VerticalAlign = StringAlignment.Far;
        //    axisX.Labels.HorizontalAlign = StringAlignment.Far;
        //    axisX.Labels.Font = new Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point);
        //    axisX.Labels.FontColor = Color.DimGray;

        //    //axisX.Labels.SeriesLabels.Font = new Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point);
        //    //axisX.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
        //    //axisX.Labels.SeriesLabels.FontColor = Color.DimGray;
        //    //axisX.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Near;
        //    //axisX.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;
        //    axisX.MajorGridLines.Visible = false;
        //    axisX.MinorGridLines.Visible = false;



        //    var axisY = ultraChart.Axis.Y;
        //    axisY.Extent = 15;
        //    axisY.LineThickness = 1;
        //    axisY.TickmarkInterval = 10;
        //    axisY.TickmarkStyle = AxisTickStyle.Smart;

        //    axisY.Labels.Orientation = TextOrientation.Horizontal;
        //    axisY.Labels.HorizontalAlign = StringAlignment.Far;
        //    axisY.Labels.VerticalAlign = StringAlignment.Center;
        //    axisY.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
        //    axisY.Labels.Font = new Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point);
        //    axisY.Labels.FontColor = Color.DimGray;

        //    //.Labels.SeriesLabels.Font = New Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point)
        //    //.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing
        //    //.Labels.SeriesLabels.FontColor = Color.DimGray
        //    //.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Far
        //    //.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center
        //    axisY.MinorGridLines.Visible = false;
        //    //With .MajorGridLines
        //    //    .Visible = True
        //    //    .AlphaLevel = 255
        //    //    .Color = Color.LightGray
        //    //    .DrawStyle = LineDrawStyle.Solid
        //    //    .Thickness = 1
        //    //End With


        //    var titleLeft = ultraChart.TitleLeft;
        //    titleLeft.Visible = true;
        //    titleLeft.Text = "Mittlere Außentemperatur [°C]";
        //    titleLeft.Extent = 15;
        //    titleLeft.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point);
        //    titleLeft.HorizontalAlign = StringAlignment.Center;
        //    var titleLeftMarginswith10 = titleLeft.Margins;
        //    titleLeftMarginswith10.Bottom = 1;
        //    titleLeftMarginswith10.Top = 1;
        //    titleLeftMarginswith10.Left = 1;
        //    titleLeftMarginswith10.Right = 1;

        //    var titleTop = ultraChart.TitleTop;
        //    titleTop.Extent = 33;
        //    titleTop.ClipText = false;
        //    titleTop.Font = new Font("Arial", 8, FontStyle.Bold, GraphicsUnit.Point);
        //    var titleTopMargins = titleTop.Margins;
        //    titleTopMargins.Bottom = 10;
        //    titleTopMargins.Top = 0;
        //    titleTopMargins.Left = 15;
        //    titleTopMargins.Right = 0;

        //    var titleBottom = ultraChart.TitleBottom;
        //    titleBottom.Visible = false;


        //    //' ''*** Implementierungs IRenderLabel für die Labels-Formatierung 
        //    Hashtable labelHash2 = new Hashtable();
        //    labelHash2.Add("MY_TOOLTIP_LABEL", new TemperaturLabelRenderer());
        //    if (chart.LabelHash == null)
        //    {
        //        ultraChart.LabelHash = labelHash2;
        //    }
        //    else
        //    {
        //        ultraChart.LabelHash.Add("MY_TOOLTIP_LABEL", new TemperaturLabelRenderer());
        //    }

        //    var tooltips = ultraChart.Tooltips;
        //    tooltips.FormatString = "<MY_TOOLTIP_LABEL>";
        //    tooltips.Overflow = TooltipOverflow.ClientArea;
        //    tooltips.BorderThickness = 0;
        //    tooltips.EnableFadingEffect = true;

        //    ultraChart.Drill.Enabled = true;
        //    ultraChart.Drill.DrillElements = new DrillElement[] { new DrillElement() };
        //    ultraChart.Drill.DrillElements[0].DrillDown = new TemperaturDrillDown(chart);
        //    var data = ultraChart.Data;
        //    //.UseRowLabelsColumn = True
        //    //.ZeroAligned = True
        //    data.SwapRowsAndColumns = true;
        //    data.EmptyStyle.EnableLineStyle = true;
        //    data.EmptyStyle.EnablePoint = true;
        //    data.EmptyStyle.EnablePE = true;
        //    data.DataSource = datasource;
        //    data.DataBind();
            
        //    return chart;
        //}

        public static UltraChart GetChart(UltraChart chart, object datasource, int width, int height, params string[] columnLabels)
        {
            
            if ((chart == null))
            {
                chart = new UltraChart();
            }

            if (columnLabels != null)
            {
                chart.Data.SetColumnLabels(columnLabels);
            }

            chart.FillSceneGraph += TemperaturChartFillSceneGraph;
            var ultraChart = chart;
            ultraChart.AccessKey = "A";
            //.ID = "ChartVergleichJahr"
            ultraChart.ChartType = ChartType.LineChart;
            ultraChart.ImagePipePageName = Constants.ImagePipePageName;
            ultraChart.DeploymentScenario = deploymentScenario;
            ultraChart.Width = width;
            ultraChart.Height = height;
            ultraChart.Border.Thickness = 0;

            var colorModel = ultraChart.ColorModel;
            colorModel.ModelStyle = ColorModels.CustomLinear;
            //.AlphaLevel = 150
            colorModel.CustomPalette = Constants.ChartColors;

            var gradientEffect = new GradientEffect
            {
                Coloring = GradientColoringStyle.Darken
            };
            ultraChart.Effects.Add(gradientEffect);

            //LineAppearance Start
            var lineApp1 = new LineAppearance
            {
                Thickness = 2,
                LineStyle =
                {
                    DrawStyle = LineDrawStyle.Dash,
                    MidPointAnchors = false
                },
                
            };
            lineApp1.IconAppearance.Icon = SymbolIcon.None;
            lineApp1.IconAppearance.IconSize = SymbolIconSize.Small;

            var lineApp2 = new LineAppearance
            {
                LineStyle =
                {
                    DrawStyle = LineDrawStyle.Solid,
                    MidPointAnchors = true,
                    EndStyle = LineCapStyle.DiamondAnchor
                },
                Thickness = 2
            };
            lineApp2.IconAppearance.IconSize = SymbolIconSize.Small;
            //LineAppearance END

            var lineChart = ultraChart.LineChart;
            lineChart.LineAppearances.Add(lineApp2);
            lineChart.LineAppearances.Add(lineApp2);
            lineChart.LineAppearances.Add(lineApp1);
            lineChart.NullHandling = NullHandling.InterpolateSimple;
            lineChart.TreatDateTimeAsString = false;
            lineChart.Thickness = 1;
            //*** Line width ****
            lineChart.MidPointAnchors = true;

            var legend = ultraChart.Legend;
            legend.Visible = true;
            legend.Location = LegendLocation.Top;
            legend.SpanPercentage = 6;
            legend.BorderThickness = 0;
            legend.AlphaLevel = 15;
            legend.DataAssociation = ChartTypeData.DefaultData;
            var marginsAppearance = legend.Margins;
            marginsAppearance.Bottom = 1;
            marginsAppearance.Top = 1;
            marginsAppearance.Left = 50;
            marginsAppearance.Right = 1;

            var axis = ultraChart.Axis;
            axis.PE.ElementType = PaintElementType.None;
            axis.PE.Fill = Color.Cornsilk;

            //'*** Implementierungs IRenderLabel für die Labels-Formatierung 

            {
                Hashtable labelHash = new Hashtable();
                labelHash.Add("MY_MONTH_LABEL", new TempMonthLabelRenderer());
                chart.LabelHash = labelHash;
                chart.Axis.X.Labels.ItemFormat = AxisItemLabelFormat.Custom;
                chart.Axis.X.Labels.ItemFormatString = "<MY_MONTH_LABEL>";
            }
            //'*** END Implementierungs IRenderLabel-Interface für die Labels-Formatierung

            var axisX = ultraChart.Axis.X;
            axisX.Extent = 15;
            axisX.LineColor = Color.LightGray;
            axisX.LineThickness = 1;
            axisX.TickmarkStyle = AxisTickStyle.DataInterval;
            axisX.TickmarkInterval = 1;
            axisX.TickmarkIntervalType = AxisIntervalType.Months;
            //axisX.Labels.ItemFormatString = "<ITEM_LABEL:MMM>";
            axisX.Labels.Orientation = TextOrientation.Horizontal;
            axisX.Labels.VerticalAlign = StringAlignment.Far;
            axisX.Labels.HorizontalAlign = StringAlignment.Far;
            axisX.Labels.Font = new Font("Verdana", 9, FontStyle.Regular, GraphicsUnit.Point);
            axisX.Labels.FontColor = Color.Black;
            axisX.Labels.Layout.Padding = 15;

            //axisX.Labels.SeriesLabels.Font = new Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point);
            //axisX.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            //axisX.Labels.SeriesLabels.FontColor = Color.DimGray;
            //axisX.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Near;
            //axisX.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;
            axisX.MajorGridLines.Visible = false;
            axisX.MinorGridLines.Visible = false;

            var axisY = ultraChart.Axis.Y;
            axisY.Extent = 30;
            axisY.LineThickness = 1;
            axisY.TickmarkInterval = 10;
            axisY.TickmarkStyle = AxisTickStyle.Smart;

            axisY.Labels.Orientation = TextOrientation.Horizontal;
            axisY.Labels.HorizontalAlign = StringAlignment.Far;
            //axisY.Labels.VerticalAlign = StringAlignment.Center;
            axisY.Labels.ItemFormatString = "<DATA_VALUE:00.######>";
            axisY.Labels.Font = new Font("Verdana", 9, FontStyle.Regular, GraphicsUnit.Point);
            axisY.Labels.FontColor = Color.Black;

            //.Labels.SeriesLabels.Font = New Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point)
            //.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing
            //.Labels.SeriesLabels.FontColor = Color.DimGray
            //.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Far
            //.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center
            axisY.MinorGridLines.Visible = false;
            //With .MajorGridLines
            //    .Visible = True
            //    .AlphaLevel = 255
            //    .Color = Color.LightGray
            //    .DrawStyle = LineDrawStyle.Solid
            //    .Thickness = 1
            //End With


            var titleLeft = ultraChart.TitleLeft;
            titleLeft.Visible = true;
            titleLeft.Text = "Mittlere Außentemperatur [°C]";
            titleLeft.Extent = 15;
            titleLeft.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point);
            titleLeft.HorizontalAlign = StringAlignment.Center;
            var titleLeftMarginswith10 = titleLeft.Margins;
            titleLeftMarginswith10.Bottom = 1;
            titleLeftMarginswith10.Top = 1;
            titleLeftMarginswith10.Left = 1;
            titleLeftMarginswith10.Right = 1;

            var titleTop = ultraChart.TitleTop;
            titleTop.Extent = 33;
            titleTop.ClipText = false;
            titleTop.Font = new Font("Arial", 8, FontStyle.Bold, GraphicsUnit.Point);
            var titleTopMargins = titleTop.Margins;
            titleTopMargins.Bottom = 10;
            titleTopMargins.Top = 0;
            titleTopMargins.Left = 15;
            titleTopMargins.Right = 0;

            var titleBottom = ultraChart.TitleBottom;
            titleBottom.Visible = false;


            //' ''*** Implementierungs IRenderLabel für die Labels-Formatierung 
            Hashtable labelHash2= new Hashtable();
            labelHash2.Add("MY_TOOLTIP_LABEL", new TemperaturLabelRenderer());
            if (chart.LabelHash == null)
            {
                ultraChart.LabelHash = labelHash2;
            }
            else
            {
                ultraChart.LabelHash.Add("MY_TOOLTIP_LABEL", new TemperaturLabelRenderer());
            }

            var tooltips = ultraChart.Tooltips;
            tooltips.FormatString = "<MY_TOOLTIP_LABEL>";
            tooltips.Overflow = TooltipOverflow.ClientArea;
            tooltips.BorderThickness = 0;
            tooltips.EnableFadingEffect = true;

            ultraChart.Drill.Enabled = true;
            ultraChart.Drill.DrillElements = new DrillElement[] { new DrillElement() };
            ultraChart.Drill.DrillElements[0].DrillDown = new TemperaturDrillDown(chart);
            var data = ultraChart.Data;
            //.UseRowLabelsColumn = True
            //.ZeroAligned = True
            data.SwapRowsAndColumns = true;
            data.EmptyStyle.EnableLineStyle = true;
            data.EmptyStyle.EnablePoint = true;
            data.EmptyStyle.EnablePE = true;
            data.DataSource = datasource;
            data.DataBind();

            return chart;
        }


        public class TempMonthLabelRenderer : IRenderLabel
        {
            private readonly List<int> months;

            public TempMonthLabelRenderer()
            {
                this.months = new List<int>();
            }

            public string ToString(Hashtable context)
            {
                var datum = ((DateTime)context["ITEM_LABEL"]);
                var month = datum.Month;
                if (this.months.Contains(month))
                {
                    return string.Empty;
                }

                var strDatum = String.Format("{0:MMM}", datum);
                if ((month > 5 & month < 9))
                {
                    strDatum = string.Format("({0})", strDatum);
                }
                this.months.Add(month);
                return strDatum;
            }
        }


        //'*** Implementierungs IRenderLabel für die Label(Tooltip)-Formatierung
        public class TemperaturLabelRenderer : IRenderLabel
        {
            public string ToString(Hashtable context)
            {
                DateTime datum = (DateTime)context["TIME_VALUE"];
                double dataValue = Convert.ToDouble(context["DATA_VALUE"]);
                string jahr = Convert.ToString(context["ITEM_LABEL"]);
                //Return Format(dataValue, "0.##°") & " (" & Format(datum, "dd/MM/") & jahr & ")"
                return String.Format("{0:0.##°} ({1:dd/MM})", dataValue, datum);// + " (" + Strings.Format(datum, "dd/MM") + ")";


                //             int row = Convert.ToInt32(context["DATA_ROW"]);
                //         now you have the row index, you can get whatever information you
                //need back from your table.
                //        return _TheData.Rows[row]["Column1"] + _TheData.Rows[row]["Column2"]
                //+ _TheData.Rows[row]["Column1"];

            }
        }

        internal class TemperaturDrillDown : IDrillDown
        {
            private UltraChart _chart;
            public TemperaturDrillDown(UltraChart chart)
            {
                this._chart = chart;

            }
            #region "IDrillDown Members"

            public void Drill(int row, int column, ChartType chartType, object dataSource)
            {
                // Initialize child chart 
                this._chart.ChartType = chartType;
                this._chart.Drill.Enabled = false;
                this._chart.AccessKey = "B";
                // myChart.TabIndex = 99
                this._chart.Data.DataSource = dataSource;
                // myChart.Drill.ShowButton = True
                this._chart.Axis.X.Labels.Orientation = TextOrientation.Horizontal;
                //myChart.Data.IncludeColumn(1, False)
                //myChart.Axis.X.TickmarkIntervalType = AxisIntervalType.Days
                // myChart.Tooltips.FormatString = ""
                this._chart.Data.DataBind();
            }
            #endregion
        }


        private static void TemperaturChartFillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            double target = 15;
            IAdvanceAxis axisY = e.Grid["Y"] as IAdvanceAxis;
            IAdvanceAxis axisX = e.Grid["X"] as IAdvanceAxis;

            int targetYCoord = Convert.ToInt32(axisY.Map(target));
            int xStart = Convert.ToInt32(axisX.MapMinimum);
            int xEnd = Convert.ToInt32(axisX.MapMaximum);

            Line targetLine = new Line(new Point(xStart, targetYCoord), new Point(xEnd, targetYCoord));

            targetLine.PE.Stroke = Color.Green;
            targetLine.PE.StrokeWidth = 1;


            //e.SceneGraph.Add(targetLine)

            Text targetLabel = new Text();
            targetLabel.SetTextString("Heizgrenztemperatur");
            Size targetLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(targetLabel.GetTextString(), targetLabel.labelStyle));
            targetLabel.bounds = new Rectangle(xStart + 10, targetYCoord - targetLabelSize.Height, targetLabelSize.Width, targetLabelSize.Height);
            e.SceneGraph.Add(targetLabel);
        }

    }
}