using System;
using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class NetworkRenderer
{
    private static readonly Color[] Colors =
    {
        Color.FromArgb(120,110,255,200),
        Color.FromArgb(110,120,220,255),
        Color.FromArgb(100,160,210,255),
        Color.FromArgb(120,120,255,170)
    };

    public void Render(
        DrawingContext context,
        double cx,
        double cy,
        double radius,
        double time)
    {
        DrawOrbitalTraffic(
            context,
            cx,
            cy,
            radius,
            time);

        DrawPulseLinks(
            context,
            cx,
            cy,
            radius,
            time);
    }

    private static void DrawOrbitalTraffic(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        for (int orbit = 0; orbit < 5; orbit++)
        {
            var ring =
                r *
                (1.02 + orbit * .12);

            var flatten =
                .60 +
                orbit * .03;

            var color =
                Colors[orbit % Colors.Length];

            var glow =
                new Pen(
                    new SolidColorBrush(
                        Color.FromArgb(
                            22,
                            color.R,
                            color.G,
                            color.B)),
                    4);

            var pen =
                new Pen(
                    new SolidColorBrush(color),
                    1);

            for (int i = 0; i < 20; i++)
            {
                var a =
                    t *
                    (.45 + orbit * .08)
                    + i * .17;

                Point p1 =
                    new(
                        cx + Math.Cos(a) * ring,
                        cy + Math.Sin(a) * ring * flatten);

                Point p2 =
                    new(
                        cx + Math.Cos(a + .12) * ring,
                        cy + Math.Sin(a + .12) * ring * flatten);

                context.DrawLine(glow, p1, p2);
                context.DrawLine(pen, p1, p2);
            }

            var packet =
                (Math.Sin(
                    t *
                    (.8 + orbit * .3))
                    + 1) * .5;

            var pa =
                packet *
                Math.PI *
                2;

            var px =
                cx +
                Math.Cos(pa) *
                ring;

            var py =
                cy +
                Math.Sin(pa) *
                ring *
                flatten;

            context.DrawEllipse(
                Brushes.White,
                null,
                new Point(px, py),
                2,
                2);

            context.DrawEllipse(
                new SolidColorBrush(color),
                null,
                new Point(px, py),
                6,
                6);
        }
    }

    private static void DrawPulseLinks(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double t)
    {
        for (int i = 0; i < 10; i++)
        {
            var a =
                i *
                Math.PI /
                5 +
                t * .12;

            var b =
                a +
                Math.PI;

            var p1 =
                new Point(
                    cx + Math.Cos(a) * r * .72,
                    cy + Math.Sin(a) * r * .72);

            var p2 =
                new Point(
                    cx + Math.Cos(b) * r * .72,
                    cy + Math.Sin(b) * r * .72);

            var pulse =
                .5 +
                .5 *
                Math.Sin(
                    t * 2 +
                    i);

            var pen =
                new Pen(
                    new SolidColorBrush(
                        Color.FromArgb(
                            (byte)(18 + pulse * 42),
                            90,
                            220,
                            255)),
                    1);

            context.DrawLine(
                pen,
                p1,
                p2);
        }
    }
}