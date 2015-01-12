namespace Metrona.Wt.Reports
{
    using System.Drawing;

    public static class Constants
    {
        public const string ImagePipePageName = "ig_res/ImagePipe.aspx";

        public static Color[] ChartColors
        {
            get
            {
                Color[] chartColors = new Color[4];
                chartColors[0] = Color.FromArgb(0, 76, 148);
                chartColors[1] = Color.FromArgb(236, 98, 42);
                chartColors[2] = Color.Green;
                return chartColors;
            }
        }

        public static Color[] YearsChartColors
        {
            get
            {
                Color[] chartColors = new Color[3];
                chartColors[0] = Color.Green;
                chartColors[1] = Color.FromArgb(24, 152, 213);
                chartColors[2] = Color.FromArgb(24, 152, 213);
                return chartColors;
            }
        }
    }
}
