using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using WhiteBrick.NOC.Utilities;
using System.Globalization;

namespace WhiteBrick.NOC.Widgets
{
    public class CalibrationOverlayControl : Control
    {
        public int GridSpacing { get; set; } = DebugConfig.CalibrationGridSpacing;

        public override void Render(DrawingContext context)
        {
            var bounds = Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            // Match MainWindow grid margins and column sizes used in production layout.
            double margin = 20;
            double columnSpacing = 18;
            double leftWidth = 360;
            double rightWidth = 360;
            double headerHeight = 72;
            double footerHeight = 56;

            // Compute content area (row 1 in the main Grid)
            double contentX = margin;
            double contentY = margin + headerHeight;
            double contentWidth = Math.Max(0, bounds.Width - 2 * margin - 2 * columnSpacing);
            double centerWidth = Math.Max(0, contentWidth - leftWidth - rightWidth);
            double contentHeight = Math.Max(0, bounds.Height - 2 * margin - headerHeight - footerHeight);

            var leftRect = new Rect(contentX, contentY, leftWidth, contentHeight);
            var centerRect = new Rect(contentX + leftWidth + columnSpacing, contentY, centerWidth, contentHeight);
            var rightRect = new Rect(contentX + leftWidth + columnSpacing + centerWidth + columnSpacing, contentY, rightWidth, contentHeight);

            // Register monitor regions
            LayoutRegistry.Set("Monitor.Left", leftRect);
            LayoutRegistry.Set("Monitor.Center", centerRect);
            LayoutRegistry.Set("Monitor.Right", rightRect);

            // Draw semi-transparent outlines for monitors
            var outlinePen = new Pen(Brushes.Cyan, 2);
            context.DrawRectangle(null, outlinePen, leftRect);
            context.DrawRectangle(null, outlinePen, centerRect);
            context.DrawRectangle(null, outlinePen, rightRect);

            // Draw grid
            var gridPen = new Pen(Brushes.Gray, 1) { DashStyle = new DashStyle(new double[] { 2, 2 }, 0) };
            for (double x = 0; x <= bounds.Width; x += GridSpacing)
            {
                context.DrawLine(gridPen, new Point(x, 0), new Point(x, bounds.Height));
            }
            for (double y = 0; y <= bounds.Height; y += GridSpacing)
            {
                context.DrawLine(gridPen, new Point(0, y), new Point(bounds.Width, y));
            }

            // Draw axis labels along top and left edges every GridSpacing * 2
            var labelBrush = Brushes.White;
            var typeface = Typeface.Default;
            int labelStep = Math.Max(1, GridSpacing * 2);
            for (double x = 0; x <= bounds.Width; x += labelStep)
            {
                var s = ((int)x).ToString(CultureInfo.InvariantCulture);
                var ft = new FormattedText(s, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, 10, labelBrush);
                context.DrawText(ft, new Point(x + 2, 2));
            }
            for (double y = 0; y <= bounds.Height; y += labelStep)
            {
                var s = ((int)y).ToString(CultureInfo.InvariantCulture);
                var ft = new FormattedText(s, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, 10, labelBrush);
                context.DrawText(ft, new Point(2, y + 2));
            }

            // Prepare placeholder rectangles for major widgets
            var padding = 12;
            var opsRect = new Rect(leftRect.X + padding, leftRect.Y + padding, leftRect.Width - padding * 2, leftRect.Height * 0.28);
            var netStatusRect = new Rect(opsRect.X, opsRect.Bottom + 12, opsRect.Width, leftRect.Height * 0.18);

            var heroW = centerRect.Width * 0.78;
            var heroH = centerRect.Height * 0.66;
            var heroRect = new Rect(centerRect.X + (centerRect.Width - heroW) / 2, centerRect.Y + 12, heroW, heroH);

            var packetRect = new Rect(centerRect.X + 12, heroRect.Bottom + 12, centerRect.Width * 0.6 - 24, centerRect.Height - heroH - 36);
            var cameraRect = new Rect(packetRect.Right + 12, heroRect.Bottom + 12, centerRect.Width * 0.4 - 36, packetRect.Height);

            // Right monitor vertical layout (stacked)
            var rightPadding = 12;
            double rightInnerWidth = rightRect.Width - rightPadding * 2;
            double rightY = rightRect.Y + rightPadding;
            var liveConsoleRect = new Rect(rightRect.X + rightPadding, rightY, rightInnerWidth, rightRect.Height * 0.18);
            rightY += liveConsoleRect.Height + 12;
            var weatherRect = new Rect(rightRect.X + rightPadding, rightY, rightInnerWidth, rightRect.Height * 0.16);
            rightY += weatherRect.Height + 12;
            var radarRect = new Rect(rightRect.X + rightPadding, rightY, rightInnerWidth, rightRect.Height * 0.18);
            rightY += radarRect.Height + 12;
            var camerasRect = new Rect(rightRect.X + rightPadding, rightY, rightInnerWidth, rightRect.Height * 0.16);
            rightY += camerasRect.Height + 12;
            var packetInspectorRect = new Rect(rightRect.X + rightPadding, rightY, rightInnerWidth, rightRect.Bottom - rightY - rightPadding);

            // Footer rect (the bottom bar)
            var footerRect = new Rect(margin, bounds.Height - margin - footerHeight, bounds.Width - margin * 2, footerHeight);

            // Register widget rectangles
            LayoutRegistry.Set("Widget.Operations", opsRect);
            LayoutRegistry.Set("Widget.NetworkStatus", netStatusRect);
            LayoutRegistry.Set("Widget.HeroGlobe", heroRect);
            LayoutRegistry.Set("Widget.PacketStream", packetRect);
            LayoutRegistry.Set("Widget.CameraWall", cameraRect);
            LayoutRegistry.Set("Widget.LiveConsole", liveConsoleRect);
            LayoutRegistry.Set("Widget.Weather", weatherRect);
            LayoutRegistry.Set("Widget.Radar", radarRect);
            LayoutRegistry.Set("Widget.Cameras", camerasRect);
            LayoutRegistry.Set("Widget.PacketInspector", packetInspectorRect);
            LayoutRegistry.Set("Widget.Footer", footerRect);

            // Draw placeholders with labels
            DrawPlaceholder(context, opsRect, "Operations");
            DrawPlaceholder(context, netStatusRect, "Network Status");
            DrawPlaceholder(context, heroRect, "Hero Globe");
            DrawPlaceholder(context, packetRect, "Packet Stream");
            DrawPlaceholder(context, cameraRect, "Camera Wall");
            DrawPlaceholder(context, liveConsoleRect, "Live Console");
            DrawPlaceholder(context, weatherRect, "Weather");
            DrawPlaceholder(context, radarRect, "Radar");
            DrawPlaceholder(context, camerasRect, "Cameras");
            DrawPlaceholder(context, packetInspectorRect, "Packet Inspector");
            DrawPlaceholder(context, footerRect, "Footer");
        }

        private void DrawPlaceholder(DrawingContext context, Rect r, string title)
        {
            var fill = new SolidColorBrush(Color.FromArgb(48, 0, 200, 200));
            var stroke = new Pen(new SolidColorBrush(Color.FromArgb(200, 0, 200, 200)), 2);
            context.DrawRectangle(fill, stroke, r);

            var typeface2 = Typeface.Default;
            var titleText = new FormattedText(title, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface2, 14, Brushes.White);
            context.DrawText(titleText, new Point(r.X + 6, r.Y + 6));

            var info = $"X:{(int)r.X} Y:{(int)r.Y} W:{(int)r.Width} H:{(int)r.Height}";
            var infoText = new FormattedText(info, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface2, 11, Brushes.LightGray);
            context.DrawText(infoText, new Point(r.X + 6, r.Y + 28));
        }
    }
}
