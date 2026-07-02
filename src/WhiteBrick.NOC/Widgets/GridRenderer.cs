using System;
using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed partial class GridRenderer
{
    public void Render(DrawingContext context, double cx, double cy, double radius, double time)
    {
        DrawLatitudeLines(context, cx, cy, radius);
        DrawLongitudeLines(context, cx, cy, radius, time);
        DrawEquatorBand(context, cx, cy, radius, time);
    }

    private static void DrawLatitudeLines(DrawingContext context, double cx, double cy, double r)
    {
        var pen = new Pen(new SolidColorBrush(Color.FromArgb(120, 110, 255, 245)), 1.1);

        for (int i = -3; i <= 3; i++)
        {
            var yRadius = r * Math.Cos(i * Math.PI / 8);
            context.DrawEllipse(null, pen, new Point(cx, cy), r, Math.Abs(yRadius));
        }
    }

    private static void DrawLongitudeLines(DrawingContext context, double cx, double cy, double r, double time)
    {
        var pen = new Pen(new SolidColorBrush(Color.FromArgb(115, 110, 255, 245)), 1.0);

        for (int i = 0; i < 10; i++)
        {
            var angle = time * 0.42 + i * Math.PI / 10;
            var xScale = 0.72 + 0.28 * Math.Abs(Math.Cos(angle));
            var yScale = 0.92 + 0.08 * Math.Sin(angle * 0.7 + 0.5);

            context.DrawEllipse(null, pen, new Point(cx, cy), r * xScale, r * yScale);
        }
    }

    private static void DrawEquatorBand(DrawingContext context, double cx, double cy, double r, double time)
    {
        var pulse = 0.55 + 0.45 * Math.Sin(time * 1.4 + 0.6);

        var glowPen = new Pen(
            new SolidColorBrush(Color.FromArgb((byte)(18 + 24 * pulse), 36, 215, 255)),
            5.2);

        var pen = new Pen(
            new SolidColorBrush(Color.FromArgb((byte)(45 + 45 * pulse), 142, 248, 255)),
            1.6);

        context.DrawEllipse(null, glowPen, new Point(cx, cy), r * 1.01, r * 0.18);
        context.DrawEllipse(null, pen, new Point(cx, cy), r * 1.01, r * 0.18);
    }
}