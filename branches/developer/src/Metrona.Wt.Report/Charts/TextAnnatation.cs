//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TextAnnatation.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Reports.Charts
{
    using System.Drawing;

    using Infragistics.UltraChart.Core;
    using Infragistics.UltraChart.Core.Primitives;
    using Infragistics.UltraChart.Core.Util;
    using Infragistics.UltraChart.Resources;
    using Infragistics.UltraChart.Resources.Appearance;
    using Infragistics.UltraChart.Shared.Styles;

    internal class TextAnnatation : CalloutAnnotation
    {
        public override void RenderAnnotation(SceneGraph scene, Point renderPoint)
        {
            if (renderPoint.Y < 0)
            {
                return;
            }

            var bubbleSize = this.GetBubbleSize();
            int width = bubbleSize.Width;
            int height = bubbleSize.Height;
            var bubbleRect = new Rectangle(renderPoint.X - width / 2, 0, width, height);
            this.RenderLabel(scene, bubbleRect);
        }

        private Size GetBubbleSize()
        {
            var sizeF = this.TextStyle == null
                ? Platform.GetStringSizePixels(this.Text, DefaultConstants.D_TextFont)
                : Platform.GetStringSizePixels(this.Text, this.TextStyle.Font);
            int height = this.Height >= 0 ? this.Height : (int)(sizeF.Height * 1.1);
            return new Size(this.Width >= 0 ? this.Width : (int)(sizeF.Width * 1.1), height);
        }

        private void RenderLabel(SceneGraph scene, Rectangle bubbleRect)
        {
            var label = new Text(bubbleRect, this.Text, this.TextStyle.Clone());
            this.SetTextSetting(label);
            if (bubbleRect.Width <= 0 || bubbleRect.Height <= 0)
            {
                return;
            }
            label.labelStyle.SetNoUpdate(true);
            label.labelStyle.Orientation = TextOrientation.Horizontal;
            label.labelStyle.SetNoUpdate(false);

            scene.Add(label);
        }
    }
}