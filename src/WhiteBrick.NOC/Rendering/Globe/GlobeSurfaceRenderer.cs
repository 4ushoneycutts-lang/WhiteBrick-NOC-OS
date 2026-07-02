using System;
using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class GlobeSurfaceRenderer
{
    public void Render(
        DrawingContext context,
        double cx,
        double cy,
        double radius,
        double time)
    {
        DrawOrbitalRings(context, cx, cy, radius, time);
        DrawNetworkTraffic(context, cx, cy, radius, time);
        DrawTelemetry(context, cx, cy, radius, time);
        DrawBeacon(context, cx, cy, radius, time);
    }

    private static void DrawOrbitalRings(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        DrawRing(context, cx, cy, r * 1.12, .25, t * .90,
            Color.FromArgb(190, 80, 230, 255));

        DrawRing(context, cx, cy, r * 1.28, .18, -t * .72,
            Color.FromArgb(180, 110, 255, 170));

        DrawRing(context, cx, cy, r * 1.44, .12, t * .55,
            Color.FromArgb(180, 180, 160, 255));
    }

    private static void DrawRing(
        DrawingContext context,
        double cx,
        double cy,
        double radius,
        double verticalScale,
        double angle,
        Color color)
    {
        var pen =
            new Pen(
                new SolidColorBrush(
                    Color.FromArgb(
                        60,
                        color.R,
                        color.G,
                        color.B)),
                1);

        context.DrawEllipse(
            null,
            pen,
            new Point(cx, cy),
            radius,
            radius * verticalScale);

        var x =
            cx +
            Math.Cos(angle) *
            radius;

        var y =
            cy +
            Math.Sin(angle) *
            radius *
            verticalScale;

        context.DrawEllipse(
            new SolidColorBrush(color),
            null,
            new Point(x, y),
            4,
            4);
    }

    private static void DrawNetworkTraffic(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        Color[] colors =
        {
            Color.FromArgb(120,100,255,220),
            Color.FromArgb(120,120,220,255),
            Color.FromArgb(120,180,255,220)
        };

        for (int a = 0; a < 4; a++)
        {
            var orbit = r * (1.02 + a * .12);

            var flatten = .60;

            var pen =
                new Pen(
                    new SolidColorBrush(colors[a % colors.Length]),
                    1);

            int segments = 22;

            for (int i = 0; i < segments; i++)
            {
                double aa =
                    t * (.45 + a * .1) +
                    i * .18;

                Point p1 =
                    new(
                        cx + Math.Cos(aa) * orbit,
                        cy + Math.Sin(aa) * orbit * flatten);

                Point p2 =
                    new(
                        cx + Math.Cos(aa + .14) * orbit,
                        cy + Math.Sin(aa + .14) * orbit * flatten);

                context.DrawLine(pen, p1, p2);
            }

            var packet =
                (Math.Sin(t * (.8 + a * .3)) + 1) / 2;

            double pa =
                packet * Math.PI * 2;

            var px =
                cx +
                Math.Cos(pa) *
                orbit;

            var py =
                cy +
                Math.Sin(pa) *
                orbit *
                flatten;

            context.DrawEllipse(
                Brushes.White,
                null,
                new Point(px, py),
                2.5,
                2.5);

            context.DrawEllipse(
                new SolidColorBrush(colors[a % colors.Length]),
                null,
                new Point(px, py),
                6,
                6);
        }
    }

    private static void DrawTelemetry(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        for (int i = 0; i < 3; i++)
        {
            double radius =
                r * (1.06 + i * .10);

            var pen =
                new Pen(
                    new SolidColorBrush(
                        Color.FromArgb(
                            25,
                            180,
                            220,
                            255)),
                    1);

            context.DrawEllipse(
                null,
                pen,
                new Point(cx, cy),
                radius,
                radius * .22);

            double angle =
                t * (.20 + i * .08);

            var x =
                cx +
                Math.Cos(angle) *
                radius;

            var y =
                cy +
                Math.Sin(angle) *
                radius *
                .22;

            context.DrawEllipse(
                new SolidColorBrush(
                    Color.FromArgb(
                        180,
                        230,
                        255,
                        255)),
                null,
                new Point(x, y),
                2,
                2);
        }
    }

    private static void DrawBeacon(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        var pulse =
            .5 +
            .5 *
            Math.Sin(t * 3);

        Point p =
            new(
                cx + r * .24,
                cy - r * .18);

        context.DrawEllipse(
            new SolidColorBrush(
                Color.FromArgb(
                    (byte)(55 + pulse * 55),
                    120,
                    255,
                    170)),
            null,
            p,
            12 + pulse * 8,
            12 + pulse * 8);

        context.DrawEllipse(
            new SolidColorBrush(
                Color.FromArgb(
                    220,
                    120,
                    255,
                    170)),
            null,
            p,
            4,
            4);
    }
}