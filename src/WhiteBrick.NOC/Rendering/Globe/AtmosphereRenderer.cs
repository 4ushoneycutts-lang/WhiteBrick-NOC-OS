using System;
using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class AtmosphereRenderer
{
    public void Render(
        DrawingContext context,
        double cx,
        double cy,
        double radius,
        double time)
    {
        DrawOuterHalo(context, cx, cy, radius, time);
        DrawAtmosphere(context, cx, cy, radius, time);
        DrawBreathingEdge(context, cx, cy, radius, time);
        DrawPolarGlow(context, cx, cy, radius, time);
    }

    private static void DrawOuterHalo(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        var pulse = 0.55 + 0.25 * Math.Sin(t * 1.15);

        var halo = new RadialGradientBrush();

        halo.GradientStops.Add(
            new GradientStop(
                Color.FromArgb(
                    (byte)(95 + pulse * 55),
                    36,
                    215,
                    255), 0));

        halo.GradientStops.Add(
            new GradientStop(
                Color.FromArgb(
                    (byte)(45 + pulse * 20),
                    36,
                    215,
                    255), .55));

        halo.GradientStops.Add(
            new GradientStop(
                Color.FromArgb(
                    0,
                    36,
                    215,
                    255), 1));

        context.DrawEllipse(
            halo,
            null,
            new Point(cx, cy),
            r * 1.82,
            r * 1.82);
    }

    private static void DrawAtmosphere(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        var alpha =
            65 +
            25 *
            Math.Sin(t * 0.80);

        var pen =
            new Pen(
                new SolidColorBrush(
                    Color.FromArgb(
                        (byte)alpha,
                        130,
                        240,
                        255)),
                7);

        context.DrawEllipse(
            null,
            pen,
            new Point(cx, cy),
            r + 2,
            r + 2);
    }

    private static void DrawBreathingEdge(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        var breath =
            .5 +
            .5 *
            Math.Sin(t * 2.3);

        for (int i = 0; i < 5; i++)
        {
            var width =
                2 +
                i * 2;

            var alpha =
                (byte)(30 *
                breath /
                (i + 1));

            var pen =
                new Pen(
                    new SolidColorBrush(
                        Color.FromArgb(
                            alpha,
                            36,
                            215,
                            255)),
                    width);

            context.DrawEllipse(
                null,
                pen,
                new Point(cx, cy),
                r + i * 2,
                r + i * 2);
        }
    }

    private static void DrawPolarGlow(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        var pulse =
            .5 +
            .5 *
            Math.Sin(t * 1.7);

        var brush =
            new SolidColorBrush(
                Color.FromArgb(
                    (byte)(45 + pulse * 40),
                    120,
                    255,
                    180));

        context.DrawEllipse(
            brush,
            null,
            new Point(
                cx,
                cy - r * .72),
            r * .20,
            r * .08);

        context.DrawEllipse(
            brush,
            null,
            new Point(
                cx,
                cy + r * .72),
            r * .20,
            r * .08);
    }
}