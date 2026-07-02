using System;
using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class GlobeEffectsRenderer
{
    public void Render(DrawingContext context, double cx, double cy, double radius, double time)
    {
        var drift = 0.08 * Math.Sin(time * 0.7 + 0.5);
        var sweep = time * 0.86 + drift;

        DrawOrbitalRing(context, cx, cy, radius * 1.12, 0.26, sweep * 1.18, 0.15, Color.FromArgb(190, 120, 248, 255));
        DrawOrbitalRing(context, cx, cy, radius * 1.28, 0.18, -sweep * 0.92 + 1.2, 0.55, Color.FromArgb(185, 110, 255, 161));
        DrawOrbitalRing(context, cx, cy, radius * 1.46, 0.14, sweep * 0.72 + 2.1, 1.0, Color.FromArgb(175, 166, 126, 255));

        DrawPacketArc(context, cx, cy, radius, sweep * 0.72 + 0.15, Color.FromArgb(165, 36, 215, 255));
        DrawPacketArc(context, cx, cy, radius, sweep * 0.85 + 2.1, Color.FromArgb(165, 142, 248, 255));
        DrawPacketArc(context, cx, cy, radius, -sweep * 0.64 + 1.4, Color.FromArgb(165, 110, 255, 161));

        DrawNetworkTraffic(context, cx, cy, radius, time, sweep);
        DrawTelemetryRings(context, cx, cy, radius, time);
        DrawBeacon(context, cx, cy, radius, time);
    }

    private static void DrawOrbitalRing(
        DrawingContext context,
        double cx,
        double cy,
        double radius,
        double verticalScale,
        double angle,
        double phase,
        Color color)
    {
        var ringColor = Color.FromArgb(65, color.R, color.G, color.B);

        context.DrawEllipse(
            null,
            new Pen(new SolidColorBrush(ringColor), 1),
            new Point(cx, cy),
            radius,
            radius * verticalScale);

        var x = cx + Math.Cos(angle + phase) * radius;
        var y = cy + Math.Sin(angle + phase) * radius * verticalScale;

        context.DrawEllipse(
            new SolidColorBrush(color),
            null,
            new Point(x, y),
            4.3,
            4.3);
    }

    private static void DrawPacketArc(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double phase,
        Color color)
    {
        var pen = new Pen(new SolidColorBrush(color), 1.6);
        var glowPen = new Pen(new SolidColorBrush(Color.FromArgb(70, color.R, color.G, color.B)), 4.2);

        for (int i = 0; i < 5; i++)
        {
            var a = phase + i * 0.10;

            var p1 = new Point(
                cx + Math.Cos(a) * r * 1.10,
                cy + Math.Sin(a) * r * 0.60);

            var p2 = new Point(
                cx + Math.Cos(a + 0.08) * r * 1.10,
                cy + Math.Sin(a + 0.08) * r * 0.60);

            context.DrawLine(glowPen, p1, p2);
            context.DrawLine(pen, p1, p2);
        }

        var packetPhase = (phase + 0.24) % (Math.PI * 2);

        var packetX = cx + Math.Cos(packetPhase) * r * 1.10;
        var packetY = cy + Math.Sin(packetPhase) * r * 0.60;

        context.DrawEllipse(
            new SolidColorBrush(Color.FromArgb(220, 217, 247, 255)),
            null,
            new Point(packetX, packetY),
            2.8,
            2.8);
    }

    private static void DrawNetworkTraffic(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double time,
        double sweep)
    {
        var networkBaseColors = new[]
        {
            Color.FromArgb(120, 110, 255, 200),
            Color.FromArgb(100, 120, 220, 255),
            Color.FromArgb(90, 160, 200, 255)
        };

        const int netArcs = 4;

        for (int ai = 0; ai < netArcs; ai++)
        {
            var nr = r * (1.02 + ai * 0.14);
            var nvs = 0.58 + ai * 0.06;
            var nSpeed = 0.42 + ai * 0.18;
            var nPhase = sweep * (0.6 + ai * 0.2) * (ai % 2 == 0 ? 1 : -1);

            var baseColor = networkBaseColors[ai % networkBaseColors.Length];

            var glowPen = new Pen(
                new SolidColorBrush(Color.FromArgb(30, baseColor.R, baseColor.G, baseColor.B)),
                5.0);

            var thinPen = new Pen(
                new SolidColorBrush(Color.FromArgb(70, baseColor.R, baseColor.G, baseColor.B)),
                1.0);

            const int segments = 18;

            for (int s = 0; s < segments; s++)
            {
                var a1 = nPhase + (s / (double)segments) * Math.PI * 2 * 0.7;
                var a2 = nPhase + ((s + 1) / (double)segments) * Math.PI * 2 * 0.7;

                var p1 = new Point(
                    cx + Math.Cos(a1) * nr,
                    cy + Math.Sin(a1) * nr * nvs);

                var p2 = new Point(
                    cx + Math.Cos(a2) * nr,
                    cy + Math.Sin(a2) * nr * nvs);

                context.DrawLine(glowPen, p1, p2);
                context.DrawLine(thinPen, p1, p2);
            }

            var progress = (Math.Sin(time * (nSpeed * 0.9 + ai * 0.07) + ai * 1.3) + 1.0) * 0.5;
            var packetAngle = nPhase + progress * Math.PI * 2 * 0.7;

            var px = cx + Math.Cos(packetAngle) * nr;
            var py = cy + Math.Sin(packetAngle) * nr * nvs;

            context.DrawEllipse(
                new SolidColorBrush(Color.FromArgb((byte)(180 * progress), 235, 255, 250)),
                null,
                new Point(px, py),
                3.2,
                3.2);

            context.DrawEllipse(
                new SolidColorBrush(Color.FromArgb((byte)(100 + 80 * progress), baseColor.R, baseColor.G, baseColor.B)),
                null,
                new Point(px, py),
                6 + 2 * progress,
                6 + 2 * progress);

            const int nodes = 3;

            for (int ni = 0; ni < nodes; ni++)
            {
                var nodePosition = ni / (double)nodes;

                var nodeAngle =
                    nPhase +
                    nodePosition * Math.PI * 2 * 0.7 +
                    Math.Sin(time * (0.6 + ni * 0.2) + ai) * 0.18;

                var nx = cx + Math.Cos(nodeAngle) * nr;
                var ny = cy + Math.Sin(nodeAngle) * nr * nvs;

                var pulse =
                    0.4 +
                    0.6 *
                    Math.Abs(Math.Sin(time * (1.2 + ni * 0.3) + ai * 0.9 + ni));

                context.DrawEllipse(
                    new SolidColorBrush(Color.FromArgb((byte)(60 + 120 * pulse), baseColor.R, baseColor.G, baseColor.B)),
                    null,
                    new Point(nx, ny),
                    2.0 + 1.6 * pulse,
                    2.0 + 1.6 * pulse);
            }
        }
    }

    private static void DrawTelemetryRings(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double time)
    {
        var telemetryColors = new[]
        {
            Color.FromArgb(28, 180, 220, 255),
            Color.FromArgb(24, 160, 210, 230),
            Color.FromArgb(20, 140, 200, 220)
        };

        const int telemetryRings = 3;

        for (int ri = 0; ri < telemetryRings; ri++)
        {
            var tr = r * (1.06 + ri * 0.12);
            var tvs = 0.20 + ri * 0.06;
            var direction = ri % 2 == 0 ? 1.0 : -1.0;
            var speed = 0.08 + ri * 0.03;
            var seed = ri * 1.37;

            var ringAngle =
                direction *
                (time * speed + Math.Sin(time * (0.05 + ri * 0.02) + seed) * 0.02);

            var ringPen = new Pen(
                new SolidColorBrush(telemetryColors[ri % telemetryColors.Length]),
                1.0);

            context.DrawEllipse(
                null,
                ringPen,
                new Point(cx, cy),
                tr,
                tr * tvs);

            var indicators = 1 + ri % 2;

            for (int ind = 0; ind < indicators; ind++)
            {
                var localOffset = ind * (Math.PI * 0.9 / Math.Max(1, indicators));
                var indicatorSpeed = speed * (1.0 + 0.2 * ind) * (0.8 + 0.15 * Math.Sin(seed + ind));

                var indicatorProgress =
                    (time * indicatorSpeed + seed * 0.23) %
                    (Math.PI * 2);

                var ia = ringAngle + localOffset + indicatorProgress;

                var ix = cx + Math.Cos(ia) * tr;
                var iy = cy + Math.Sin(ia) * tr * tvs;

                var visibility =
                    0.35 +
                    0.65 *
                    Math.Abs(Math.Sin(time * (0.9 + ri * 0.15) + ind * 0.4 + seed));

                context.DrawEllipse(
                    new SolidColorBrush(Color.FromArgb((byte)(40 + 120 * visibility), 225, 245, 255)),
                    null,
                    new Point(ix, iy),
                    2.2,
                    2.2);

                context.DrawEllipse(
                    new SolidColorBrush(Color.FromArgb((byte)(30 * visibility), 180, 220, 255)),
                    null,
                    new Point(ix, iy),
                    5.0 * visibility,
                    2.0 * visibility);
            }
        }
    }

    private static void DrawBeacon(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double time)
    {
        var beaconPulse = 0.55 + 0.45 * Math.Sin(time * 3.2 + 0.4);
        var beaconDrift = 0.6 + 0.4 * Math.Sin(time * 1.1 + 0.2);

        var beaconCenter = new Point(
            cx + r * (0.24 + 0.02 * Math.Sin(time * 0.8)),
            cy - r * (0.18 + 0.01 * Math.Cos(time * 0.9)));

        context.DrawEllipse(
            new SolidColorBrush(Color.FromArgb((byte)(70 + 70 * beaconPulse), 110, 255, 161)),
            null,
            beaconCenter,
            12 + beaconPulse * 8,
            12 + beaconPulse * 8);

        context.DrawEllipse(
            new SolidColorBrush(Color.FromArgb((byte)(145 + 85 * beaconPulse * beaconDrift), 110, 255, 161)),
            null,
            beaconCenter,
            6 + beaconPulse * 6,
            6 + beaconPulse * 6);
    }
}