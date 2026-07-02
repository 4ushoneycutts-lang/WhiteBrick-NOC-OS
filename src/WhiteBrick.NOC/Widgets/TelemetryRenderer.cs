using System;
using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class TelemetryRenderer
{
    public void Render(
        DrawingContext context,
        double cx,
        double cy,
        double radius,
        double time)
    {
        DrawTelemetryRings(context, cx, cy, radius, time);
        DrawCorePulse(context, cx, cy, radius, time);
    }

    private static void DrawTelemetryRings(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double time)
    {
        for (int i = 0; i < 4; i++)
        {
            var ring = r * (0.72 + i * 0.13);
            var flatten = 0.20 + i * 0.045;

            var pen = new Pen(
                new SolidColorBrush(
                    Color.FromArgb(
                        (byte)(26 + i * 8),
                        150,
                        240,
                        255)),
                1);

            context.DrawEllipse(
                null,
                pen,
                new Point(cx, cy),
                ring,
                ring * flatten);

            var angle =
                time * (0.35 + i * 0.12) +
                i * 1.4;

            var p =
                new Point(
                    cx + Math.Cos(angle) * ring,
                    cy + Math.Sin(angle) * ring * flatten);

            context.DrawEllipse(
                Brushes.White,
                null,
                p,
                2,
                2);
        }
    }

    private static void DrawCorePulse(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double time)
    {
        var pulse =
            .5 +
            .5 *
            Math.Sin(time * 2.4);

        context.DrawEllipse(
            new SolidColorBrush(
                Color.FromArgb(
                    (byte)(40 + pulse * 55),
                    110,
                    255,
                    170)),
            null,
            new Point(cx, cy),
            r * (.18 + pulse * .03),
            r * (.18 + pulse * .03));

        context.DrawEllipse(
            Brushes.White,
            null,
            new Point(cx, cy),
            r * .05,
            r * .05);
    }
}