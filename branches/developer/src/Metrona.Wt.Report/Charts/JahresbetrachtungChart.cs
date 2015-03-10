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
    using System.Data;
    using System.Drawing;
    using System.Linq;

    using Infragistics.UltraChart.Core;
    using Infragistics.UltraChart.Core.Primitives;
    using Infragistics.UltraChart.Core.Util;
    using Infragistics.UltraChart.Resources;
    using Infragistics.UltraChart.Resources.Appearance;
    using Infragistics.UltraChart.Shared.Events;
    using Infragistics.UltraChart.Shared.Styles;
    using Infragistics.WebUI.UltraWebChart;

    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Model.Meteo;
    using Metrona.Wt.Service.Extensions;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public static class JahresbetrachtungChart
    {

        private static int MarginX = 20;

        private static bool isPdf;
        private static readonly WebDeploymentScenario deploymentScenario = new WebDeploymentScenario();



        static JahresbetrachtungChart( )
        {
            
            deploymentScenario.Scenario = ImageDeploymentScenario.Session;
        }

        

        public static UltraChart GetChart(MeteoGtzYear meteoGtzYear, int width, int height, bool pdf = false)
        {
            isPdf = pdf;

            var datasource = meteoGtzYear.ToDataTable();

            var chart = new UltraChartEx();
            chart.ExtraData = meteoGtzYear;
            AddAnnotation(chart, meteoGtzYear, pdf);

            chart.FillSceneGraph += ChartOnFillSceneGraph;
            //.ID = "ChartVergleichJahr"
            chart.DeploymentScenario = deploymentScenario;
            chart.Width = width;
            chart.Height = height;
            chart.Border.Thickness = 0;

            var colorModel = chart.ColorModel;
            colorModel.ModelStyle = ColorModels.CustomLinear;
            //.AlphaLevel = 150
            colorModel.CustomPalette = Constants.YearsChartColors;

            var gradientEffect = new GradientEffect
            {
                Coloring = GradientColoringStyle.Darken
            };
            chart.Effects.Add(gradientEffect);


            chart.ChartType = ChartType.ColumnChart;
            chart.ImagePipePageName = Constants.ImagePipePageName;

            var data = chart.Data;
            data.SetColumnLabels(new string[] {
		        "Aktuelles Jahr",
		        "Vorjahr",
                "Vorvorjahr"
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
            columnChart.ColumnSpacing = 4;
            columnChart.SeriesSpacing = 1;


            var legend = chart.Legend;
            legend.Visible = false;            

            var margins = legend.Margins;
            margins.Bottom = 1;
            margins.Top = 1;
            margins.Left = 50;
            margins.Right = 1;

            ////'*** Implementierungs IRenderLabel für die Labels-Formatierung 
            //Hashtable labelHash = new Hashtable();
            //labelHash.Add("MY_LABEL", new MyLabelRenderer());
            //chart.LabelHash = labelHash;
            //chart.Axis.X.Labels.ItemFormat = AxisItemLabelFormat.Custom;
            //chart.Axis.X.Labels.ItemFormatString = "<MY_LABEL>";
            ////'*** END Implementierungs IRenderLabel-Interface für die Labels-Formatierung

            var axisX = chart.Axis.X;
            axisX.Extent = 15;
            //axisX.LineColor = Color.Green;
            axisX.LineThickness = 1;
            axisX.TickmarkStyle = AxisTickStyle.Smart;
            axisX.TickmarkInterval = 1;
            //axisX.Labels.Visible = false;
            axisX.Labels.ItemFormatString = "<ITEM_LABEL>";
            axisX.Labels.Orientation = TextOrientation.Horizontal;
            axisX.Labels.HorizontalAlign = StringAlignment.Near;
            axisX.Labels.VerticalAlign = StringAlignment.Center;
            axisX.Labels.Font = new Font("Verdana", 7, FontStyle.Regular, GraphicsUnit.Point);
            axisX.Labels.FontColor = Color.Black;

            
            axisX.Labels.SeriesLabels.Visible = false;
           
            axisX.MajorGridLines.Visible = false;
            axisX.MinorGridLines.Visible = false;
            axisX.Margin.Near.Value = MarginX;
            axisX.Margin.Far.Value = MarginX;

            var axisY = chart.Axis.Y;
            axisY.Extent = 65;
            axisY.LineThickness = 1;
            axisY.TickmarkInterval =2;
            axisY.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;            
            axisY.Labels.Visible = false;
            axisY.MajorGridLines.Visible = false;
            axisY.MinorGridLines.Visible = false;   
            axisY.LineEndCapStyle = LineCapStyle.ArrowAnchor;
            axisY.RangeType = AxisRangeType.Custom;
            axisY.RangeMax = 4300;
          

            var titleLeft = chart.TitleLeft;
            titleLeft.Visible = true;
            titleLeft.Extent = 15;
            titleLeft.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point);
            titleLeft.HorizontalAlign = StringAlignment.Center;
            //titleLeft.Text = "Monat war im Vergleich zum Langzeitmittel kälter / wärmer [%]";
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

            data.DataSource = datasource;
            data.DataBind();
            try
            {
                data.IncludeColumn("LGTZ", false);
            }
            catch (Exception exception)
            {

            }

            return chart;
        }

        private static void AddAnnotation(UltraChart chart, MeteoGtzYear meteoGtzYear, bool pdf)
        {
            var relativeData = meteoGtzYear.ToRelativeData();

            var textStyle = new LabelStyle
            {
                Font = new Font("Verdana", pdf ? 6.5f : 7.5f, FontStyle.Italic, GraphicsUnit.Point),
                FontColor = Constants.YearsChartColors[1],
                HorizontalAlign = StringAlignment.Center,
                FontSizeBestFit = false,
                ClipText = true,
                Orientation = TextOrientation.Horizontal,
                WrapText = false,
                Flip = true,
                VerticalAlign = StringAlignment.Center,
                Dy = 0
            };
            var ann = new TextAnnatation
            {
                Width = pdf ? 80 : -1,
                Location = new Location
                {
                    Type = LocationType.RowColumn,
                    Row = 0,
                    Column = 1
                },
                Text = GetAnnotationText(relativeData.Period2, "war um"),
                TextStyle = textStyle
            };
            chart.Annotations.Add(ann);

            ann = new TextAnnatation
            {
                Width = pdf ? 80 : -1,
                Location = new Location
                {
                    Type = LocationType.RowColumn,
                    Row = 0,
                    Column = 2,
                    ValueY = meteoGtzYear.Period2
                },
                Text = GetAnnotationText(relativeData.Period3, "war um"),
                TextStyle = textStyle
            };
            chart.Annotations.Add(ann);
        }

        private static string GetAnnotationText(double value, string prefix)
        {
            var text = string.Format(
@"{0} {1}% {2}
als das aktuelle Jahr",
               prefix, Math.Abs(Math.Round(value, 2)), (value > 0 ? "wärmer" : "kälter"));
            return text;
        }



        private static void ChartOnFillSceneGraph(object sender, FillSceneGraphEventArgs e)
        {

            //List<Primitive> boxes = e.SceneGraph.Cast<Primitive>().Where(p => p is Box && p.Row > -1).ToList();

            //foreach (var box in boxes)
            //{
            //    Point loc = ((Box)box).rect.Location;

            //    //loc.Offset(((Box)box).rect.Width, ((Box)box).rect.Height);
            //    loc.Offset(0, -20);

            //    //Rectangle rect = new Rectangle(loc, new Size(((Box)box).rect.Width, 25));

            //    Text t = new Text(loc, box.Value.ToString());
            //    t.SetLabelStyle(new LabelStyle { FontColor = Color.Black });
            //    // new LabelStyle(this.Font, Color.Black, false, false, false, StringAlignment.Center, StringAlignment.Center, TextOrientation.Horizontal)); 
            //    //e.SceneGraph.Add(t);
            //}

            var chart = sender as UltraChartEx;

            if (chart == null)
            {
                return;
            }
            var data = chart.ExtraData as MeteoGtzYear;


            var relativeData = data.ToRelativeData();
           
            //Aktuelles Jahr
            var aktuellJahr = data.Period1; 
            var lgtz = data.Lgtz; 

            var axisY = e.Grid["Y"] as IAdvanceAxis;
            var axisX = e.Grid["X"] as IAdvanceAxis;

            int targetYCoord = Convert.ToInt32(axisY.Map(lgtz));

            int aktuellJahrY = Convert.ToInt32(axisY.Map(aktuellJahr));

            int nullY = Convert.ToInt32(axisY.Map(0));

            var margin = MarginX * 3.1;
            if (!isPdf)
            {
                margin = MarginX * 6.2;
            }

            int xStart = Convert.ToInt32(axisX.MapMinimum - margin);
            int xEnd = Convert.ToInt32(axisX.MapMaximum + margin + (isPdf ? 32 :15));

            Line nullLine = new Line(new Point(xStart, nullY), new Point(xEnd, nullY))
            {
                PE =
                {
                    Stroke = Color.Black,
                    StrokeWidth = 1
                },
                lineStyle =
                {
                    DrawStyle = LineDrawStyle.Solid

                }
            };

            e.SceneGraph.Add(nullLine);

            var annLabel = new Text();
            annLabel.SetTextString(GetAnnotationText(relativeData.Lgtz, "das Langzeitmittel" + Environment.NewLine + "ist"));
            annLabel.SetLabelStyle(new LabelStyle { FontColor = Color.FromArgb(236, 98, 42), Font = new Font("Verdana", isPdf ? 5.6f : 7.5f, FontStyle.Italic, GraphicsUnit.Point) });
            Size annLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(annLabel.GetTextString(), annLabel.labelStyle));
            annLabel.bounds = new Rectangle(xEnd - annLabelSize.Width, targetYCoord - annLabelSize.Height / 2, annLabelSize.Width, annLabelSize.Height);
            e.SceneGraph.Add(annLabel);

            Line targetLine = new Line(new Point(xStart, targetYCoord), new Point(xEnd - annLabelSize.Width - 2, targetYCoord))
            {
                PE =
                {
                    Stroke = Color.FromArgb(236, 98, 42),
                    StrokeWidth = 2
                },
                lineStyle =
                {
                    DrawStyle = LineDrawStyle.Dash
                     
                }
            };
            
            e.SceneGraph.Add(targetLine);

            var targetLabel = new Text();
            targetLabel.SetTextString("Langszeitmittel");
            targetLabel.SetLabelStyle(new LabelStyle { FontColor = Color.FromArgb(236, 98, 42) });
            Size targetLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(targetLabel.GetTextString(), targetLabel.labelStyle));
            targetLabel.bounds = new Rectangle(xStart - (targetLabelSize.Width - (isPdf ? 10 : 0)), targetYCoord - targetLabelSize.Height / 2, targetLabelSize.Width, targetLabelSize.Height);

            e.SceneGraph.Add(targetLabel);

            if (Math.Abs(targetYCoord - aktuellJahrY) < 5)
            {
                return;
            }

            var targetLine2 = new Line(new Point(xStart, aktuellJahrY), new Point(xEnd - annLabelSize.Width - 2, aktuellJahrY))
            {
                PE =
                {
                    Stroke = Color.Black,
                    StrokeWidth = 2
                },
                lineStyle =
                {
                    DrawStyle = LineDrawStyle.Dot
                }

            };

            e.SceneGraph.Add(targetLine2);

            var targetLabel2 = new Text();
            targetLabel2.SetLabelStyle(new LabelStyle { FontColor = Color.Black});
            targetLabel2.SetTextString("100%");
            Size targetLabelSize2 = Size.Ceiling(Platform.GetLabelSizePixels(targetLabel2.GetTextString(), targetLabel2.labelStyle));
            targetLabel2.bounds = new Rectangle(xStart - targetLabelSize2.Width , aktuellJahrY - targetLabelSize2.Height/2, targetLabelSize2.Width, targetLabelSize2.Height);

            e.SceneGraph.Add(targetLabel2);
        }

     

        public class MyLabelRenderer : IRenderLabel
        {
            public string ToString(Hashtable context)
            {
                return context["ITEM_LABEL"].ToString();
            }
        }



    }
}