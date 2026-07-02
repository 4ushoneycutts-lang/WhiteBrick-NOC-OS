using System;
using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class NetworkRenderer
{
    private static readonly Color[] Colors =
    {
        Color.FromArgb(120, 110, 255, 200),
        Color.FromArgb(110, 120, 220, 255),
        Color.FromArgb(100, 160, 210, 255),
        Color.FromArgb(120, 120, 255, 170)
    };

    public void Render(DrawingContext context, double cx, double cy, double radius, double time)
    {
        DrawOrbitalTraffic(context, cx, cy, radius, time);
        DrawPulseLinks(context, cx, cy, radius, time);
    }

    private static void DrawOrbitalTraffic(DrawingContext context, double cx, double cy, double r, double time)
    {
        for (int orbit = 0; orbit < 5; orbit++)
        {
            var ring = r * (1.02 + orbit * .12);
            var flatten = .60 + orbit * .03;
            var color = Colors[orbit % Colors.Length];

            var glowPen = new Pen(
                new SolidColorBrush(Color.FromArgb(22, color.R, color.G, color.B)),
                4);

            var pen = new Pen(new SolidColorBrush(color), 1);

            for (int i = 0; i < 20; i++)
            {
                var a = time * (.45 + orbit * .08) + i * .17;

                var p1 = new Point(
                    cx + Math.Cos(a) * ring,
                    cy + Math.Sin(a) * ring * flatten);

                var p2 = new Point(
                    cx + Math.Cos(a + .12) * ring,
                    cy + Math.Sin(a + .12) * ring * flatten);

                context.DrawLine(glowPen, p1, p2);
                context.DrawLine(pen, p1, p2);
            }

            var packet = (Math.Sin(time * (.8 + orbit * .3)) + 1) * .5;
            var pa = packet * Math.PI * 2;

            var packetPoint = new Point(
                cx + Math.Cos(pa) * ring,
                cy + Math.Sin(pa) * ring * flatten);

            context.DrawEllipse(Brushes.White, null, packetPoint, 2.2, 2.2);
            context.DrawEllipse(new SolidColorBrush(color), null, packetPoint, 6, 6);
        }
    }

    private static void DrawPulseLinks(DrawingContext context, double cx, double cy, double r, double time)
    {
        for (int i = 0; i < 10; i++)
        {
            var a = i * Math.PI / 5 + time * .12;
            var b = a + Math.PI;

            var p1 = new Point(
                cx + Math.Cos(a) * r * .72,
                cy + Math.Sin(a) * r * .72);

            var p2 = new Point(
                cx + Math.Cos(b) * r * .72,
                cy + Math.Sin(b) * r * .72);

            var pulse = .5 + .5 * Math.Sin(time * 2 + i);

            var pen = new Pen(
                new SolidColorBrush(Color.FromArgb((byte)(18 + pulse * 42), 90, 220, 255)),
                1);

            context.DrawLine(pen, p1, p2);
        }
    }
}